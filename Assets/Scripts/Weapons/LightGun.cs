using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGun : Weapon
{
    private float rampUpTime, rampUpTimer;
    private float currFireRate;
    private int rampUpModifier;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0.1f;
        reloadSpeed = 0.85f;
        projectileSpeed = 60f;
        critRate = 0.2f;

        damage = 3;
        clipSize = 80;
        ammoInClip = clipSize;
        maxAmmoCapacity = -1;
        currentAmmoCapacity = maxAmmoCapacity;

        reloading = false;

        name = "Light Gun";
        projectilePoolKey = "lightGun";

        rampUpTime = 4f / 5;
        rampUpModifier = 5;
        currFireRate = fireRate * rampUpModifier;
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);
    }

    public override void Shoot(Vector3 target, PlayerStats stats)
    {
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

                LightGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectilePoolKey).GetComponent<LightGun_projectile>();                
                target.y = shootFromTransform.position.y;
                projectile.FireProjectile(shootFromTransform.position, damage, target);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            rampUpModifier = 5;
            rampUpTimer = 0;
            currFireRate = fireRate * rampUpModifier;
        }
    }
}
