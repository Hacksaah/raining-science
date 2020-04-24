using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenMicroscope : Attachment
{
    float amount = 0.25f;

    public BrokenMicroscope()
    {
        name = "Broken Microscope";
        flavorText = "Increase accuracy by " + (amount * 100) + "%";

        attID = 1;
        projectileOverride = shootOverride = false;
        behaviorOrder = 200;
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.Accuracy -= (weapon.baseStats.shootBloom * amount);
    }

    public override void ReverseAlter(Weapon weapon)
    {
        weapon.Accuracy += (weapon.baseStats.shootBloom * amount);
    }
}
