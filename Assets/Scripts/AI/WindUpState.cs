using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class WindUpState : State<SimpleAI>
{
    private static WindUpState instance;

    private WindUpState()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }

    public static WindUpState Instance
    {
        get
        {
            if (instance == null)
            {
                new WindUpState();
            }

            return instance;
        }
    }

    public override void EnterState(SimpleAI _owner)
    {
        Debug.Log("Entering WindUp State");

        TestDummy windUpInstance = new TestDummy();
        windUpInstance.AttemptWindupExclusive();

        FacePlayer facePlayerInstance = new FacePlayer();
    }

    public override void ExitState(SimpleAI _owner)
    {
        //Debug.Log("Exiting WindUp State");
    }

    public override void UpdateState(SimpleAI _owner)
    {
        if (_owner.switchState)
        {
            _owner.stateController.ChangeState(PunchState.Instance);
        }
    }
}
