using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [SerializeField]
    private VarInt maxHP = null;
    [SerializeField]
    private VarInt currHP = null;

    [SerializeField]
    private VarFloat moveSpeed = null;
    [SerializeField]
    private VarFloat dashSpeed = null;

    [SerializeField]
    private VarInt currentAmmoCapacity = null;
    [SerializeField]
    private VarInt currentAmmoInClip = null;
    [SerializeField]
    private VarFloat currentReloadTime = null;

    public int MaxHP { get { return maxHP.value; } }
    public int CurrHP { get { return currHP.value; } set { currHP.value = value; } }
    public float MoveSpeed { get { return moveSpeed.value; } }
    public float DashSpeed { get { return dashSpeed.value; } }

    public int AmmoInClip { set { currentAmmoInClip.value = value; } }
    public int AmmoCapacity { set { currentAmmoCapacity.value = value; } }
    public float ReloadTime { get { return currentReloadTime.value; } set { currentReloadTime.value = value; } }
}
