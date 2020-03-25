using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform shootPosition;         // the position where bullets will spawn from
    public string projectileKey;     

    public void FireWeapon(Vector3 target)
    {
        GameObject newBullet = GameObjectPooler.RequestItemFromPool(projectileKey);
        newBullet.transform.position = shootPosition.position;
        Vector3 shootAt = target;
        shootAt.y = shootPosition.position.y;
        newBullet.transform.LookAt(shootAt);
    }
}
