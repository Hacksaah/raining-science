using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class gunnerBoss_moveToHealthOrb : State<GunnerBoss>
{
    private static gunnerBoss_moveToHealthOrb instance;
    private gunnerBoss_moveToHealthOrb()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static gunnerBoss_moveToHealthOrb Instance
    {
        get
        {
            if (instance == null)
            {
                new gunnerBoss_moveToHealthOrb();
            }
            return instance;
        }
    }

    float moveSpeedRampUp;
    float moveSpeed;

    Rigidbody orbRB;

    public override void EnterState(GunnerBoss owner)
    {
        Debug.Log("Entering moveToHealthOrb");

        moveSpeed = owner.stats.GetMoveSpeed();
        owner.currTarget = owner.HealthOrb_GameObj.transform.position;
        orbRB = owner.HealthOrb_GameObj.GetComponent<Rigidbody>();
        moveSpeedRampUp = 1f;
        owner.RequestPath();
    }

    public override void ExitState(GunnerBoss owner)
    {
        
    }

    public override void FixedUpdateState(GunnerBoss owner)
    {
        
    }

    public override void UpdateState(GunnerBoss owner)
    {
        // checks distance to health orb
        if (Vector3.Distance(owner.transform.position, owner.HealthOrb_GameObj.transform.position) <= 6.0f)
            owner.stateMachine.ChangeState(gunnerBoss_retreiveOrb.Instance);

        // move towards health orb when we have a path
        if (owner.moveTargetIndex >= 0)
        {
            owner.currTarget.y = owner.transform.position.y;
            owner.TurnToFace(owner.currTarget, 8f);
            owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.currTarget, moveSpeed * moveSpeedRampUp * Time.deltaTime);
            moveSpeedRampUp += Time.deltaTime;
            
            // check distance to next node on move path
            if(Vector3.Distance(owner.currTarget, owner.transform.position) < 2.0f)
            {
                int pathLength = owner.movePath.Count;
                owner.moveTargetIndex++;

                if (owner.moveTargetIndex < pathLength)
                {                    
                    owner.currTarget = owner.movePath[owner.moveTargetIndex];
                }
                else
                {
                    owner.currTarget = owner.HealthOrb_GameObj.transform.position + orbRB.velocity.normalized * 8;
                    owner.moveTargetIndex = -1;
                    Debug.Log("Not close enough to orb");
                    owner.RequestPath();
                }
            }
        }
    }
}
