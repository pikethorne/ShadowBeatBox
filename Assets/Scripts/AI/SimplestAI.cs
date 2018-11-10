using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI state machine for Shadowboxing
/// Uses enum for state machines along with switch statement
/// </summary>
public class SimplestAI : MonoBehaviour
{

    private State _state;
    private EnemyUnit MoveSet;
    public bool updateState = false;
    public int stateDuration = 0;
    MoveLoader parsedList;

    /// <summary>
    /// State declarations
    /// </summary>
    public enum State
    {
        RightPunch,
        LeftPunch,
        WindUp,
        Idle,
        UpdateState
    }

    /// <summary>
    /// Start function, initializes state as well as starts state machine
    /// </summary>
    private void Start()
    {
        MoveSet = GetComponent<EnemyUnit>();
        parsedList = GetComponent<MoveLoader>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Global.switchStateBPM == true)
        {
            UpdateStateSwitch(stateDuration);
            // toggles bpm switch off
            Global.switchStateBPM = false;
        }
    }

    /// <summary>
    /// Enemy Action state machine
    /// </summary>
    private void EnemyActions()
    {
        switch (_state)
        {
            case State.RightPunch:
                RightPunchFunc();
                break;
            case State.LeftPunch:
                LeftPunchFunc();
                break;
            case State.WindUp:
                WindUpFunc();
                break;
            case State.Idle:
                IdleFunc();
                break;
            case State.UpdateState:
                UpdateState();
                break;
        }
    }

    /// <summary>
    /// Checks to see if conditions are correct for a state switch
    /// </summary>
    private void UpdateStateSwitch(int duration)
    {
        if (duration == Global.counterBPM)
        {
            _state = State.UpdateState;
            Global.counterBPM = 0;
            updateState = true;
            UpdateState();
        }
        else if(duration == 0)
        {
            _state = State.UpdateState;
            Global.counterBPM = 0;
            updateState = true;
            UpdateState();
        }
    }

    /// <summary>
    /// Handles the state change
    /// </summary>
    private void UpdateState()
    {
        string newState;
        //int newStateDuration = 0;
        int randomMoveList = Random.Range(0, parsedList.parsedMoves.Count / 4);
        Debug.Log("Update State");

        for (int i = (randomMoveList * 4); i < ((randomMoveList + 1) * 4); i++)
        {
            newState = parsedList.parsedMoves[i];
            stateDuration = int.Parse(parsedList.parsedDuration[i]);

            switch(newState)
            {
                case "RightPunch":
                    _state = State.RightPunch;
                    EnemyActions();
                    updateState = false;
                    break;
                case "LeftPunch":
                    _state = State.LeftPunch;
                    EnemyActions();
                    updateState = false;
                    break;
                case "WindUp":
                    _state = State.WindUp;
                    EnemyActions();
                    updateState = false;
                    break;
                case "Idle":
                    _state = State.Idle;
                    EnemyActions();
                    updateState = false;
                    break;
            }
            while (updateState == false)
            {
                UpdateStateSwitch(stateDuration);
            }
        }
        UpdateState();

    }

    private void RightPunchFunc()
    {
        MoveSet.RightPunch();
    }

    private void LeftPunchFunc()
    {
        MoveSet.LeftPunch();
    }

    private void WindUpFunc()
    {
        MoveSet.WindUp();
    }

    private void IdleFunc()
    {
        MoveSet.Idle();
    }

    private void PrintList(List<string> listOne)
    {
        foreach (var item in listOne)
        {
            print(item);
            print(listOne.Count);
        }
    }
}

public class Global
{
    public static bool switchStateBPM = false;
    public static int counterBPM = 0;
}