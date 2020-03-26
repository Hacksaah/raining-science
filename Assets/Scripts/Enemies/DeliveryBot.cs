using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DeliveryBot : EnemyActor
{
    private StateMachine<DeliveryBot> stateMachine;

    public float explosionForce;
    public float blastRadius;
    public LayerMask explosionLayers;

    DeliveryBot()
    {
        stateMachine = new StateMachine<DeliveryBot>(this);
    }

    private void Awake()
    {
        Startup();
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(deliveryBot_explosion.Instance);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }   

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();        
    }
}
