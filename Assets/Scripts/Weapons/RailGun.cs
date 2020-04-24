using UnityEngine;

public class RailGun : Weapon
{
    public Light pointLight;

    // Start is called before the first frame update
    void Start()
    {
        AssignBaseStats();
        ammoInClip = clipSize;
        currentAmmoCapacity = maxAmmoCapacity;
        attachmentSlots = 5;

        reloading = false;

        name = "Rail Gun";
        projectilePoolKey = "railGun";

        baseProjectileBehavior = new RailGun_IProjectile();
        projectileBehavior = baseProjectileBehavior;

        baseWeaponBehavior = new RailGun_IWeapon();
        weaponBehavior = baseWeaponBehavior;
    }

    public override void WeaponControls(Vector3 target, PlayerStats stats)
    {
        if (Input.GetButton("Fire1"))
        {
            if (!reloading && fireRateTimer < fireRate)
            {
                fireRateTimer += Time.deltaTime;
                pointLight.intensity += Time.deltaTime;
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (!reloading)
            {
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                int size;
                if (fireRateTimer >= fireRate)
                {
                    size = 3;
                }
                else if (fireRateTimer <= fireRate / 3)
                {
                    size = 1;
                }
                else
                {
                    size = 2;
                }

                fireRateTimer = 0;
                pointLight.intensity = 0;
                target.y = shootFromTransform.position.y;
                weaponBehavior.FireWeapon(projectilePoolKey, shootFromTransform.position, target, damage, projectileSpeed, size, damageType, projectileBehavior);

                if(ammoInClip == 0)
                    ReloadWeapon(stats);
            }
        }
        if (Input.GetButtonDown("Fire1") && ammoInClip == 0 && !reloading)
            ReloadWeapon(stats);
    }
}
