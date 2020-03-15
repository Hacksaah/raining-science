using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class MobileGunner : EnemyActor
{
    public StateMachine<MobileGunner> stateMachine { get; set; }

    private float timer_lineOfSight = 0.0f;

    public Transform shootPosition; // the position where bullets will spawn from

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
            PathRequestManager.RequestPath(new PathRequest(transform.position, TestAttackTarget.transform.position, OnPathFound));
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            movePath = newPath;
            moveTargetIndex = 0;
            stateMachine.ChangeState(mobileGunner_followPath.Instance);
        }
    }

    public IEnumerator FireWeapon(int shotCount, Vector3 shootAtPos)
    {
        for(int i = 0; i < shotCount; i++)
        {
            GameObject newBullet = Instantiate(projectilePrefab, shootPosition.position, Quaternion.identity);
            newBullet.transform.LookAt(shootAtPos);

            yield return new WaitForSeconds(Random.Range(0.1f, 0.7f));
        }
        //What this enemy does after it's done firing.

        // chance to find a new shoot position
        if(Random.Range(1, 100) > 50)
        {
            // Find some nearby vecto3 to stand on

        }
    }
}
