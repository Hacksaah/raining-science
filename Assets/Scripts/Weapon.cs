using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected LinkedList<Attachment> attachments = new LinkedList<Attachment>();

    protected float fireRate;
    protected float damage;
    protected float projectileSpeed;
    protected float reloadSpeed;
    protected float critRate;

    protected int clipSize;
    // current bullet count or max number of bullets a weapon can have?
    protected int ammoCapacity;

    protected string name;

    public float FireRate { set { fireRate = value; } }
    public float Damage { set { damage = value; } }
    public float ProjectileSpeed { set { projectileSpeed = value; } }
    public float ReloadSpeed { set { reloadSpeed = value; } }
    public float CritRate { set { critRate = value; } }

    public int ClipSize { set { clipSize = value; } }
    public int AmmoCapacity { set { ammoCapacity = value; } }

    public string Name { get { return name; } }

    public void AddAttachment(Attachment attachment)
    {
        attachment.alterWeapon(this);
        attachments.AddLast(attachment);
    }

    public void ReloadWeapon()
    {
        // TODO: Can be implemented in here,
        // just need clarification on something first
    }

    public abstract void Shoot();

    // TODO: Create an inner class called Builder in here to instantiate weapons
    // with whatever attachments the consumer of the API wants
}
