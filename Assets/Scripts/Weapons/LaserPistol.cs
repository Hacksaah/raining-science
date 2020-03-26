using System.Collections.Generic;
using UnityEngine;

public class LaserPistol : Weapon
{
    public GameObject projectilePrefab;



    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0.2f;
        reloadSpeed = 1f;
        projectileSpeed = 50f;
        critRate = 0.2f;

        damage = 20;
        clipSize = 6;
        ammoInClip = clipSize;
        maxAmmoCapacity = -1;
        currentAmmoCapacity = maxAmmoCapacity;

        name = "Laser Pistol";
        projectilePoolKey = "laser";
    }

    private void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);
        reloadTimer = UpdateTimer(reloadTimer);
    }

    public override void Shoot(Vector3 target)
    {
        if(Input.GetButton("Fire1"))
        {
            if (fireRateTimer == 0 && reloadTimer == 0 && ammoInClip > 0)
            {
                ammoInClip--;
                
                WeaponProjectile projectile = GameObjectPoolManager.RequestItemFromPool(projectilePoolKey).GetComponent<WeaponProjectile>();

                projectile.gameObject.transform.position = shootFromTransform.position;
                target.y = shootFromTransform.position.y;
                projectile.FireProjectile(projectileSpeed, damage, target);

                fireRateTimer = fireRate;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            fireRateTimer = 0;
        }
    }
}
