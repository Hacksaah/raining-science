using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomDivider_IWeapon : Weapon_Decorator
{
    public AtomDivider_IWeapon(IWeapon weapon) : base(weapon) { }

    public override void FireWeapon(string projectileKey, Vector3 start, Vector3 target, int damage, float projectileSpeed, float shootBloom, Damage_Type damageType, IProjectile projBehavior)
    {
        Vector3 dir = target - start;
        base.FireWeapon(projectileKey, start, target + DetermineTarget(dir, -25), damage, projectileSpeed, shootBloom, damageType, projBehavior);
        base.FireWeapon(projectileKey, start, target + DetermineTarget(dir, -50), damage, projectileSpeed, shootBloom, damageType, projBehavior);
        base.FireWeapon(projectileKey, start, target, damage, projectileSpeed, shootBloom, damageType, projBehavior);
        base.FireWeapon(projectileKey, start, target + DetermineTarget(dir, 50), damage, projectileSpeed, shootBloom, damageType, projBehavior);
        base.FireWeapon(projectileKey, start, target + DetermineTarget(dir, 25), damage, projectileSpeed, shootBloom, damageType, projBehavior);
    }

    private Vector3 DetermineTarget(Vector3 dir, float angle)
    {
        Vector3 newTarget = dir;
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        float x = newTarget.x;
        float z = newTarget.z;

        newTarget.x = x * cos + z * -sin;
        newTarget.z = x * sin + z * cos;

        return newTarget;
    }
}
