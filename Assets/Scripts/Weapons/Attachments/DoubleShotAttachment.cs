using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotAttachment : Attachment
{
    DoubleShotAttachment()
    {
        name = "Double Shot Attachment";
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.DoubleShot = true;
    }

    public override void AlterFire(Weapon weapon)
    {
        
    }

    public override void ReverseAlter(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }
}
