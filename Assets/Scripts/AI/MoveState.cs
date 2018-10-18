using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class MoveState : State<SimpleAI>
{
    private static MoveState instance;

    private MoveState()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static MoveState Instance
    {
        get
        {
            if (instance == null)
            {
                new MoveState();
            }

            return instance;
        }
    }

    public override void EnterState(SimpleAI _owner)
    {
        Debug.Log("Entering Move State");
    }

    public override void ExitState(SimpleAI _owner)
    {
        Debug.Log("Exiting Move State");
    }

    public override void UpdateState(SimpleAI _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateController.ChangeState(PunchState.Instance);
        }
    }
}
