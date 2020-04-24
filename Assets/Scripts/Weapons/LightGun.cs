﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGun : Weapon
{
    private float rampUpTime, rampUpTimer;
    private float currFireRate;
    private float currShootBloom;
    private int rampUpModifier;

    // Start is called before the first frame update
    void Start()
    {
        AssignBaseStats();
        ammoInClip = clipSize;
        attachmentSlots = 4;

        reloading = false;

        name = "Light Gun";
        flavorText = "Shoots beams of light. It needs a little time to warm up";
        projectilePoolKey = "lightGun";

        rampUpTime = 4f / 5;
        rampUpModifier = 5;
        currFireRate = fireRate * rampUpModifier;
        currShootBloom = shootBloom * rampUpModifier;

        baseWeaponBehavior = new LightGun_IWeapon();
        weaponBehavior = baseWeaponBehavior;

        baseProjectileBehavior = new LightGun_IProjectile();
        projectileBehavior = baseProjectileBehavior;
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);        
    }

    public override void WeaponControls(Vector3 target, PlayerStats stats)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
        if (Input.GetButton("Fire1"))
        {
            // if we havent ramped up yet
            if (rampUpModifier > 1)
            {
                rampUpTimer -= Time.deltaTime;
                if (rampUpTimer <= 0)
                {
                    // Conduct ramp up
                    rampUpModifier--;
                    currFireRate = fireRate * rampUpModifier;
                    currShootBloom = shootBloom * rampUpModifier;
                    rampUpTimer = rampUpTime;
                }
            }

            if (!reloading && fireRateTimer == 0 && ammoInClip > 0)
            {                
                // shoot the shit
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                fireRateTimer = currFireRate;

                weaponBehavior.FireWeapon(projectilePoolKey, shootFromTransform.position, target, damage, 0, shootBloom, damageType, projectileBehavior);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            rampUpModifier = 5;
            rampUpTimer = 0;
            currFireRate = fireRate * rampUpModifier;
            currShootBloom = shootBloom * rampUpModifier;
        }
        if (ammoInClip == 0 && !reloading && Input.GetButtonDown("Fire1"))
            ReloadWeapon(stats);
    }
}
