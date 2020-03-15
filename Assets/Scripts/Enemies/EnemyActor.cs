using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public int currHP;
    public EnemyStats stats;

    public Vector3 currTarget;

    // Pathfinding variables    
    public Vector3[] movePath;
    public int moveTargetIndex = -1;

    // Physic Components
    public Rigidbody rb;

    //TEST variables
    public GameObject TestAttackTarget; // being used for pathfinding and attack target
    public GameObject projectilePrefab;

    protected void Startup()
    {
        rb = GetComponent<Rigidbody>();
        ResetActor();
    }

    public void ResetActor()
    {
        currHP = stats.GetMaxHP();
    }

    //Turns this enemy actor to face a target vector3 without altering its X or Z rotation
    public void TurnToFace(Vector3 target)
    {
        Vector3 lookDir = transform.InverseTransformPoint(target);

        float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;

        Vector3 eulerAngularVelocity = Vector3.up * angle * 10;
        Quaternion deltaRotation = Quaternion.Euler(eulerAngularVelocity * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
