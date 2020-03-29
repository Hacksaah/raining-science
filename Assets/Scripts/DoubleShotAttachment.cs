using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShotAttachment : Attachment
{

    public override void alterWeapon(Weapon weapon)
    {
        weapon.DoubleShot = true;
    }

    public void SetName(string n)
    {
        name = n;
    }
}
