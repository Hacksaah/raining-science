using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidGun : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0.5f;
        reloadSpeed = 1f;
        projectileSpeed = 10f;
        critRate = 0.2f;

        damage = 5;
        clipSize = 4;
        ammoInClip = clipSize;
        maxAmmoCapacity = -1;
        currentAmmoCapacity = maxAmmoCapacity;
        attachmentSlots = 4;

        reloading = false;

        name = "Acid Cannon";
        projectilePoolKey = "acidGun";
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
            if (fireRateTimer == 0 && !reloading && ammoInClip > 0)
            {
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                AcidGun_projectile projectile = GameObjectPoolManager.RequestItemFromPool(projectilePoolKey).GetComponent<AcidGun_projectile>();

                projectile.gameObject.transform.position = shootFromTransform.position;
                target.y = shootFromTransform.position.y;
                projectile.FireProjectile(projectileSpeed, damage, target, 1);

                fireRateTimer = fireRate;
            }
        }
    }
}
