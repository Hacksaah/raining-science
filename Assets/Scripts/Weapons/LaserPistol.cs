using System.Collections.Generic;
using UnityEngine;

public class LaserPistol : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0.2f;
        reloadSpeed = 1f;
        projectileSpeed = 50f;
        critRate = 0.2f;
        shootBloom = 7;

        damage = 5;
        clipSize = 6;
        ammoInClip = clipSize;
        maxAmmoCapacity = -1;
        currentAmmoCapacity = maxAmmoCapacity;
        attachmentSlots = 3;

        reloading = false;

        name = "Laser Pistol";
        flavorText = "Fires lasors in a quick and efficient manner";
        projectilePoolKey = "laser";
    }

    private void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);
    }

    public override void Shoot(Vector3 target, PlayerStats stats)
    {
        if(Input.GetButton("Fire1"))
        {
            if (fireRateTimer == 0 && !reloading && ammoInClip > 0)
            {
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                Laser_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectilePoolKey).GetComponent<Laser_projectile>();

                projectile.gameObject.transform.position = shootFromTransform.position;
                target.y = shootFromTransform.position.y;
                projectile.FireProjectile(projectileSpeed, damage, shootBloom, target);

                fireRateTimer = fireRate;
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
