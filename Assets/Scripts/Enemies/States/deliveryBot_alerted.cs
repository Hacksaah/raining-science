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
        // turns the delivery bot towards the player target
        float angle = owner.TurnToFace(owner.AttackTarget.position, 1.3f);

        // check if delivery bot runs into a static or dynamic object
        Physics.Raycast(owner.rayOrigin.position, owner.transform.forward, out hit, 1.25f);
        if (hit.collider)
        {            
            // explodes upon impact
            int layer = hit.collider.gameObject.layer;
            if(layer == 9 || layer == 11)
            {
                owner.TakeDamage(owner.currHP, Vector3.zero, 0);
            }
        }
    }

    public override void UpdateState(DeliveryBot owner)
    {        
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.transform.position + owner.transform.forward, owner.stats.GetMoveSpeed() * Time.deltaTime);        
    }
}
