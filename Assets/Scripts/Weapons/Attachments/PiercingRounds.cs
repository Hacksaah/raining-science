using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingRounds : Attachment
{
    public PiercingRounds()
    {
        name = "Diamond Drill Bits";
        flavorText = "Capable of piercing through any metal";

        attID = 5;
        attSpriteID = attID;
        projectileOverride = true;
        shootOverride = false;
        behaviorOrder = 100;
    }

    public override void AlterWeapon(Weapon weapon)
    {
        if(weapon.ProjectileBehavior != null)
            weapon.ProjectileBehavior = new PiercingRounds_Decorator(weapon.ProjectileBehavior);
        else
        {
            weapon.ProjectileBehavior = new PiercingRound_IProjectile();
        }
    }

    public override void ReverseAlter(Weapon weapon)
    {
        
    }
}
