using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun_IProjectile : IProjectile
{
    private int enemyHitCount;

    public void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type)
    {
        enemy.TakeDamage(damage, velocity, damage_Type);
        enemyHitCount--;
        if (enemyHitCount == 0)
            projectile.SetActive(false);
    }

    public void ResetValues()
    {
        enemyHitCount = 3;
    }
}
