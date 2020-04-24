using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidGun_IWeapon : IWeapon
{
    public void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior)
    {
        AcidGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectileKey).GetComponent<AcidGun_projectile>();

        projectile.split = 1;
        projectile.gameObject.transform.position = start;
        projectile.transform.localScale = Vector3.one;

        projectile.AssignData(projBehavior, damage, projectileSpeed, shootBloom, damageType);        
        projectile.FireProjectile(target);
    }
}
