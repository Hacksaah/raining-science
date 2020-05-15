using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform shootPosition;         // the position where bullets will spawn from
    public string projectileKey;

    public float projectileSpeed;
    public int damage;

    public void FireWeapon(Vector3 target, int shootBloom)
    {
        EnemyProjectile projectile = GameObjectPoolManager.Instance.RequestItemFromPool(projectileKey).GetComponent<EnemyProjectile>();
        projectile.gameObject.transform.position = shootPosition.position;
        target.y = shootPosition.position.y;
        projectile.FireProjectile(projectileSpeed, damage, shootBloom, target);
    }
}
