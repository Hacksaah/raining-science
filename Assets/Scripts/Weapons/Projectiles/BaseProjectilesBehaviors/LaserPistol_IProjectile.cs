using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPistol_IProjectile : IProjectile
{
    public void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type)
    {
        enemy.TakeDamage(damage, velocity, damage_Type);
        projectile.SetActive(false);
    }

    public void ResetValues() { }
}
