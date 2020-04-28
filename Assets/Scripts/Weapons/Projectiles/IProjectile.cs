using UnityEngine;

public interface IProjectile
{
    void Deal_Damage(GameObject projectile, EnemyActor enemy, int damage, Vector3 velocity, Damage_Type damage_Type);
    void ResetValues();
}
