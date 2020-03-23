using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public Transform shootPosition;         // the position where bullets will spawn from
    public GameObject projectilePrefab;     

    public void FireWeapon(Vector3 target)
    {
        GameObject newBullet = Instantiate(projectilePrefab, shootPosition.position, Quaternion.identity);
        Vector3 shootAtPos = target;
        shootAtPos.y = shootPosition.position.y;
        newBullet.transform.LookAt(shootAtPos);
    }
}
