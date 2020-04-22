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
    }

    public override void Shoot(Vector3 target, PlayerStats stats)
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

                RailGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectilePoolKey).GetComponent<RailGun_projectile>();

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

                projectile.gameObject.transform.position = shootFromTransform.position;
                target.y = shootFromTransform.position.y;
                projectile.FireProjectile(projectileSpeed, damage, size, target);

                fireRateTimer = 0;
                pointLight.intensity = 0;
                if(ammoInClip == 0)
                    ReloadWeapon(stats);
            }
        }
        if (Input.GetButtonDown("Fire1") && ammoInClip == 0 && !reloading)
            ReloadWeapon(stats);
    }
}
