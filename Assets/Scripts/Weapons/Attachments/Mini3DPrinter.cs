using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini3DPrinter : Attachment
{
    float amount = 0.5f;

    public Mini3DPrinter()
    {
        name = "Pint-sized 3D printer";
        flavorText = "Increases your magazine size by " + (amount * 100) + "%";

        attID = 4;
        attSpriteID = attID;
        shootOverride = projectileOverride = false;
        behaviorOrder = 200;
    }

    public override void AlterWeapon(Weapon weapon)
    {       

        if (weapon.ClipSize < 5)
            weapon.ClipSize += 1;
        else
            weapon.ClipSize += (int)(weapon.ClipSize * amount);

        weapon.AmmoInClip += (int)(weapon.ClipSize * amount);
        AmmoUI.Instance.UpdateText(weapon.AmmoInClip, weapon.MaxAmmoCapacity);            
    }

    public override void ReverseAlter(Weapon weapon)
    {
        bool updateUI = false;
        if (weapon.AmmoInClip == weapon.ClipSize)
            updateUI = true;

        if (weapon.ClipSize < 5)
            weapon.ClipSize -= 1;
        else
            weapon.ClipSize -= (int)(weapon.ClipSize * amount);

        if (updateUI)
        {
            weapon.AmmoInClip = weapon.ClipSize;
            weapon.updateUI.Raise();
        }
    }
}
