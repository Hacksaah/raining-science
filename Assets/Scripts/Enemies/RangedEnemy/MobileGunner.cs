using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class MobileGunner : EnemyActor
{
    public StateMachine<MobileGunner> stateMachine { get; set; }  

    void Awake()
    {
        stateMachine = new StateMachine<MobileGunner>(this);
        Startup();
    }

    // Start is called before the first frame update
    void Start()
    {
        RequestPath();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public void RequestPath()
    {
        if (TestTarget != null)
        {
            PathRequestManager.RequestPath(new PathRequest(transform.position, TestTarget.transform.position, OnPathFound));
        }
    }
    // __        __        __
    // ||        ||        ||
    // ||        ||        ||
    // \/        \/        \/
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            movePath = newPath;
            moveTargetIndex = 0;
            stateMachine.ChangeState(mobileGunner_followPath.Instance);
        }
    }
}
