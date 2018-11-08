using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplestAI : MonoBehaviour
{

    private State _state;
    private EnemyUnit MoveSet;
    public bool updateState = false;

    public enum State
    {
        Initialize,
        RightPunch,
        LeftPunch,
        WindUp,
        Idle,
        UpdateState
    }

    private void Start()
    {
        _state = State.Initialize;
        MoveSet = GetComponent<EnemyUnit>();

        //UpdateState();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Global.switchStateBPM == true)
        {
            Debug.Log("Hi");
            UpdateStateSwitch();
            _state = State.UpdateState;
            UpdateState();
            Global.switchStateBPM = false;
        }
    }

    private void EnemyActions()
    {
        switch (_state)
        {
            case State.Initialize:
                InitEnemyAI();
                break;
            case State.RightPunch:
                RightPunchFunc();
                MoveSet.RightPunch();
                break;
            case State.LeftPunch:
                LeftPunchFunc();
                MoveSet.LeftPunch();
                break;
            case State.WindUp:
                WindUpFunc();
                MoveSet.WindUp();
                break;
            case State.Idle:
                IdleFunc();
                MoveSet.Idle();
                break;
            case State.UpdateState:
                UpdateState();
                break;
        }
        //yield return 0;
    }

    private void UpdateStateSwitch()
    {
        updateState = !updateState;
    }

    private void InitEnemyAI()
    {
        Debug.Log("Initialize Enemy AI");
        _state = State.RightPunch;
    }

    private void RightPunchFunc()
    {
        Debug.Log("Right Punch");
    }

    private void LeftPunchFunc()
    {
        Debug.Log("Left Punch");
        _state = State.WindUp;
    }

    private void WindUpFunc()
    {
        Debug.Log("WindUp");
        _state = State.Idle;
    }
    private void IdleFunc()
    {
        Debug.Log("Idle");
        _state = State.UpdateState;
    }

    private void UpdateState()
    {
        Debug.Log("Update State");
        _state = State.Initialize;
        EnemyActions();

    }
}

public class Global
{
    public static bool switchStateBPM = false;
}