using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class DeliveryBot : EnemyActor
{
    private StateMachine<DeliveryBot> stateMachine;
    public void HaltState() { stateMachine.HaltState(); }
    public Transform rayOrigin;

    public float explosionForce;
    public float blastRadius;
    public LayerMask explosionLayers;
    public ParticleSystem explosiveParticles;

    DeliveryBot()
    {
        stateMachine = new StateMachine<DeliveryBot>(this);
        size = 1.5f;
    }

    private void Awake()
    {
        explosiveParticles = GetComponent<ParticleSystem>();        
    }

    private void Start()
    {
        Startup();
        SpawnActor(transform.position, Vector3.zero);
    }

    private void OnEnable()
    {
        stateMachine.ChangeState(deliveryBot_patrol.Instance);
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }   
    
    void Update()
    {
        stateMachine.Update();        
    }

    public override void TakeDamage(int damage, Vector3 force, Damage_Type dam_Type)
    {        
        if (isAlive)
        {
            if (currHP == stats.GetMaxHP())
            {
                stateMachine.ChangeState(deliveryBot_alerted.Instance);
                StartCoroutine(StopAndTurnToFaceDynamicTarget(AttackTarget, 2.5f));
            }

            if (dam_Type == Damage_Type.CORROSIVE)
            {
                dotMultiplier++;
                dotTicks = 7;
                dotDamage = 5;
                StartCoroutine(ApplyDoT());
            }

            currHP -= damage;
            if (currHP <= 0)
            {
                isAlive = false;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.None;
                rb.AddForce(((force.normalized + (Vector3.up * 0.25f)) * 9), ForceMode.Impulse);
                stateMachine.ChangeState(deliveryBot_explosion.Instance);
            }
        }
        
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

    // a dynamic target is a gameobject that may change position after this coroutine is called
    IEnumerator StopAndTurnToFaceDynamicTarget(Transform target, float turnSpeed)
    {
        State<DeliveryBot> saveState = stateMachine.currentState;
        stateMachine.HaltState();
        float timer = 1.25f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            TurnToFace(target.position, turnSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        stateMachine.ChangeState(saveState);
    }    
}
