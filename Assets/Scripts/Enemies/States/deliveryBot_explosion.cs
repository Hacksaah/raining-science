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
    }

    public override void ExitState(DeliveryBot owner)
    {

    }

    public override void FixedUpdateState(DeliveryBot owner)
    {
    }

    public override void UpdateState(DeliveryBot owner)
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Explode(owner);
    }

    private void Explode(DeliveryBot owner)
    {
        hitColliders = Physics.OverlapSphere(owner.transform.position, owner.blastRadius, owner.explosionLayers);

        foreach(Collider col in hitColliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(owner.explosionForce, owner.transform.position, owner.blastRadius, 1, ForceMode.Impulse);
                if(col.gameObject.tag == "Player")
                {
                    // hurt player
                }
                else if(col.gameObject.tag == "Enemy")
                {
                    col.GetComponent<EnemyActor>().TakeDamage(65);
                }
            }
        }
    }
}
