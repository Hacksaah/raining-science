using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotAttachment : Attachment
{
    public TripleShotAttachment()
    {
        name = "Triple Shot";
    }

    public override void AlterFire(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.DoubleShot = true;
    }
}
