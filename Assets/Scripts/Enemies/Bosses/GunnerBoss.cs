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

    private bool spinAttack = false;
    private Vector3 spinTurretOffest;

    GunnerBoss()
    {
        stateMachine = new StateMachine<GunnerBoss>(this);
    }

    private void Awake()
    {
        spinTurretOffest = spinningTurret.transform.position - spinTurretTurnPosition.position;
        transform.GetChild(2).parent = null;
        HealthOrb_GameObj = Instantiate(HealthOrb_GameObj);        
        Startup();
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

        spinningTurret.transform.position = transform.position + spinTurretOffest;        
    }

    public override void TakeDamage(int incomingDamage, Vector3 force, Damage_Type dam_Type)
    {
        if (dam_Type == Damage_Type.EXPLOSIVE)
        {
            stateMachine.ChangeState(gunnerBoss_intermission.Instance);
        }
    }

    public IEnumerator FireShotGuns()
    {        
        Vector3 dir = shotGunTurret1.transform.position + shotGunTurret1.transform.forward*2;

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

        dir = shotGunTurret2.transform.position + shotGunTurret1.transform.forward * 2;
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
            //Retreive path to orb
            //Pick it up
            //Change state to attack phase
        yield return null;
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
