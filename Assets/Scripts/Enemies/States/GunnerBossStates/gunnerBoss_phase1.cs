using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class gunnerBoss_phase1 : State<GunnerBoss>
{
    private static gunnerBoss_phase1 instance;
    private gunnerBoss_phase1()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static gunnerBoss_phase1 Instance
    {
        get
        {
            if (instance == null)
            {
                new gunnerBoss_phase1();
            }
            return instance;
        }
    }

    private int attackCount = 0;
    private float timeBetweenThoughts = 0;
    private float turnSpeed = 7;

    public override void EnterState(GunnerBoss owner)
    {
        Debug.Log("Entering Phase1");
        owner.currTarget = owner.AttackTarget.position;
    }

    public override void ExitState(GunnerBoss owner)
    {
        owner.spinAttack = false;
        owner.pathNotDone = false;
    }

    public override void FixedUpdateState(GunnerBoss owner)
    {
        owner.TurnToFace(owner.currTarget, turnSpeed);
    }

    public override void UpdateState(GunnerBoss owner)
    {
        if(timeBetweenThoughts > 0 || owner.pathNotDone)
        {
            timeBetweenThoughts -= Time.deltaTime;
            if (owner.moveTargetIndex < 0)
                owner.currTarget = owner.AttackTarget.position;
        }
        else
        {
            attackCount++;
            timeBetweenThoughts = Random.Range(1.2f, 2f);
            int chance = Random.Range(0, 1000 / attackCount);
            if (owner.moveTargetIndex == -1 && Random.Range(0, 100) < 15)
            {
                owner.stateMachine.ChangeState(gunnerBoss_reposition.Instance);
                //owner.StartCoroutine(owner.RequestMovePathToAttack());
            }                
            else if (chance < 20 || attackCount > 3)
            {
                owner.SpawnExplosiveBot();
                attackCount = 0;
            }
            else
            {
                // Fire guns
                switch(Random.Range(0, 2))
                {
                    case 0:
                        //owner.StartCoroutine(owner.FireShotGuns());
                        owner.stateMachine.ChangeState(gunnerBoss_fireShotGuns.Instance);
                        break;
                    case 1:
                        owner.StartCoroutine(owner.FireSpinningTurret(Random.Range(5, 10)));
                        break;
                    case 2:
                        //owner.StartCoroutine(owner.FireShotGuns());                       
                        owner.StartCoroutine(owner.FireSpinningTurret(Random.Range(7, 10)));
                        owner.stateMachine.ChangeState(gunnerBoss_fireShotGuns.Instance);
                        break;
                }
            }
        }
    }
}
