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
    public float size;

    // Pathfinding variables    
    public List<Vector3> movePath;
    public int moveTargetIndex;

    // Physic Components
    public Rigidbody rb;

    //TEST variables
    public Transform AttackTarget; // being used for pathfinding and attack target

    protected int dotTicks, dotDamage, dotMultiplier;
    protected bool hasDot = false;

    Renderer objRend; 
    Color originalColor; 

    protected void Startup()
    {
        objRend = GetComponent<Renderer>();
        originalColor = objRend.material.GetColor(176);
        rb = GetComponent<Rigidbody>();
        moveTargetIndex = -1;
        AttackTarget = Level_Grid.Instance.PlayerTransform;
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
        movePath.Clear();
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

    public virtual void TakeDamage(int incomingDamage, Vector3 force, Damage_Type dam_Type)
    {
        currHP -= incomingDamage;
        if(dam_Type == Damage_Type.CORROSIVE)
        {
            dotMultiplier++;
            dotTicks = 7;
            dotDamage = 5;
            StartCoroutine(ApplyDoT());
        }

        if (isAlive && currHP <= 0)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.None;
            if (force == Vector3.zero)
            {
                Vector3 dir = transform.right;
                dir.y = 0.3f;
                Vector3 position = transform.position;
                position.y += dir.y;
                rb.AddForceAtPosition(dir * 4, position, ForceMode.Impulse);
            }
            else
                rb.AddForce(((force.normalized + (Vector3.up * 0.25f)) * 9), ForceMode.Impulse);
            isAlive = false;
            StopAllCoroutines();
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
            if (movePath.Count > 0)
                movePath.Clear();
            foreach (Vector3 v in newPath)
                movePath.Add(v);
            moveTargetIndex = 0;
            currTarget = movePath[moveTargetIndex];
            Debug.Log("Path found :: " + gameObject.name);
        }
        else
        {
            Debug.Log("Path failed :: " + gameObject.name);
        }
    }

    protected IEnumerator ApplyDoT()
    {
        if (!hasDot)
        {
            // Change color           
            objRend.material.SetColor(176, Color.green);
            
            // Tick damage
            hasDot = true;
            float timer = 1.2f;
            while(dotTicks > 0)
            {
                TakeDamage(dotDamage * dotMultiplier, Vector3.zero, Damage_Type.PROJECTILE);
                while(timer > 0)
                {
                    timer -= Time.deltaTime;
                    yield return null;
                }                
                dotTicks--;
                timer = 1.2f;
            }
            
            // Revert color
            objRend.material.SetColor(176, originalColor);
            dotMultiplier = 0;
            hasDot = false;
        }
    }

    protected void FixColor()
    {
        objRend.material.SetColor(176, originalColor);
    }

    protected IEnumerator TurnToRagdoll()
    {        
        yield return new WaitForSeconds(1);
        while (rb.velocity.magnitude > 3)
        {
            yield return null;
        }
        rb.mass = 0;
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
}
