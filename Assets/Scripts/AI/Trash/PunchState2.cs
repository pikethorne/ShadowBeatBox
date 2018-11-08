using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchState2 : MonoBehaviour, IState
{
    EnemyAI owner;
    private EnemyUnit Punch2;
    //private static PunchState2 instance;

    public PunchState2(EnemyAI owner)
    {
        this.owner = owner;
    }

    void Start()
    {
        Punch2 = GetComponent<EnemyUnit>();
    }

    public void Enter()
    {
        Debug.Log("Entering Punch State");
        Punch2.RightPunch();
    }

    public void Execute()
    {
        Debug.Log("Executing Punch State");
    }

    public void Exit()
    {
        Debug.Log("Exiting Punch State");
    }

    public void UpdateState()
    {
        if (owner.switchState)
        {
            owner.stateMachine.ChangeState(new PunchState2(owner));
        }
    }
}