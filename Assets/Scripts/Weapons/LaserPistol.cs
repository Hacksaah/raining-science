using System.Collections.Generic;
using UnityEngine;

public class LaserPistol : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        AssignBaseStats();
        ammoInClip = clipSize;
        currentAmmoCapacity = maxAmmoCapacity;
        attachmentSlots = 3;

        reloading = false;

        name = "Laser Pistol";
        projectilePoolKey = "laser";
        damageType = Damage_Type.PROJECTILE;

        baseProjectileBehavior = new LaserPistol_IProjectile();
        projectileBehavior = baseProjectileBehavior;

        baseWeaponBehavior = new LaserPistol_IWeapon();
        weaponBehavior = baseWeaponBehavior;
    }

    private void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);
    }

    public override void WeaponControls(Vector3 target, PlayerStats stats)
    {
        if (Input.GetButton("Fire1"))
        {
            if (fireRateTimer == 0 && !reloading && ammoInClip > 0)
            {
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                fireRateTimer = fireRate;
                target.y = shootFromTransform.position.y;
                weaponBehavior.FireWeapon(projectilePoolKey, shootFromTransform.position, target, damage, projectileSpeed, shootBloom, damageType, projectileBehavior);
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            fireRateTimer = 0;
        }
        if (ammoInClip == 0 && !reloading && Input.GetButtonDown("Fire1"))
            ReloadWeapon(stats);
    }
}
