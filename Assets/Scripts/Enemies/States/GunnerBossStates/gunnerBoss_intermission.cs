using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class gunnerBoss_intermission : State<GunnerBoss>
{
    private static gunnerBoss_intermission instance;
    private gunnerBoss_intermission()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static gunnerBoss_intermission Instance
    {
        get
        {
            if (instance == null)
            {
                new gunnerBoss_intermission();
            }
            return instance;
        }
    }

    private float timer;

    public override void EnterState(GunnerBoss owner)
    {
        Debug.Log("Entering intermission");

        timer = 5.0f;
        owner.EjectHealthOrb();
    }

    public override void ExitState(GunnerBoss owner)
    {

    }

    public override void FixedUpdateState(GunnerBoss owner)
    {
        if (timer < 0)
            owner.stateMachine.ChangeState(gunnerBoss_moveToHealthOrb.Instance);
            //owner.StartCoroutine(owner.RetreiveHealthOrb());
    }

    public override void UpdateState(GunnerBoss owner)
    {
        timer -= Time.deltaTime;
    }
}
