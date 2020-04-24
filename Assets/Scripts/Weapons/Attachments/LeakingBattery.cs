using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakingBattery : Attachment
{
    public LeakingBattery()
    {
        projectileOverride = shootOverride = false;
        behaviorOrder = 199;

        name = "Leaky Battery";
        flavorText = "Your shots have a chance to deal corrosive damage";
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.ProjectileBehavior = new LeakingBattery_Decorator(weapon.ProjectileBehavior);
    }

    public override void ReverseAlter(Weapon weapon)
    {
        
    }
}
