﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class deliveryBot_patrol : State<DeliveryBot>
{
    float moveSpeed = 5.0f;
    Ray ray;
    RaycastHit hit;

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
        owner.transform.position = Vector3.MoveTowards(owner.transform.position, owner.transform.position + owner.transform.forward, moveSpeed * Time.deltaTime);

        // check if the delivery bot is about to collide with a static or dynamic object in the game world
        Physics.Raycast(owner.rayOrigin.position, owner.transform.forward, out hit, 1.5f);
        if (hit.collider)
        {
            int layer = hit.collider.gameObject.layer;
            if (layer == 9 || layer == 11)
            {
                owner.StartCoroutine(owner.StopAndRotate(90, 3.0f));
            }
        }        
    }
}
