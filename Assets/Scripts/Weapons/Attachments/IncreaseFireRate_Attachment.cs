using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRate_Attachment : Attachment
{
    private float amount = 0.35f;

    public IncreaseFireRate_Attachment()
    {
        name = "Titanium laced coil";
        flavorText = "Increases fire rate by " + (amount * 100) + "%";

        attID = 0;
        projectileOverride = shootOverride = false;
        behaviorOrder = 200;
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.FireRate -= (weapon.baseStats.fireRate * amount);
    }

    public override void ReverseAlter(Weapon weapon)
    {
        weapon.FireRate += (weapon.baseStats.fireRate * amount);
    }
}
