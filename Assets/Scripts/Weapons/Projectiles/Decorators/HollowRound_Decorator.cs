using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// spawns 2 extra projectiles upon exiting an enemy
public class HollowRound_Decorator : Projectile_Decorator
{
    bool lineFlag=false;
    public HollowRound_Decorator(IProjectile projectile) : base(projectile)
    {
        lineFlag = false;
        if (projectile.GetType().Equals(typeof(LightGun_IProjectile)))
            lineFlag = true;
    }

    public override void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type)
    {
        WeaponProjectile projectile1 = GameObjectPoolManager.RequestItemFromPool("hollow").GetComponent<WeaponProjectile>();
        WeaponProjectile projectile2 = GameObjectPoolManager.RequestItemFromPool("hollow").GetComponent<WeaponProjectile>();

        // check if this is a lightGun projectile
        if (lineFlag)
        {
            LineRenderer lineRend = projectile.GetComponent<LineRenderer>();
            Vector3 endPos = lineRend.GetPosition(1);
            Vector3 dir = (endPos - lineRend.GetPosition(0)).normalized;

            projectile1.gameObject.transform.position = endPos + dir * enemy.size;
            projectile2.gameObject.transform.position = endPos + dir * enemy.size;

            Vector3 target = endPos + dir * 10;
            projectile1.transform.LookAt(target);
            projectile2.transform.LookAt(target);
        }
        // otherwise it's a normal projectile
        else
        {
            projectile1.gameObject.transform.position = projectile.transform.position + projectile.transform.forward * enemy.size;
            projectile2.gameObject.transform.position = projectile.transform.position + projectile.transform.forward * enemy.size;

            Vector3 target = projectile.transform.position + projectile.transform.forward * 10;
            projectile1.transform.LookAt(target);
            projectile2.transform.LookAt(target);
        }

        int halfDamage = damage / 2;

        projectile1.AssignData(_projectile, halfDamage, 40, 15, damage_Type);
        projectile2.AssignData(_projectile, halfDamage, 40, 15, damage_Type);
        
        projectile1.transform.Rotate(Vector3.up, Random.Range(-15, 15));       
        projectile2.transform.Rotate(Vector3.up, Random.Range(-15, 15));

        projectile1.FireProjectile();
        projectile2.FireProjectile();

        base.Deal_Damage(projectile, enemy, damage, velocity, damage_Type);
    }
}
