using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGun_IWeapon : IWeapon
{
    public void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior)
    {
        LightGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectileKey).GetComponent<LightGun_projectile>();
        projectile.AssignData(projBehavior, damage, 0, shootBloom, damageType);
        projectile.FireProjectile(start, target);
    }
}
