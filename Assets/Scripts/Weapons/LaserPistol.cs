﻿using System.Collections.Generic;
using UnityEngine;

public class LaserPistol : Weapon
{
    public GameObject projectilePrefab;



    // Start is called before the first frame update
    void Start()
    {
        fireRate = 0f;
        reloadSpeed = 1f;
        projectileSpeed = 50f;
        critRate = 0.2f;

        damage = 20;
        clipSize = 6;
        ammoInClip = clipSize;
        maxAmmoCapacity = 30;
        currentAmmoCapacity = maxAmmoCapacity;

        name = "Laser Pistol";
    }

    private void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);
        reloadTimer = UpdateTimer(reloadTimer);
    }

    public override void Shoot(Vector3 target)
    {
        if (fireRateTimer <= 0 && reloadTimer <= 0 && ammoInClip > 0)
        {
            ammoInClip--;

            WeaponProjectile projectile = Instantiate(projectilePrefab).GetComponent<WeaponProjectile>();

            projectile.gameObject.SetActive(false);

            projectile.gameObject.transform.position = shootFromTransform.position;
            target.y = shootFromTransform.position.y;
            projectile.transform.LookAt(target);
            projectile.speed = projectileSpeed;
            projectile.damage = damage;

            projectile.gameObject.SetActive(true);

            fireRateTimer = fireRate;
        }
    }
}
