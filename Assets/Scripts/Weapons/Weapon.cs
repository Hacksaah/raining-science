using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    public WeaponStats baseStats;
    public Transform shootFromTransform;

    [SerializeField]
    protected string projectilePoolKey;

    public GameEvent updateUI;
    public GameEvent reloadEvent;

    public LinkedList<Attachment> attachments = new LinkedList<Attachment>();

    protected float fireRate;
    protected float reloadSpeed;
    protected float projectileSpeed;
    protected float critRate;
    protected float shootBloom;

    protected int damage;
    protected int clipSize;
    protected int ammoInClip;
    protected int maxAmmoCapacity;
    protected int currentAmmoCapacity; // current amount of ammo
    protected Damage_Type damageType;
    
    protected float fireRateTimer;
    protected bool reloading;

    protected int attachmentSlots;
    [SerializeField]
    protected Sprite gunSprite;

    protected new string name;
    protected string flavorText;

    protected bool isPiercing = false;
    protected IProjectile baseProjectileBehavior;
    protected IProjectile projectileBehavior;

    protected IWeapon baseWeaponBehavior;
    protected IWeapon weaponBehavior;

    protected AudioSource shoot;
    protected AudioSource reload;
    protected AudioSource revingUp;
    protected AudioSource constantRev;
    protected AudioSource revingDown;


    //Attachment variables
    //Added
    protected bool doubleShot;
    //protected DoubleShotAttachment DS;

    public float FireRate { get { return fireRate; } set { fireRate = value; } }
    public float ReloadSpeed { get { return reloadSpeed; } set { reloadSpeed = value; } }
    public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }
    public float CritRate { get { return critRate; } set { critRate = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public float Accuracy { get { return shootBloom; } set { shootBloom = value; } }
    public Damage_Type DamageType { get { return damageType; } set { damageType = value; } }

    public int ClipSize { get { return clipSize; } set { clipSize = value; } }
    public int MaxAmmoCapacity { get { return maxAmmoCapacity; } set { maxAmmoCapacity = value; } }
    public int AmmoInClip { get { return ammoInClip; } set { ammoInClip = value; } }
    public int AttachmentSlots { get { return attachmentSlots; } set { attachmentSlots = value; } }
    public Sprite GunSprite { get { return gunSprite; } set { gunSprite = value; } }
    public IProjectile ProjectileBehavior { get { return projectileBehavior; } set { projectileBehavior = value; } }
    public IWeapon WeaponBehavior { get { return weaponBehavior; } set { weaponBehavior = value; } }

    public string Name { get { return name; } }
    public string FlavorText { get { return flavorText; } }

    private void OnDisable()
    {
        reloading = false;
    }

    // ------------------- Custom Functions ----------------------------------------

    protected void AssignBaseStats()
    {
        fireRate = baseStats.fireRate;
        reloadSpeed = baseStats.reloadSpeed;
        projectileSpeed = baseStats.projectileSpeed;
        critRate = baseStats.critRate;
        shootBloom = baseStats.shootBloom;

        damage = baseStats.damage;
        clipSize = baseStats.clipSize;
        maxAmmoCapacity = baseStats.maxAmmoCapacity;
        damageType = baseStats.damageType;
    }

    public void AddAttachment(Attachment newAttachment)
    {
        if(attachments.Count == 0)
        {            
            attachments.AddLast(newAttachment);
            int behavior = newAttachment.behaviorOrder;
            if (behavior < 100)
                UpdateWeaponBehavior();
            else if (behavior < 200)
                UpdateProjectileBehavior();
            else
                newAttachment.AlterWeapon(this);
            return;
        }

        // we're updating weapon stats, sort order don't matter
        if(newAttachment.behaviorOrder >= 200)
        {
            newAttachment.AlterWeapon(this);
            attachments.AddLast(newAttachment);
        }
        // else if we're updating the projectile's behavior
        else if(newAttachment.behaviorOrder >= 100)
        {
            LinkedListNode<Attachment> projNode = attachments.First;

            // determine the sort order for the projectile altering attachment
            // find the first projectile behavior attachment
            while(projNode != null && newAttachment.behaviorOrder > projNode.Value.behaviorOrder)
                projNode = projNode.Next;

            // edge case, found the end of list, no projectile behaviors detected
            if (projNode == null)
            {
                attachments.AddLast(newAttachment);
                UpdateProjectileBehavior();
            }                
            else
            {
                attachments.AddBefore(projNode, newAttachment);
                UpdateProjectileBehavior();
            }
        }
        // otherwise we're altering the shoot mechanic of the weapon
        else
        {
            //Debug.Log("Determining Position");
            LinkedListNode<Attachment> weaponNode = attachments.First;

            while (weaponNode != null && newAttachment.behaviorOrder > weaponNode.Value.behaviorOrder)
                weaponNode = weaponNode.Next;

            // edge case, found end of list
            if(weaponNode == null)
            {
                //Debug.Log("Found end of list, adding to end");
                attachments.AddLast(newAttachment);
                UpdateWeaponBehavior();
            }
            else
            {
                //Debug.Log("Found spot, adding before " + weaponNode.Value.Name);
                attachments.AddBefore(weaponNode, newAttachment);
                UpdateWeaponBehavior();
            }
        }
    }

    public void RemoveAttachment(Attachment attachment)
    {
        attachments.Remove(attachment);
        attachment.ReverseAlter(this); // reverses any stat changes that may occured
        if (attachment.behaviorOrder < 100)
            UpdateWeaponBehavior();
        else if (attachment.behaviorOrder < 200)
            UpdateProjectileBehavior();
    }

    private void UpdateProjectileBehavior()
    {
        if (attachments.Count > 0)
        {
            LinkedListNode<Attachment> projNode = attachments.First;
            // find the first node that alters the projectile behavior
            while (projNode.Value.behaviorOrder < 100)
                projNode = projNode.Next;

            // create the proper behavior
            projectileBehavior = baseProjectileBehavior;
            while (projNode != null && projNode.Value.behaviorOrder < 200)
            {
                // if this attachment overrides the base projectile behavior
                if (projNode.Value.projectileOverride)
                {
                    projectileBehavior = null;
                    while (projNode.Value.projectileOverride)
                    {
                        projNode.Value.AlterWeapon(this);
                        projNode = projNode.Next;

                        // edge cases, 
                        // we've found the end of list OR last projectile override
                        if (projNode == null || projNode.Value.behaviorOrder >= 200)
                            return;
                    }
                }
                projNode.Value.AlterWeapon(this);
                projNode = projNode.Next;
            }
        }
        else
            projectileBehavior = baseProjectileBehavior;        
    }

    public void stopSound()
    {
        shoot.Stop();
        reload.Stop();
        revingUp.Stop();
        constantRev.Stop();
        revingDown.Stop();
    }
    private void UpdateWeaponBehavior()
    {
        if (attachments.Count > 0)
        {
            LinkedListNode<Attachment> weaponNode = attachments.First;

            // create the proper behavior
            weaponBehavior = baseWeaponBehavior;
            while (weaponNode != null && weaponNode.Value.behaviorOrder < 100)
            {
                // for every attachment that overrides the base shoot...
                if (weaponNode.Value.shootOverride)
                {
                    weaponBehavior = null;
                    while (weaponNode.Value.shootOverride)
                    {
                        weaponNode.Value.AlterWeapon(this);
                        weaponNode = weaponNode.Next;

                        // edge case, found end of list or no more weapon modifiers
                        if (weaponNode == null || weaponNode.Value.behaviorOrder >= 100)
                            return;
                    }
                }
                weaponNode.Value.AlterWeapon(this);
                weaponNode = weaponNode.Next;
            }
        }
        else
            weaponBehavior = baseWeaponBehavior;        
    }

    public void ReloadWeapon(PlayerStats stats)
    {
        if (ammoInClip < clipSize)
        {
            fireRateTimer = 0;
            reloading = true;
            reload.Play();
            StartCoroutine(ReloadDelay(stats));
        }
    }

    public bool isReloading()
    {
        return reloading;
    }

    public abstract void WeaponControls(Vector3 target, PlayerStats stats);

    public float UpdateTimer(float timer)
    {
        if (timer > 0)
            return timer - Time.deltaTime;
        return 0;
    }

    protected IEnumerator ReloadDelay(PlayerStats stats)
    {
        stats.ReloadTime = reloadSpeed;
        reloadEvent.Raise();
        yield return new WaitForSeconds(reloadSpeed);
        if (currentAmmoCapacity > 0)
        {
            currentAmmoCapacity = currentAmmoCapacity - clipSize + ammoInClip;
            if (currentAmmoCapacity < 0)
            {
                ammoInClip = clipSize + currentAmmoCapacity;
                currentAmmoCapacity = 0;
                stats.AmmoCapacity = currentAmmoCapacity;
                stats.AmmoInClip = ammoInClip;
                updateUI.Raise();
                yield break;
            }
            stats.AmmoCapacity = currentAmmoCapacity;
        }
        ammoInClip = clipSize;
        stats.AmmoInClip = ammoInClip;
        updateUI.Raise();
        reloading = false;
    }

    // TODO: Create an inner class called Builder in here to instantiate weapons
    // with whatever attachments the consumer of the API wants
}
