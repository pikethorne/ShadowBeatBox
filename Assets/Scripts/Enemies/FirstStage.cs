using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstStage : EnemyUnit {

    /// <summary>
    /// Requests the enemy to do a punch. Will only execute if conditions are met.
    /// </summary>
    /// 
    public override void RightPunch()
    {
        Debug.Log("Right Punch!");
    }

    /// <summary>
    /// Requests the enemy to windup AND then punch. Will only execute if conditions are met.
    /// </summary>
    public override void LeftPunch()
    {
        Debug.Log("Left Punch!");
    }

    /// <summary>
    /// Requests the unit to only windup and not punch after. Will only execute if conditions are met.
    /// </summary>
    public override void WindUp()
    {
        Debug.Log("Windup!");
    }

    /// <summary>
    /// Requests the unit to have no action. Will play an idle animation.
    /// </summary>
    public override void Idle()
    {
        Debug.Log("Idle!");
    }

    /// <summary>
    /// Requests the unit to have no action. Will appear exhausted for a period of time and then play standard idle animation.
    /// </summary>
    /// <param name="exhaustTime">Amount of time the unit should appear exhausted. Should be within tempo.</param>
    public override void AttemptIdle(float exhaustTime)
    {

    }
}
