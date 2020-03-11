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
        
    }

    public override void ExitState(MobileGunner owner)
    {
        
    }

    public override void UpdateState(MobileGunner owner)
    {
        FollowPath(owner);
    }

    //-------- Custom Functions --------------------------

    private void LookAtTarget(MobileGunner owner, Vector3 currWaypoint)
    {
        Vector3 targetDir = currWaypoint - owner.transform.position;
        Vector3 localTarget = owner.transform.InverseTransformPoint(currWaypoint);

        float angle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        Vector3 eulerAngularVelocity = new Vector3(0, angle, 0) * 10;
        Quaternion deltaRotation = Quaternion.Euler(eulerAngularVelocity * Time.deltaTime);
        owner.rb.MoveRotation(owner.rb.rotation * deltaRotation);
    }   

    private void FollowPath(MobileGunner owner)
    {
        if (owner.moveTargetIndex > -1)
        {
            Vector3 currentWaypoint = owner.movePath[owner.moveTargetIndex];
            if (Vector3.Distance(currentWaypoint, owner.transform.position) < nodeRadius)
            {
                owner.moveTargetIndex++;
                if (owner.moveTargetIndex >= owner.movePath.Length)
                {
                    //Owner needs to find a new path...
                    //Debug.Log("Owner needs a new path...");
                    owner.RequestPath();
                    owner.stateMachine.HaltState();
                }
            }
            LookAtTarget(owner, currentWaypoint);
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, currentWaypoint, owner.stats.GetMoveSpeed() * Time.deltaTime);
        }
        else
        {
            Debug.Log("Has no movePath");
        }

    }
}
