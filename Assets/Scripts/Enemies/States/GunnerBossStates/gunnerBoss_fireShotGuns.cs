using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class gunnerBoss_fireShotGuns : State<GunnerBoss>
{
    private static gunnerBoss_fireShotGuns instance;
    private gunnerBoss_fireShotGuns()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static gunnerBoss_fireShotGuns Instance
    {
        get
        {
            if (instance == null)
            {
                new gunnerBoss_fireShotGuns();
            }
            return instance;
        }
    }

    float timer; // initial attack wind-up
    float secondShotDelay; // timer delay for second shotGun burst
    bool shotFirst; // to determine if the boss has shot his first shotgun
    Vector3 dir;

    public override void EnterState(GunnerBoss owner)
    {
        timer = 0.75f;
        secondShotDelay = 0.4f;
        shotFirst = false;
    }

    public override void ExitState(GunnerBoss owner)
    {
    }

    public override void FixedUpdateState(GunnerBoss owner)
    {
        if (timer > 0)
            owner.TurnToFace(owner.AttackTarget.position, 7);
    }

    public override void UpdateState(GunnerBoss owner)
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            if (!shotFirst)
            {
                EnemyWeapon shotGun1 = owner.shotGunTurret1;

                dir = owner.transform.position + owner.transform.forward * 100;

                shotGun1.FireWeapon(dir, -5);
                shotGun1.FireWeapon(dir, -15);
                shotGun1.FireWeapon(dir, 5);
                shotGun1.FireWeapon(dir, 15);

                shotFirst = true;
            }

            if (secondShotDelay > 0)
                secondShotDelay -= Time.deltaTime;
            else
            {
                EnemyWeapon shotGun2 = owner.shotGunTurret2;

                shotGun2.FireWeapon(dir, -5);
                shotGun2.FireWeapon(dir, -15);
                shotGun2.FireWeapon(dir, 5);
                shotGun2.FireWeapon(dir, 15);

                owner.stateMachine.ChangeState(gunnerBoss_phase1.Instance);
            }
        }
    }
}
