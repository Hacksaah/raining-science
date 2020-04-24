using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPistol_IWeapon : IWeapon
{
    public void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior)
    {
        WeaponProjectile projectile = GameObjectPoolManager.RequestItemFromPool(projectileKey).GetComponent<WeaponProjectile>();

        projectile.transform.position = start;
        projectile.AssignData(projBehavior, damage, projectileSpeed, shootBloom, damageType);
        
        projectile.FireProjectile(target);
    }
}
