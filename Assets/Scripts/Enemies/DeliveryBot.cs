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
        SpawnActor(transform.position, Vector3.zero);
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.ChangeState(deliveryBot_patrol.Instance);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }   

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            stateMachine.ChangeState(deliveryBot_explosion.Instance);
        stateMachine.Update();        
    }
}
