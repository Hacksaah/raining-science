using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailGun_2 : Weapon
{
    public Light pointLight;

    // Start is called before the first frame update
    void Start()
    {
        fireRate = 1.2f;
        reloadSpeed = 0.85f;
        projectileSpeed = 60f;
        critRate = 0.2f;

        damage = 40;
        clipSize = 1;
        ammoInClip = clipSize;
        maxAmmoCapacity = -1;
        currentAmmoCapacity = maxAmmoCapacity;

        reloading = false;

        name = "Rail Gun 2";
        projectilePoolKey = "railGun";
    }

    public override void Shoot(Vector3 target, PlayerStats stats)
    {
        if (Input.GetButton("Fire1"))
        {
            if(!reloading && fireRateTimer < fireRate)
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

                RailGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectilePoolKey).GetComponent<RailGun_projectile>();

                int size;
                if(fireRateTimer >= fireRate)
                {
                    size = 3;
                }
                else if(fireRateTimer <= fireRate / 3)
                {
                    size = 1;
                }
                else
                {
                    size = 2;
                }

                projectile.gameObject.transform.position = shootFromTransform.position;
                target.y = shootFromTransform.position.y;
                projectile.FireProjectile(projectileSpeed, damage, size, target);

                fireRateTimer = 0;
                pointLight.intensity = 0;
                ReloadWeapon(stats);
            }
        }
    }
}
