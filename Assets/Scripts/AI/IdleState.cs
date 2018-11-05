using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class IdleState : State<SimpleAI>
{
    private static IdleState instance;

    private IdleState()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static IdleState Instance
    {
        get
        {
            if (instance == null)
            {
                new IdleState();
            }

            return instance;
        }
    }

    public override void EnterState(SimpleAI _owner)
    {
        Debug.Log("Entering Idle State");

        //TestDummy idleInstance = new TestDummy();
        //idleInstance.AttemptIdle();
    }

    public override void ExitState(SimpleAI _owner)
    {
        //Debug.Log("Exiting Idle State");
    }

    public override void UpdateState(SimpleAI _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateController.ChangeState(WindUpState.Instance);
        }
    }
}
