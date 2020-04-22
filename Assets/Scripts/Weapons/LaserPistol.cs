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
