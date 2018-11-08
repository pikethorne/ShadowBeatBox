using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public StateMachine stateMachine = new StateMachine();
    public bool switchState = false;

    void Start()
    {
        stateMachine.ChangeState(new PunchState2(this));
    }

    void Update()
    {
        StateSwitch();
        Debug.Log("HI!");
        stateMachine.Update();
    }

    void StateSwitch()
    {
        switchState = !switchState;
    }
}
