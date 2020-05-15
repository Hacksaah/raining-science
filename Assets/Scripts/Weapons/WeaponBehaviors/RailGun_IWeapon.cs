using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun_IWeapon : IWeapon
{
    public void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior)
    {
        //this weapon doesn't have a shootBloom. So we use that variable to determine the size of the projectile
        WeaponProjectile projectile = GameObjectPoolManager.Instance.RequestItemFromPool(projectileKey).GetComponent<WeaponProjectile>();
        projectile.gameObject.transform.position = start;
        projectile.transform.LookAt(target);

        if (shootBloom < 3)
        {
            projectileSpeed = shootBloom * projectileSpeed / 3;
            damage = (int)shootBloom * damage / 3;
            projectile.transform.localScale = Vector3.one * shootBloom / 3;
        }
        
        projectile.AssignData(projBehavior, damage, projectileSpeed, 0, damageType);
        projectile.FireProjectile();
    }
}
