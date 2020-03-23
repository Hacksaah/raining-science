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
        // Turn to face the current waypoint
        owner.TurnToFace(owner.currTarget);
        if (owner.moveTargetIndex > -1)
        {
            if (Vector3.Distance(owner.currTarget, owner.transform.position) < nodeRadius)
            {
                owner.moveTargetIndex++;
                if (owner.moveTargetIndex >= owner.movePath.Length)
                {
                    owner.moveTargetIndex = -1;
                    //ToDO Owner should poll for a decision
                    owner.currTarget = owner.AttackTarget.position;
                    owner.RequestPath();
                    return;
                }
                owner.currTarget = owner.movePath[owner.moveTargetIndex];
            }
        }
    }

    //-------- Custom Functions --------------------------

    private void FollowPath(MobileGunner owner)
    {
        if (owner.moveTargetIndex > -1)
        {
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.currTarget, owner.stats.GetMoveSpeed() * Time.deltaTime);
            Debug.Log("Moving towards" + owner.currTarget.ToString());
        }
        else
        {
            //Debug.Log("Has no movePath");
        }
    }
}
