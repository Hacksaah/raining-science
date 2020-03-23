using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyStats : ScriptableObject
{
    [SerializeField]
    private VarInt maxHP;
    [SerializeField]
    private VarFloat moveSpeed;
    [SerializeField]
    private VarString enemyName;
    [SerializeField]
    private VarFloat sightDistance;

    public int GetMaxHP() { return maxHP.value; }
    public float GetMoveSpeed() { return moveSpeed.value; }
    public string GetName() { return enemyName.value; }
    public float GetSightDistance() { return sightDistance.value; }
}
