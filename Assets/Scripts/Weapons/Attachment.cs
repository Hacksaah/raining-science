using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attachment
{
    protected string name;
    protected string flavorText;
    //Holds the ID for determining which attachment data to spawn
    protected int attID;
    protected int attSpriteID;
    public abstract void AlterWeapon(Weapon weapon);
    public abstract void AlterFire(Weapon weapon);

    public string Name { get { return name; } }

    public string FlavorText { get { return flavorText; } }

    public int AttachmentID { get { return attID; } }

    public int SpriteID { get { return attSpriteID; } }
}
