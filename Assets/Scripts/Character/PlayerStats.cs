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
    private VarInt currentAmmoCapacity;
    [SerializeField]
    private VarInt currentAmmoInClip;

    public int MaxHP { get { return maxHP.value; } }
    public int CurrHP { get { return currHP.value; } set { currHP.value = value; } }
    public float MoveSpeed { get { return moveSpeed.value; } }

    public int AmmoInClip { set { currentAmmoInClip.value = value; } }
    public int AmmoCapacity { set { currentAmmoCapacity.value = value; } }
}
