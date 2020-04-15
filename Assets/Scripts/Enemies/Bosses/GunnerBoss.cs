using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class GunnerBoss : EnemyActor
{    
    private StateMachine<GunnerBoss> stateMachine;

    public EnemyWeapon shotGunTurret1;
    public EnemyWeapon shotGunTurret2;
    public EnemyWeapon spinningTurret;

    public Transform spinTurretTurnPosition;
    public GameObject HealthOrb_GameObj;

    public VarInt bossMaxHP;
    public VarInt bossCurrHP;

    private bool spinAttack = false;
    private Vector3 spinTurretOffest;

    GunnerBoss()
    {
        stateMachine = new StateMachine<GunnerBoss>(this);
    }

    private void Awake()
    {
        HealthOrb_GameObj = Instantiate(HealthOrb_GameObj);
        // ignore collision between this actor and the health orb
        Collider col = HealthOrb_GameObj.GetComponent<Collider>();
        Physics.IgnoreCollision(col, GetComponent<Collider>(), true);

        Startup();
        ResetActor();

        //Test stuff
        roomKey = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        AttackTarget = GameObjectPoolManager.PlayerTarget;
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
            stateMachine.ChangeState(gunnerBoss_intermission.Instance);
        }
    }

    public IEnumerator FireShotGuns()
    {
        Vector3 dir = transform.position + transform.forward * 100;

        shotGunTurret1.FireWeapon(dir, -5);
        shotGunTurret1.FireWeapon(dir, -15);
        shotGunTurret1.FireWeapon(dir, 5);
        shotGunTurret1.FireWeapon(dir, 15);

        float timer = Random.Range(0.2f, 0.4f);
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
        HealthOrb_GameObj.transform.position = transform.position;
        HealthOrb_GameObj.transform.LookAt(transform.forward);
        HealthOrb_GameObj.SetActive(true);
    }

    public IEnumerator RetreiveHealthOrb()
    {
        //# to-do
        stateMachine.HaltState();

        //Retreive path to orb
        currTarget = HealthOrb_GameObj.transform.position;
        RequestPath();
        while(moveTargetIndex < 0)
            yield return null;
        int lenght = movePath.Length;
        float moveSpeed = stats.GetMoveSpeed();
        float moveSpeedRampUP = 1.0f;
        while(moveTargetIndex < lenght)
        {
            TurnToFace(currTarget, 8f);
            if (moveTargetIndex < lenght - 1) {
                transform.position = Vector3.MoveTowards(transform.position, currTarget, moveSpeed * moveSpeedRampUP * Time.deltaTime);
                moveSpeedRampUP += Time.deltaTime * 2;
                if (Vector3.Distance(currTarget, transform.position) < 2.0f)
                {
                    moveTargetIndex++;
                    currTarget = movePath[moveTargetIndex];
                }
            }
            else
            {
                currTarget = HealthOrb_GameObj.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, HealthOrb_GameObj.transform.position, moveSpeed * moveSpeedRampUP * Time.deltaTime);
                if (Vector3.Distance(transform.position, HealthOrb_GameObj.transform.position) < 6.0f)
                    moveTargetIndex++;
            }
            yield return null;
        }
        moveTargetIndex = -1;

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
        if(bossCurrHP.value <= 0)
        {
            //blow up
        }

        stateMachine.ChangeState(gunnerBoss_phase1.Instance);
    }

    public IEnumerator SpawnExplosiveBot()
    {
        GameObject bot = GameObjectPoolManager.RequestItemFromPool("deliBot");
        bot.transform.position = shotGunTurret2.transform.GetChild(0).transform.position;
        Vector3 target = bot.transform.position + transform.forward;
        target.y = bot.transform.position.y;
        bot.transform.LookAt(target);
        yield return null;
    }
}
