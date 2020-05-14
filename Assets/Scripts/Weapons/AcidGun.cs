using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidGun : Weapon
{
    public AudioClip shotSound;
    public AudioClip reloadSound;

    private AudioSource shot;
    private AudioSource re;
    // Start is called before the first frame update
    void Start()
    {
        shot = gameObject.AddComponent<AudioSource>();
        shot.loop = false;
        shot.clip = shotSound;

        re = gameObject.AddComponent<AudioSource>();
        re.loop = false;
        re.clip = reloadSound;

        AssignBaseStats();
        ammoInClip = clipSize;
        currentAmmoCapacity = maxAmmoCapacity;
        attachmentSlots = 4;

        reloading = false;

        name = "Acid Cannon";
        flavorText = "It makes a f@(%!# mess";
        projectilePoolKey = "acidGun";

        baseWeaponBehavior = new AcidGun_IWeapon();
        weaponBehavior = baseWeaponBehavior;

        baseProjectileBehavior = new AcidGun_IProjectile();
        projectileBehavior = baseProjectileBehavior;
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);
    }

    public override void WeaponControls(Vector3 target, PlayerStats stats)
    {
        if (Input.GetButton("Fire1"))
        {
            if (fireRateTimer == 0 && !reloading && ammoInClip > 0)
            {
                shoot = shot;
                reload = re;

                shoot.Play();
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                fireRateTimer = fireRate;
                target.y = shootFromTransform.position.y;
                weaponBehavior.FireWeapon(projectilePoolKey, shootFromTransform.position, target, damage, projectileSpeed, shootBloom, damageType, projectileBehavior);               
            }            
        }
        if (ammoInClip == 0 && !reloading && Input.GetButtonDown("Fire1"))
            ReloadWeapon(stats);
    }
}
