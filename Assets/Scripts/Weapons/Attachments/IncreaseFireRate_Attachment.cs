using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseFireRate_Attachment : Attachment
{
    private float amount = 0.35f;

    public override void AlterFire(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.FireRate -= (weapon.FireRate * amount);
    }

}
