using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Decorator : IWeapon
{
    private IWeapon _weapon;

    public Weapon_Decorator(IWeapon weapon)
    {
        _weapon = weapon;
    }

    public virtual void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior)
    {
        _weapon.FireWeapon(projectileKey, start, target, damage, projectileSpeed, shootBloom, damageType, projBehavior);
    }
}
