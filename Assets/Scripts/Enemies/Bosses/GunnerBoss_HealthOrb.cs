using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBoss_HealthOrb : EnemyActor
{
    public VarInt bossCurrHp, bossMaxHp;

    public int force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        size = 3f;
        Startup();
        ResetActor();

        bossMaxHp.value = stats.GetMaxHP();
        bossCurrHp.value = bossMaxHp.value;
    }

    private void Start()
    {

        BossUI.Instance.ReadyHealthBar(bossMaxHp.value);
        gameObject.SetActive(false);
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
        bossCurrHp.value = currHP;
        BossUI.Instance.UpdateHealthBar(currHP);
        if (currHP <= 0)
            StopAllCoroutines();
    }
}
