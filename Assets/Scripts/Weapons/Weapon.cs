﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    public Transform shootFromTransform;

    [SerializeField]
    protected string projectilePoolKey;

    protected LinkedList<Attachment> attachments = new LinkedList<Attachment>();

    protected float fireRate;
    protected float reloadSpeed;
    protected float projectileSpeed;
    protected float critRate;

    protected int damage;
    protected int clipSize;
    protected int ammoInClip;
    protected int maxAmmoCapacity;
    protected int currentAmmoCapacity; // current amount of ammo
    
    protected float fireRateTimer;
    protected float reloadTimer;

    //Added
    protected int attachmentSlots;
    protected Sprite gunSprite;

    protected new string name;

    //Attachment variables
    //Added
    protected bool doubleShot;
    protected DoubleShotAttachment DS;

    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float ReloadSpeed { get { return reloadSpeed; } set { reloadSpeed = value; } }
    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }
    public float CritRate { get { return critRate; } set { critRate = value; } }

    public int Damage { get { return damage; } set { damage = value; } }
    public int ClipSize { get { return clipSize; } set { clipSize = value; } }
    public int MaxAmmoCapacity { get { return maxAmmoCapacity; } set { maxAmmoCapacity = value; } }
    //Added
    public int AmmoInClip { get { return ammoInClip; } set { ammoInClip = value; } }
    public int AttachmentSlots { get { return attachmentSlots; } set { attachmentSlots = value; } }
    public Sprite GunSprite { get { return gunSprite; } set { gunSprite = value; } }

    public string Name { get { return name; } }


    public bool DoubleShot { get { return doubleShot; } set { doubleShot = value; } }

    public void AddAttachment(Attachment attachment)
    {
        attachment.AlterWeapon(this);
        attachments.AddLast(attachment);
    }

    public void ReloadWeapon()
    {
        if (ammoInClip < clipSize)
        {
            reloadTimer = reloadSpeed;
            fireRateTimer = 0;
            if(currentAmmoCapacity > 0)
            {
                currentAmmoCapacity = currentAmmoCapacity - clipSize + ammoInClip;
                if (currentAmmoCapacity < 0)
                {
                    ammoInClip = clipSize + currentAmmoCapacity;
                    currentAmmoCapacity = 0;
                    return;
                }
            }            
            ammoInClip = clipSize;
        }
    }

    public abstract void Shoot(Vector3 target);

    public float UpdateTimer(float timer)
    {
        if (timer > 0)
            return timer - Time.deltaTime;
        return 0;
    }



    // TODO: Create an inner class called Builder in here to instantiate weapons
    // with whatever attachments the consumer of the API wants
}
