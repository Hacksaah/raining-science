using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class GunnerBoss : EnemyActor
{    
    private StateMachine<GunnerBoss> stateMachine;

    public VarInt BossCount;

    public EnemyWeapon shotGunTurret1;
    public EnemyWeapon shotGunTurret2;
    public EnemyWeapon spinningTurret;

    public ParticleSystem explosiveParticles;
    
    public GameObject HealthOrb_GameObj;

    public VarInt bossMaxHP;
    public VarInt bossCurrHP;

    [HideInInspector]
    public bool pathNotDone = false;
    [HideInInspector]
    public bool spinAttack = false;
    private bool retreivingOrb = false;

    private Transform spinTurretTurnPosition;
    private Vector3 spinTurretOffest;

    GunnerBoss()
    {
        stateMachine = new StateMachine<GunnerBoss>(this);
        size = 3f;
    }

    private void Awake()
    {
        spinTurretTurnPosition = transform.GetChild(3);
        explosiveParticles = transform.GetChild(4).GetComponent<ParticleSystem>();
        HealthOrb_GameObj = Instantiate(HealthOrb_GameObj);
        // ignore collision between this actor and the health orb
        Collider col = HealthOrb_GameObj.GetComponent<Collider>();
        Physics.IgnoreCollision(col, GetComponent<Collider>(), true);

        Startup();
        ResetActor();

        BossUI.Instance.ReadyHealthBar(bossMaxHP.value);
        stateMachine.ChangeState(gunnerBoss_phase1.Instance);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public override void TakeDamage(int incomingDamage, Vector3 force, Damage_Type dam_Type)
    {
        if (dam_Type == Damage_Type.EXPLOSIVE)
        {
            rb.isKinematic = true;
            if(!retreivingOrb)
                stateMachine.ChangeState(gunnerBoss_intermission.Instance);
        }
    }

    public IEnumerator RequestMovePathToAttack()
    {
        currTarget = PathRequestManager.FindOpenMoveSpotBetween(AttackTarget.position, 2, 6, roomKey);
        RequestPath();
        pathNotDone = true;
        while (moveTargetIndex == -1)
            yield return null;
        
        currTarget.y = transform.position.y;
        float moveSpeed = stats.GetMoveSpeed();
        int lenght = movePath.Count;
        while (moveTargetIndex < lenght)
        {            
            transform.position = Vector3.MoveTowards(transform.position, currTarget, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(currTarget, transform.position) < 2.0f)
            {
                moveTargetIndex++;
                if(moveTargetIndex < lenght)
                {
                    currTarget = movePath[moveTargetIndex];
                    currTarget.y = transform.position.y;
                }
                    
            }
            yield return null;
        }
        moveTargetIndex = -1;
        pathNotDone = false;
        currTarget = AttackTarget.position;
    }

    public IEnumerator FireShotGuns()
    {
        currTarget = AttackTarget.transform.position;

        float timer = 0.75f;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        Vector3 dir = transform.position + transform.forward * 100;

        shotGunTurret1.FireWeapon(dir, -5);
        shotGunTurret1.FireWeapon(dir, -15);
        shotGunTurret1.FireWeapon(dir, 5);
        shotGunTurret1.FireWeapon(dir, 15);

        timer = Random.Range(0.2f, 0.4f);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        
        shotGunTurret2.FireWeapon(dir, -5);
        shotGunTurret2.FireWeapon(dir, -15);
        shotGunTurret2.FireWeapon(dir, 5);
        shotGunTurret2.FireWeapon(dir, 15);
    }

    public IEnumerator FireSpinningTurret(float duration)
    {
        if (!spinAttack)
        {
            spinAttack = true;
            float timer = 0;
            while (duration > 0)
            {
                float deltaTime = Time.deltaTime;
                duration -= deltaTime;
                timer -= deltaTime;
                if (timer < 0)
                {
                    spinningTurret.FireWeapon(spinningTurret.transform.position + (spinningTurret.transform.forward * 5), 0);
                    timer = 0.2f;
                }
                spinningTurret.transform.RotateAround(spinTurretTurnPosition.position, Vector3.up, 0.75f);
                spinTurretOffest = spinningTurret.transform.position - transform.position;
                yield return null;
            }
            spinAttack = false;
        }
    }

    public void EjectHealthOrb()
    {
        StopAllCoroutines();
        retreivingOrb = true;
        HealthOrb_GameObj.transform.position = transform.position;
        HealthOrb_GameObj.transform.LookAt(transform.forward);
        HealthOrb_GameObj.SetActive(true);
    }

    public IEnumerator RetreiveHealthOrb()
    {
        stateMachine.HaltState();        

        //Retreive path to orb
        currTarget = HealthOrb_GameObj.transform.position;
        RequestPath();
        while (moveTargetIndex < 0)
        {
            yield return null;
        }
        //Found a path to orb

        // Moving to orb
        currTarget.y = transform.position.y;
        int lenght = movePath.Count;
        float moveSpeed = stats.GetMoveSpeed();
        float moveSpeedRampUP = 1.0f;
        while (moveTargetIndex != lenght)
        {
            TurnToFace(currTarget, 8f);
            transform.position = Vector3.MoveTowards(transform.position, currTarget, moveSpeed * moveSpeedRampUP * Time.deltaTime);
            moveSpeedRampUP += Time.deltaTime;
            if (moveTargetIndex == lenght - 1 && Vector3.Distance(transform.position, HealthOrb_GameObj.transform.position) <= 6.0f)
                break;
            else if (Vector3.Distance(currTarget, transform.position) < 2.0f)
            {
                moveTargetIndex++;
                if (moveTargetIndex < lenght)
                {
                    currTarget = movePath[moveTargetIndex];
                    currTarget.y = transform.position.y;
                }
                else
                {
                    currTarget = HealthOrb_GameObj.transform.position;
                    moveTargetIndex = -1;
                    RequestPath();
                    while (moveTargetIndex < 0)
                    {
                        yield return null;
                    }                        
                    lenght = movePath.Count;
                    currTarget.y = transform.position.y;
                    if (lenght == 0)
                        break;
                }
            }
            yield return null;
        }
        moveTargetIndex = -1;
        StartCoroutine(PickUpHealthOrb());
    }

    private IEnumerator PickUpHealthOrb()
    {
        //Pick it up
        Rigidbody orbRB = HealthOrb_GameObj.GetComponent<Rigidbody>();
        orbRB.velocity = Vector3.zero;
        orbRB.isKinematic = true;
        Vector3 position = transform.position;
        while (HealthOrb_GameObj.transform.position != position)
        {
            HealthOrb_GameObj.transform.position = Vector3.MoveTowards(HealthOrb_GameObj.transform.position, position, 3.5f * Time.deltaTime);
            yield return null;
        }
        orbRB.isKinematic = false;
        HealthOrb_GameObj.SetActive(false);

        //Change state to attack phase
        if (bossCurrHP.value <= 0)
        {
            StartCoroutine(Explode());
        }
        else
        {
            retreivingOrb = false;
            stateMachine.ChangeState(gunnerBoss_phase1.Instance);
        }
    }

    public IEnumerator SpawnExplosiveBot()
    {
        GameObject bot = GameObjectPoolManager.Instance.RequestItemFromPool("deliBot");
        bot.transform.position = shotGunTurret2.transform.GetChild(0).transform.position;
        Vector3 target = bot.transform.position + transform.forward;
        target.y = bot.transform.position.y;
        bot.transform.LookAt(target);
        yield return null;
    }

    private IEnumerator Explode()
    {
        float timer = 0;
        float startX = transform.position.x;
        float startZ = transform.position.z;
        while(timer < 2.1f)
        {
            Vector3 newPos = transform.position;
            float diff = timer * .2f;
            newPos.x = Random.Range(startX - diff, startX + diff);
            newPos.z = Random.Range(startZ - diff, startZ + diff);
            transform.position = newPos;

            timer += Time.deltaTime;
            yield return null;
        }

        rb.constraints = RigidbodyConstraints.None;
        rb.isKinematic = false;
        rb.mass = 1;
        Vector3 dir = -transform.forward + Vector3.up;
        Vector3 position = transform.position;
        position.y = position.y + 1;
        rb.AddForceAtPosition(dir * 4, position, ForceMode.Impulse);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10, 11);
        foreach (Collider col in hitColliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (col.gameObject.tag == "Player")
                {

                    col.gameObject.GetComponent<PlayerController>().TakeDamage(40);
                }
                else if (col.gameObject.tag == "Enemy")
                {
                    col.GetComponent<EnemyActor>().TakeDamage(40, Vector3.zero, Damage_Type.EXPLOSIVE);
                }
                rb.isKinematic = false;
                Vector3 explosiveForce = (rb.position - transform.position).normalized;
                explosiveForce.y = 0.65f;
                explosiveForce *= 10;
                rb.AddForce(explosiveForce, ForceMode.Impulse);                
            }
        }
        explosiveParticles.Play();
        BossCount.value--;
        yield return null;
    }
}
