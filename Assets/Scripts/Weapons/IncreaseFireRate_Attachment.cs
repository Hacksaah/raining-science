using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRate_Attachment : Attachment
{
    public float amount = 0.35f;

    public override void alterWeapon(Weapon weapon)
    {
        weapon.FireRate += (weapon.FireRate * amount);
    }
}
