using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment
{
    protected string name;
    public abstract void AlterWeapon(Weapon weapon);
    public abstract void AlterFire(Weapon weapon);

    public string Name { get { return name; } }
}
