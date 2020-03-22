using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public EnemyStats stats;

    public int currHP;
    public string room_tag;
    public Vector3 currTarget;

    // Pathfinding variables    
    public Vector3[] movePath;
    public int moveTargetIndex = -1;

    //Custom Components
    protected EnemyWeapon weapon;

    // Physic Components
    public Rigidbody rb;    

    //TEST variables
    public GameObject TestAttackTarget; // being used for pathfinding and attack target

    protected void Startup()
    {
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<EnemyWeapon>();
        currTarget = TestAttackTarget.transform.position;
        ResetActor();
    }

    public void ResetActor()
    {
        currHP = stats.GetMaxHP();
        System.Array.Clear(movePath, 0, movePath.Length);
    }

    //Turns this enemy actor to face a target vector3 without altering its X or Z rotation
    public void TurnToFace(Vector3 target)
    {
        if(target != null)
        {
            Vector3 lookDir = transform.InverseTransformPoint(target);

            float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;

            Vector3 eulerAngularVelocity = Vector3.up * angle * 10;
            Quaternion deltaRotation = Quaternion.Euler(eulerAngularVelocity * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }        
    }

    public void TakeDamage(int incomingDamage)
    {
        currHP -= incomingDamage;
        if (currHP <= 0)
        {
            Destroy(gameObject);
        }
    }    

    private void OnDrawGizmos()
    {        
        if (Selection.Contains(gameObject))
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, stats.GetSightDistance());
        }
    }
}
