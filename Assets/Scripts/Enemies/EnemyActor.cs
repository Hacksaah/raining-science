using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using StateMachine;

public class EnemyActor : MonoBehaviour
{
    public EnemyStats stats;

    public int currHP;
    public int roomKey;
    public Vector3 currTarget;

    public bool isAlive;

    // Pathfinding variables    
    public Vector3[] movePath;
    public int moveTargetIndex;

    //Custom Components
    protected EnemyWeapon weapon;

    // Physic Components
    public Rigidbody rb;    

    //TEST variables
    public Transform AttackTarget; // being used for pathfinding and attack target

    protected void Startup()
    {
        rb = GetComponent<Rigidbody>();
        weapon = GetComponent<EnemyWeapon>();
        moveTargetIndex = -1;
        roomKey = -1;
    }

    public void SpawnActor(Vector3 position, Vector3 target)
    {
        transform.position = position;
        currTarget = target;
        isAlive = true;
        ResetActor();        
    }

    public void ResetActor()
    {
        currHP = stats.GetMaxHP();
        System.Array.Clear(movePath, 0, movePath.Length);
    }

    //Turns this enemy actor to face a target vector3 without altering its X or Z rotation
    public float TurnToFace(Vector3 target, float turnSpeed)
    {
        if(target != null)
        {
            Vector3 lookDir = transform.InverseTransformPoint(target);

            float angle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
            Vector3 eulerAngularVelocity = Vector3.up * angle * turnSpeed * Time.deltaTime;
            transform.Rotate(eulerAngularVelocity);
            return angle;
        }
        return 0;
    }

    public virtual void TakeDamage(int incomingDamage, Vector3 force)
    {
        currHP -= incomingDamage;
        if (isAlive && currHP <= 0)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(force.normalized * 7, ForceMode.Impulse);
            isAlive = false;
        }
    }

    public void RequestPath()
    {
        if (currTarget != null && roomKey > -1)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, currTarget, roomKey, OnPathFound));
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            //if (movePath.Length > 0)
            //    System.Array.Clear(movePath, 0, movePath.Length);
            movePath = newPath;            
            moveTargetIndex = 0;
            currTarget = movePath[moveTargetIndex];
        }
    }

    protected IEnumerator TurnToRagdoll()
    {        
        yield return new WaitForSeconds(1);
        while (rb.velocity.magnitude > 3)
        {
            yield return null;
        }
        rb.mass = 0;
    }
}
