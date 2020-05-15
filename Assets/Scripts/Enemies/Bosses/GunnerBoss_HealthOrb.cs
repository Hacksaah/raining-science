using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBoss_HealthOrb : EnemyActor
{

    public int force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        size = 3f;
        Startup();
        ResetActor();    
    }

    private void Start()
    {
        gameObject.SetActive(false);
        BossUI.Instance.ReadyHealthBar(stats.GetMaxHP());
    }

    private void OnEnable()
    {
        rb.AddForce(transform.forward * force);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        FixColor();
    }

    public override void TakeDamage(int incomingDamage, Vector3 force, Damage_Type dam_Type)
    {
        base.TakeDamage(incomingDamage, force, dam_Type);
        BossUI.Instance.UpdateHealthBar(currHP);
        if (currHP <= 0)
            StopAllCoroutines();
    }
}
