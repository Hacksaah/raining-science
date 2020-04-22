using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    public float fireRate;
    public float reloadSpeed;
    public float projectileSpeed;
    public float critRate;
    public float shootBloom;

    public int damage;
    public int clipSize;
    public int maxAmmoCapacity;
}
