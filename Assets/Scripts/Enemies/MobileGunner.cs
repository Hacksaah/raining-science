using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using UnityEditor;

public class MobileGunner : EnemyActor
{
    public bool requestedPath = false;

    //Custom Components
    protected EnemyWeapon weapon;
    private float timer_lineOfSight = 0.0f;

    private StateMachine<MobileGunner> stateMachine;
    public void HaltState() { stateMachine.HaltState(); }

    MobileGunner()
    {
        stateMachine = new StateMachine<MobileGunner>(this);
        size = 1.5f;
    }

    void Awake()
    {
        weapon = GetComponent<EnemyWeapon>();
    }

    private void Start()
    {
        Startup();
        Room_Grid room = Level_Grid.Instance.GetRoom(roomKey);
        SpawnActor(room.AnOpenSpot(), room.AnOpenSpot());

        timer_lineOfSight = Random.Range(0.5f, 1.2f);

        RequestPath();
        requestedPath = true;
        stateMachine.ChangeState(mobileGunner_followPath.Instance);
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
                timer_lineOfSight -= Time.deltaTime;
                if(timer_lineOfSight <= 0f)
                {
                    timer_lineOfSight = Random.Range(0.5f, 1.2f);
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
            weapon.FireWeapon(AttackTarget.position, 0);            
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
            moveTargetIndex = -1;
            RequestPath();
            while (moveTargetIndex == -1)
                yield return null;
            stateMachine.ChangeState(mobileGunner_followPath.Instance);
        }
    }

}
