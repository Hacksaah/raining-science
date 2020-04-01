using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

public class mobileGunner_attack : State<MobileGunner>
{
    private static mobileGunner_attack instance;
    private mobileGunner_attack()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

    }
    public static mobileGunner_attack Instance
    {
        get
        {
            if (instance == null)
            {
                new mobileGunner_attack();
            }
            return instance;
        }
    }

    public override void EnterState(MobileGunner owner)
    {
        //Debug.Log("Entering attack state");
        owner.currTarget = owner.AttackTarget.position;
        owner.StartCoroutine(owner.FireWeapon(3));
    }

    public override void ExitState(MobileGunner owner)
    {
        //Debug.Log("Exiting attack state");
    }

    public override void UpdateState(MobileGunner owner)
    {
        owner.currTarget = owner.AttackTarget.position;
    }

    public override void FixedUpdateState(MobileGunner owner)
    {
        owner.TurnToFace(owner.currTarget, 5f);
    }
}
