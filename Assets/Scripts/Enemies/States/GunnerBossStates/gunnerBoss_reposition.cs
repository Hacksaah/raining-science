using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class gunnerBoss_reposition : State<GunnerBoss>
{
    private static gunnerBoss_reposition instance;
    private gunnerBoss_reposition()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static gunnerBoss_reposition Instance
    {
        get
        {
            if (instance == null)
            {
                new gunnerBoss_reposition();
            }
            return instance;
        }
    }

    float moveSpeed;

    public override void EnterState(GunnerBoss owner)
    {
        Debug.Log("Entering reposition");

        moveSpeed = owner.stats.GetMoveSpeed();

        //owner.currTarget = PathRequestManager.FindOpenMoveSpotBetween(owner.AttackTarget.position, 2, 6, owner.roomKey);
        owner.currTarget = Level_Grid.Instance.GetRoom(owner.roomKey).AnOpenSpot();
        owner.RequestPath();
    }

    public override void ExitState(GunnerBoss owner)
    {
    }

    public override void FixedUpdateState(GunnerBoss owner)
    {
        owner.TurnToFace(owner.currTarget, 7f);
    }

    public override void UpdateState(GunnerBoss owner)
    {
        // if a path exists
        if(owner.moveTargetIndex >= 0)
        {
            // move towards it
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.currTarget, moveSpeed * Time.deltaTime);

            // check if near next node in move path
            if (Vector3.Distance(owner.currTarget, owner.transform.position) < 2.0f)
            {
                owner.moveTargetIndex++;

                // if more nodes exist in the move path...
                if (owner.moveTargetIndex < owner.movePath.Count)
                {
                    owner.currTarget = owner.movePath[owner.moveTargetIndex];
                    owner.currTarget.y = owner.transform.position.y;
                }

                // else this is the final node in move path
                else
                {
                    owner.moveTargetIndex = -1;
                    owner.currTarget = owner.AttackTarget.position;
                    owner.stateMachine.ChangeState(gunnerBoss_phase1.Instance);
                }
            }
        }
    }
}
