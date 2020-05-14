using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : ScriptableObject
{
    [SerializeField]
    private VarInt maxHP = null;
    [SerializeField]
    private VarFloat moveSpeed = null;
    [SerializeField]
    private VarString enemyName = null;
    [SerializeField]
    private VarFloat sightDistance = null;

    public int GetMaxHP() { return maxHP.value; }
    public float GetMoveSpeed() { return moveSpeed.value; }
    public string GetName() { return enemyName.value; }
    public float GetSightDistance() { return sightDistance.value; }
}
