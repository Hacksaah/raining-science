using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class deliveryBot_alerted : State<DeliveryBot>
{
    Ray ray;
    RaycastHit hit;

    private static deliveryBot_alerted instance;
    private deliveryBot_alerted()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static deliveryBot_alerted Instance
    {
        get
        {
            if (instance == null)
            {
                new deliveryBot_alerted();
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
        Physics.Raycast(owner.rayOrigin.position, owner.transform.forward, out hit, 1.25f);
        if (hit.collider)
        {
            owner.TakeDamage(999);
        }
        float angle = owner.TurnToFace(owner.AttackTarget.position, 1.3f);
        if (angle < 90 && angle > -90 && Vector3.Distance(owner.transform.position, owner.AttackTarget.position) < 1.25f)
        {
            owner.TakeDamage(999);
        }
    }

    public override void UpdateState(DeliveryBot owner)
    {        
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.transform.position + owner.transform.forward, owner.stats.GetMoveSpeed() * Time.deltaTime);        
    }
}
