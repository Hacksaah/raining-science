using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment
{
    protected string name;
    public abstract void alterWeapon(Weapon weapon);

    public string Name { get { return name; } }
}
