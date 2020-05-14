using UnityEngine;

public class RailGun : Weapon
{
    public Light pointLight;
    public AudioClip shotSound;
    public AudioClip reloadSound;
    public AudioClip revvingUp;
    public AudioClip constantReving;

    private AudioSource shot;
    private AudioSource re;
    private AudioSource revUp;
    private AudioSource constant;
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

        AssignBaseStats();
        ammoInClip = clipSize;
        currentAmmoCapacity = maxAmmoCapacity;
        attachmentSlots = 5;

        reloading = false;

        name = "Rail Gun";
        flavorText = "The longer the charge, the greater the output";
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
            revingUp = revUp;
            constantRev = constant;
            if (!reloading && fireRateTimer < fireRate)
            {
                if(!revingUp.isPlaying)
                    revingUp.Play();
                fireRateTimer += Time.deltaTime;
                pointLight.intensity += Time.deltaTime;
            }
            if(!reloading && fireRateTimer >= fireRate)
            {
                if (!revingUp.isPlaying && !constantRev.isPlaying)
                    constantRev.Play();
            }
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            if (!reloading)
            {
                revingUp.Stop();
                constantRev.Stop();
                shoot = shot;
                reload = re;

                shoot.Play();

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
