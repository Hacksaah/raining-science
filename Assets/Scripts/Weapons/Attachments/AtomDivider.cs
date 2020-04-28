using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// alter's a weapon's firing mechanic to act like a shotgun
public class AtomDivider : Attachment
{
    float amount = 0.4f;

    public AtomDivider()
    {
        attID = 3;
        attSpriteID = attID;
        shootOverride = false;
        projectileOverride = false;
        behaviorOrder = 0;

        name = "Atom Divider";
        flavorText = "Turns your weapon into a shotgun";
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.FireRate += weapon.baseStats.fireRate * amount;
        weapon.WeaponBehavior = new AtomDivider_IWeapon(weapon.WeaponBehavior);
    }

    public override void ReverseAlter(Weapon weapon)
    {
        weapon.FireRate -= weapon.baseStats.fireRate * amount;
    }
}
