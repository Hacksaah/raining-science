using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEditor;

public class MobileGunner : EnemyActor
{
    private float timer_lineOfSight = 0.0f;

    private StateMachine<MobileGunner> stateMachine;
    public void HaltState() { stateMachine.HaltState(); }

    MobileGunner()
    {
        stateMachine = new StateMachine<MobileGunner>(this);
    }

    void Awake()
    {        
        Startup();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
            CheckLineOfSight();
        else
        {
            stateMachine.HaltState();
            StartCoroutine(TurnToRagdoll());
        }
        stateMachine.Update();        
    }

    private void OnEnable()
    {
        RequestPath();
        stateMachine.ChangeState(mobileGunner_followPath.Instance);
    }

    private void OnDisable()
    {
        stateMachine.HaltState();
    }

    void CheckLineOfSight()
    {
        RaycastHit hit;
        Vector3 direction = AttackTarget.position - transform.position;
        if(Physics.Raycast(transform.position, direction, out hit, stats.GetSightDistance()))
        {
            if(hit.transform == AttackTarget)
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

    public IEnumerator FireWeapon(int shotCount)
    {
        float timer = 0.75f;
        while(timer > 0)
        {
            //TurnToFace(AttackTarget.position, 4);
            timer -= Time.deltaTime;
            yield return null;
        }

        for(int i = 0; i < shotCount; i++)
        {
            weapon.FireWeapon(AttackTarget.position);            
            timer = 0.1f;
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
            Vector3 openPosition = PathRequestManager.RequestNewMoveSpot(transform.position, (int)stats.GetSightDistance(), roomKey);
            currTarget = openPosition;
            RequestPath();
            stateMachine.ChangeState(mobileGunner_followPath.Instance);
        }
    }

    
}
