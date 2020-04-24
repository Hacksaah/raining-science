using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Decorator : IProjectile
{
    protected IProjectile _projectile;

    public Projectile_Decorator(IProjectile projectile)
    {
        _projectile = projectile;
    }

    public virtual void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type)
    {
        _projectile.Deal_Damage(projectile, enemy, damage, velocity, damage_Type);
    }

    public void ResetValues()
    {
        _projectile.ResetValues();
    }
}
