using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment
{
    protected string name;
    protected string flavorText;
    public abstract void AlterWeapon(Weapon weapon);
    public abstract void AlterFire(Weapon weapon);
    public abstract void ReverseAlter(Weapon weapon);

    public string Name { get { return name; } }
}
