using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class gunnerBoss_retreiveOrb : State<GunnerBoss>
{
    private static gunnerBoss_retreiveOrb instance;
    private gunnerBoss_retreiveOrb()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static gunnerBoss_retreiveOrb Instance
    {
        get
        {
            if (instance == null)
            {
                new gunnerBoss_retreiveOrb();
            }
            return instance;
        }
    }

    Vector3 bossPosition;
    Rigidbody orbRB;

    public override void EnterState(GunnerBoss owner)
    {
        Debug.Log("Entering Health orb Retreive");

        orbRB = owner.HealthOrb_GameObj.GetComponent<Rigidbody>();
        orbRB.velocity.Set(0, 0, 0);
        orbRB.isKinematic = true;
        bossPosition = owner.transform.position;
    }

    public override void ExitState(GunnerBoss owner)
    {
        
    }

    public override void FixedUpdateState(GunnerBoss owner)
    {
        
    }

    public override void UpdateState(GunnerBoss owner)
    {
        if(owner.HealthOrb_GameObj.transform.position != bossPosition)
        {
            owner.HealthOrb_GameObj.transform.position = Vector3.MoveTowards(owner.HealthOrb_GameObj.transform.position, bossPosition, 3.5f * Time.deltaTime);
        }
        else
        {
            owner.HealthOrb_GameObj.SetActive(false);
            orbRB.isKinematic = false;

            if (owner.bossCurrHP.value <= 0)
                owner.StartCoroutine(owner.Explode());
            else
            {
                owner.retreivingOrb = false;                
                owner.stateMachine.ChangeState(gunnerBoss_phase1.Instance);
            }
        }
    }
}
