using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour
{
    public int currHP;
    public EnemyStats stats;    

    // Physic Components
    public Rigidbody rb;

    //Test Variables
    public GameObject TestTarget;
    public Vector3[] movePath;
    public int moveTargetIndex = -1;

    protected void Startup()
    {
        rb = GetComponent<Rigidbody>();
        ResetActor();
    }

    public void ResetActor()
    {
        currHP = stats.GetMaxHP();
    }
    
}
