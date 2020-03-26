using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DeliveryBot : EnemyActor
{
    private StateMachine<DeliveryBot> stateMachine;
    public Transform rayOrigin;

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

    public IEnumerator StopAndRotate(float angle, float turnSpeed)
    {
        State<DeliveryBot> saveState = stateMachine.currentState;
        stateMachine.HaltState();
        yield return new WaitForSeconds(0.65f);
        float total = angle;
        while(total > 0)
        {
            float diff = angle * turnSpeed * Time.deltaTime;
            total -= diff;
            transform.Rotate(Vector3.up, diff);

            yield return null;
        }
        stateMachine.ChangeState(saveState);
    }
}
