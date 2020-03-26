using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class deliveryBot_patrol : State<DeliveryBot>
{
    private static deliveryBot_patrol instance;
    private deliveryBot_patrol()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static deliveryBot_patrol Instance
    {
        get
        {
            if (instance == null)
            {
                new deliveryBot_patrol();
            }
            return instance;
        }
    }

    public override void EnterState(DeliveryBot owner)
    {
    }

    public override void ExitState(DeliveryBot owner)
    {
    }

    public override void FixedUpdateState(DeliveryBot owner)
    {
    }

    public override void UpdateState(DeliveryBot owner)
    {
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.transform.position + owner.transform.forward, owner.stats.GetMoveSpeed() * Time.deltaTime);
    }
}
