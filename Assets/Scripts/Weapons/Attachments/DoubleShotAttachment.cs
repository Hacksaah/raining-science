using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotAttachment : Attachment
{
    DoubleShotAttachment()
    {
        name = "Double Shot Attachment";
        attSpriteID = 0;
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.DoubleShot = true;
    }

    public override void AlterFire(Weapon weapon)
    {
        
    }
}
