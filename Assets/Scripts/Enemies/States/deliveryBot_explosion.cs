using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class deliveryBot_explosion : State<DeliveryBot>
{
    private Collider[] hitColliders;

    private static deliveryBot_explosion instance;
    private deliveryBot_explosion()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static deliveryBot_explosion Instance
    {
        get
        {
            if (instance == null)
            {
                new deliveryBot_explosion();
            }
            return instance;
        }
    }

    public override void EnterState(DeliveryBot owner)
    {
        owner.StopAllCoroutines();
        Explode(owner);
        owner.isAlive = false;
        owner.HaltState();
    }

    public override void ExitState(DeliveryBot owner)
    {
    }

    public override void FixedUpdateState(DeliveryBot owner)
    {
    }

    public override void UpdateState(DeliveryBot owner)
    {
    }

    private void Explode(DeliveryBot owner)
    {
        hitColliders = Physics.OverlapSphere(owner.transform.position, owner.blastRadius, owner.explosionLayers);
        foreach(Collider col in hitColliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                if(col.gameObject.tag == "Player")                {
                    
                    col.gameObject.GetComponent<CharacterController>().TakeDamage(40);
                }
                else if(col.gameObject.tag == "Enemy")
                {
                    col.GetComponent<EnemyActor>().TakeDamage(40, Vector3.zero);
                }
                rb.isKinematic = false;
                Vector3 explosiveForce = (rb.position - owner.transform.position).normalized;
                explosiveForce.y = 0.65f;
                explosiveForce *= owner.explosionForce;
                rb.AddForce(explosiveForce, ForceMode.Impulse);
                owner.explosiveParticles.Play();
            }
        }
    }
}
