using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGun : Weapon
{
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public AudioClip revvingUp;
    public AudioClip constantReving;
    public AudioClip revvingDown;

    private AudioSource shot;
    private AudioSource re;
    private AudioSource revUp;
    private AudioSource constant;
    private AudioSource revDown;
    private float rampUpTime, rampUpTimer;
    private float currFireRate;
    private float currShootBloom;
    private int rampUpModifier;

    // Start is called before the first frame update
    void Start()
    {
        shot = gameObject.AddComponent<AudioSource>();
        shot.loop = false;
        shot.playOnAwake = false;
        shot.clip = shotSound;

        re = gameObject.AddComponent<AudioSource>();
        re.loop = false;
        re.playOnAwake = false;
        re.clip = reloadSound;

        revUp = gameObject.AddComponent<AudioSource>();
        revUp.loop = false;
        revUp.playOnAwake = false;
        revUp.clip = revvingUp;

        constant = gameObject.AddComponent<AudioSource>();
        constant.loop = true;
        constant.playOnAwake = false;
        constant.clip = constantReving;

        revDown = gameObject.AddComponent<AudioSource>();
        revDown.loop = false;
        revDown.playOnAwake = false;
        revDown.clip = revvingDown;

        AssignBaseStats();
        ammoInClip = clipSize;
        attachmentSlots = 4;

        reloading = false;

        name = "Light Gun";
        flavorText = "Shoots beams of light. It needs a little time to warm up";
        projectilePoolKey = "lightGun";

        rampUpTime = 4f / 5;
        rampUpModifier = 5;
        currFireRate = fireRate * rampUpModifier;
        currShootBloom = shootBloom * rampUpModifier;

        baseWeaponBehavior = new LightGun_IWeapon();
        weaponBehavior = baseWeaponBehavior;

        baseProjectileBehavior = new LightGun_IProjectile();
        projectileBehavior = baseProjectileBehavior;        
    }

    // Update is called once per frame
    void Update()
    {
        fireRateTimer = UpdateTimer(fireRateTimer);        
    }

    public override void WeaponControls(Vector3 target, PlayerStats stats)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
        if (Input.GetButton("Fire1"))
        {
            shoot = shot;
            reload = re;
            revingUp = revUp;
            constantRev = constant;
            revingDown = revDown;

            // if we havent ramped up yet
            if (rampUpModifier > 1)
            {
                rampUpTimer -= Time.deltaTime;
                if (rampUpTimer <= 0)
                {
                    // Conduct ramp up
                    if(!revingUp.isPlaying)
                        revingUp.Play();
                    rampUpModifier--;
                    currFireRate = fireRate * rampUpModifier;
                    currShootBloom = shootBloom * rampUpModifier;
                    rampUpTimer = rampUpTime;
                }
            }

            if (!reloading && fireRateTimer == 0 && ammoInClip > 0)
            {
                // shoot the shit
                shoot.Play();
                if (!revingUp.isPlaying && !constantRev.isPlaying)
                    constantRev.Play();
                ammoInClip--;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();

                fireRateTimer = currFireRate;

                weaponBehavior.FireWeapon(projectilePoolKey, shootFromTransform.position, target, damage, 0, shootBloom, damageType, projectileBehavior);
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            revingUp.Stop();
            constantRev.Stop();
            revingDown.Play();
            rampUpModifier = 5;
            rampUpTimer = 0;
            currFireRate = fireRate * rampUpModifier;
            currShootBloom = shootBloom * rampUpModifier;
        }
        if (ammoInClip == 0 && !reloading && Input.GetButtonDown("Fire1"))
            ReloadWeapon(stats);
    }
}
