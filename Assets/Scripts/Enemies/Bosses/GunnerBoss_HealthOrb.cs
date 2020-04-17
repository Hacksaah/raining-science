using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBoss_HealthOrb : EnemyActor
{
    public VarInt bossMaxHP;
    public VarInt bossCurrHP;

    public int force;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Startup();
        ResetActor();
        bossMaxHP.value = stats.GetMaxHP();
        bossCurrHP.value = currHP;
        
        BossUI.Instance.gameObject.SetActive(true);
        BossUI.Instance.ReadyUI();

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
        bossCurrHP.value = currHP;
        BossUI.Instance.UpdateHealthBar();
    }
}
