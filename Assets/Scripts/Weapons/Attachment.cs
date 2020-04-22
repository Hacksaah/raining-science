using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment
{
    protected string name;
    protected string flavorText;
    protected int attSpriteID;
    public abstract void AlterWeapon(Weapon weapon);
    public abstract void AlterFire(Weapon weapon);

    public string Name { get { return name; } }

    public string FlavorText { get { return flavorText; } }

    public int SpriteID { get { return attSpriteID; } }
}
