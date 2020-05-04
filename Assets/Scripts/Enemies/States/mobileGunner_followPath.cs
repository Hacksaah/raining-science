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
        // follows along the path to the current target
        if (owner.moveTargetIndex > -1)
        {
            owner.TurnToFace(owner.currTarget, 8f);
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.currTarget, owner.stats.GetMoveSpeed() * Time.deltaTime);
        }
    }

    public override void FixedUpdateState(MobileGunner owner)
    {
              
        if (owner.moveTargetIndex > -1)
        {
            // checks if close enough to next path node
            if (Vector3.Distance(owner.currTarget, owner.transform.position) < nodeRadius)
            {
                owner.moveTargetIndex++;
                // checks if reached last node in move path
                if (owner.moveTargetIndex >= owner.movePath.Length)
                {
                    // requests a new path
                    owner.moveTargetIndex = -1;
                    owner.currTarget = owner.AttackTarget.position;
                    owner.RequestPath();
                    return;
                }
            }
            Vector3 moveTo = owner.movePath[owner.moveTargetIndex];
            owner.currTarget = moveTo;
        }
    }
}
