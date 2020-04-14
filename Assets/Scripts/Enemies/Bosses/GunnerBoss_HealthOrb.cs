using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBoss_HealthOrb : EnemyActor
{
    public int force;

    public GunnerBoss enemyActor;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * force);
    }
}
