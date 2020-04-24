﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingRounds : Attachment
{
    public PiercingRounds()
    {
        name = "Diamond Drill Bits";
        flavorText = "Capable of piercing through any metal";

        projectileOverride = true;
        shootOverride = false;
        behaviorOrder = 100;
    }

    public override void AlterWeapon(Weapon weapon)
    {
        weapon.ProjectileBehavior = new HollowRound_Decorator(weapon.ProjectileBehavior);
    }

    public override void ReverseAlter(Weapon weapon)
    {
        
    }
}
