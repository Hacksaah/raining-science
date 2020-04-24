﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowedRounds : Attachment
{
    public HollowedRounds()
    {
        name = "Hollowing Rounds";
        flavorText = "Sends shrapnel out the back of enemies";

        attID = 2;
        projectileOverride = shootOverride = false;
        behaviorOrder = 198;
    }


    public override void AlterWeapon(Weapon weapon)
    {
        weapon.ProjectileBehavior = new HollowRound_Decorator(weapon.ProjectileBehavior);
    }

    public override void ReverseAlter(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }
}
