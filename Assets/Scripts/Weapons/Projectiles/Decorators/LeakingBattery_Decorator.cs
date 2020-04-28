using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakingBattery_Decorator : Projectile_Decorator
{
    public LeakingBattery_Decorator(IProjectile projectile) : base(projectile) { }

    public override void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type)
    {
        if (Random.Range(0f, 1f) < 0.5f)
        {
            damage_Type = Damage_Type.CORROSIVE;
            //todo change color of bullet to greenish
        }           

        base.Deal_Damage(projectile, enemy, damage, velocity, damage_Type);
    }    
}
