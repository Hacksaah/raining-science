using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    [SerializeField]
    private VarInt maxHP;
    [SerializeField]
    private VarInt currHP;

    [SerializeField]
    private VarFloat moveSpeed;
    [SerializeField]
    private VarFloat dashSpeed;

    [SerializeField]
    private VarInt currentAmmoCapacity;
    [SerializeField]
    private VarInt currentAmmoInClip;
    [SerializeField]
    private VarFloat currentReloadTime;

    public int MaxHP { get { return maxHP.value; } }
    public int CurrHP { get { return currHP.value; } set { currHP.value = value; } }
    public float MoveSpeed { get { return moveSpeed.value; } }
    public float DashSpeed { get { return dashSpeed.value; } }

    public int AmmoInClip { set { currentAmmoInClip.value = value; } }
    public int AmmoCapacity { set { currentAmmoCapacity.value = value; } }
    public float ReloadTime { get { return currentReloadTime.value; } set { currentReloadTime.value = value; } }
}
