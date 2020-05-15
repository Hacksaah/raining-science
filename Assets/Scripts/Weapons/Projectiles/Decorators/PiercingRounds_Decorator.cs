using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingRounds_Decorator : Projectile_Decorator
{
    public PiercingRounds_Decorator(IProjectile projectile) : base(projectile)
    {

    }

    public override void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type)
    {

    }
}
