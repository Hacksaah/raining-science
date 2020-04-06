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


    GunnerBoss()
    {
        stateMachine = new StateMachine<GunnerBoss>(this);
    }

    private void Awake()
    {
        Startup();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(FireSpinningTurret(10));
    }

    public IEnumerator FireShotGuns()
    {
        shotGunTurret1.FireWeapon(AttackTarget.position, -5);
        shotGunTurret1.FireWeapon(AttackTarget.position, -15);
        shotGunTurret1.FireWeapon(AttackTarget.position, 5);
        shotGunTurret1.FireWeapon(AttackTarget.position, 15);

        float timer = Random.Range(0.2f, 0.4f);
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }           

        shotGunTurret2.FireWeapon(AttackTarget.position, -5);
        shotGunTurret2.FireWeapon(AttackTarget.position, -15);
        shotGunTurret2.FireWeapon(AttackTarget.position, 5);
        shotGunTurret2.FireWeapon(AttackTarget.position, 15);
    }

    public IEnumerator FireSpinningTurret(float duration)
    {
        float timer = 0;
        while (duration > 0)
        {
            float deltaTime = Time.deltaTime;
            duration -= deltaTime;
            timer -= deltaTime;
            if(timer < 0)
            {
                spinningTurret.FireWeapon(spinningTurret.transform.forward, 0);
                timer = 0.5f;
            }
            spinningTurret.transform.RotateAround(spinTurretTurnPosition.position, Vector3.up, 0.5f);
            
            yield return null;
        }
    }
}
