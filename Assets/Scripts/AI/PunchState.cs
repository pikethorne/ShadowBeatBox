using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class PunchState : State<SimpleAI>
{
    private static PunchState instance;

    private PunchState()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static PunchState Instance
    {
        get
        {
            if (instance == null)
            {
                new PunchState();
            }

            return instance;
        }
    }

    public override void EnterState(SimpleAI _owner)
    {
        Debug.Log("Entering Punch State");

        //TestDummy punchInstance = new TestDummy();
        //punchInstance.AttemptPunch();
    }

    public override void ExitState(SimpleAI _owner)
    {
        //Debug.Log("Exiting Punch State");
    }

    public override void UpdateState(SimpleAI _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateController.ChangeState(IdleState.Instance);
        }
    }
}