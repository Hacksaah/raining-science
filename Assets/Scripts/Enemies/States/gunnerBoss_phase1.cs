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
        owner.currTarget = owner.AttackTarget.position;
    }

    public override void ExitState(GunnerBoss owner)
    {

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
            timeBetweenThoughts = Random.Range(3f, 5);
            int chance = Random.Range(0, 1000 / attackCount);
            if (owner.moveTargetIndex == -1 && Random.Range(0, 100) < 25)
                owner.StartCoroutine(owner.RequestMovePathToAttack());
            else if (chance < 20)
            {
                owner.StartCoroutine(owner.SpawnExplosiveBot());
                owner.currTarget = owner.AttackTarget.transform.position;
            }
            else
            {
                switch(Random.Range(0, 2))
                {
                    case 0:
                        owner.StartCoroutine(owner.FireShotGuns());
                        break;
                    case 1:
                        owner.StartCoroutine(owner.FireSpinningTurret(Random.Range(5, 10)));
                        break;
                    case 2:
                        owner.StartCoroutine(owner.FireShotGuns());
                        owner.StartCoroutine(owner.FireSpinningTurret(Random.Range(7, 10)));
                        break;
                }
            }
        }
    }
}
