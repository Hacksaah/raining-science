using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior);
}
