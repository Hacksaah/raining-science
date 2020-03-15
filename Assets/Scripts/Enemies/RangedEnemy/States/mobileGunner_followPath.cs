using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class mobileGunner_followPath : State<MobileGunner>
{
    // TO DO connect this variable to the path_grid variable to avoid dependency issue
    public float nodeRadius = 2.0f;

    private static mobileGunner_followPath instance;
    private mobileGunner_followPath()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static mobileGunner_followPath Instance
    {
        get
        {
            if (instance == null)
            {
                new mobileGunner_followPath();
            }
            return instance;
        }
    }

    public override void EnterState(MobileGunner owner)
    {
        //Debug.Log("Entering path follow state");
    }

    public override void ExitState(MobileGunner owner)
    {
        //Debug.Log("Exiting path follow state");
    }

    public override void UpdateState(MobileGunner owner)
    {
        FollowPath(owner);
    }

    public override void FixedUpdateState(MobileGunner owner)
    {
        // if the target has moved...find a new path
        if (Vector3.Distance(owner.TestAttackTarget.transform.position, owner.movePath[owner.movePath.Length - 1]) > 3 * nodeRadius)
        {
            Debug.Log("Target moved");
            owner.RequestPath();
        }

        // Turn to face the current waypoint
        owner.TurnToFace(owner.currTarget);
    }

    //-------- Custom Functions --------------------------

    private void FollowPath(MobileGunner owner)
    {
        if (owner.moveTargetIndex > -1)
        {
            owner.currTarget = owner.movePath[owner.moveTargetIndex];
            if (Vector3.Distance(owner.currTarget, owner.transform.position) < nodeRadius)
            {
                owner.moveTargetIndex++;
                if (owner.moveTargetIndex >= owner.movePath.Length)
                {
                    //Owner needs to find a new path...
                    owner.RequestPath();
                    owner.stateMachine.HaltState();
                    return;
                }
                
            }
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.currTarget, owner.stats.GetMoveSpeed() * Time.deltaTime);
        }
        else
        {
            Debug.Log("Has no movePath");
        }

    }
}
