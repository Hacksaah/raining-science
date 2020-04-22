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
    }

    public override void AlterFire(Weapon weapon)
    {
        throw new System.NotImplementedException();
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
