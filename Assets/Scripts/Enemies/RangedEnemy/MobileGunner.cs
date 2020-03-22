using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class MobileGunner : EnemyActor
{
    public StateMachine<MobileGunner> stateMachine { get; set; }

    private float timer_lineOfSight = 0.0f;   

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

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        CheckLineOfSight();
    }

    public void MakeDecisionPoll()
    {

    }

    void CheckLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = TestAttackTarget.transform.position - transform.position;
        if(Physics.Raycast(transform.position, direction, out hit, stats.GetSightDistance()))
        {
            if(hit.transform == TestAttackTarget.transform)
            {
                timer_lineOfSight += Time.deltaTime;
                if(timer_lineOfSight >= 1.7f)
                {
                    timer_lineOfSight = 0.0f;
                    stateMachine.ChangeState(mobileGunner_attack.Instance);
                }
            }
            else
            {
                if (timer_lineOfSight > 0)
                    timer_lineOfSight -= Time.deltaTime;
            }
        }
    }

    public void RequestPath()
    {
        if (currTarget != null)
        {
            stateMachine.HaltState();
            PathRequestManager.RequestPath(new PathRequest(transform.position, currTarget, OnPathFound));
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            System.Array.Clear(movePath, 0, movePath.Length);
            movePath = newPath;
            moveTargetIndex = 0;
            stateMachine.ChangeState(mobileGunner_followPath.Instance);
        }
    }

    public IEnumerator FireWeapon(int shotCount)
    {
        for(int i = 0; i < shotCount; i++)
        {
            weapon.FireWeapon(currTarget);            
            float timer = 0.1f;
            do
            {
                timer -= Time.deltaTime;
                yield return null;
            } while (timer >= 0);
        }

        //What this enemy does after it's done firing...

        // chance to find a new shoot position
        if(Random.Range(1, 100) > 65)
        {
            // Find some nearby vector to stand on
            Vector3 openPosition = PathRequestManager.RequestNewMoveSpot(transform.position, (int)stats.GetSightDistance());
            currTarget = openPosition;
            RequestPath();
        }
    }
}
