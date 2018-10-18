using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;

public class SimpleAI : Timer
{
    public bool switchState = false;

    public StateController<SimpleAI> stateController { get; set; }

    private void Start()
    {
        stateController = new StateController<SimpleAI>(this);
        stateController.ChangeState(MoveState.Instance);
        InvokeRepeating("StateSwitch", 0.0f, Random.Range(1, 5) * noMusicTimer);
    }

    private void Update()
    {
        stateController.Update();
    }

    void StateSwitch()
    {
        switchState = !switchState;
    }
}
