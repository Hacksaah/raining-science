using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotAttachment : Attachment
{
    public override void alterWeapon(Weapon weapon)
    {
        weapon.DoubleShot = true;
    }

    public void SetName()
    {
        name = "Triple Shot";
    }
}
