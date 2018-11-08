using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class EnemyUnit : MonoBehaviour
{
	/// <summary>
	/// Requests the enemy to do a right punch.
	/// </summary>
	public abstract void RightPunch();

	/// <summary>
	/// Requests the enemy to do a left punch.
	/// </summary>
	public abstract void LeftPunch();


	/// <summary>
	/// Requests the unit to windup.
	/// </summary>
	public abstract void WindUp();


	/// <summary>
	/// Requests the unit to have no action. Will play an idle animation.
	/// </summary>
	public abstract void Idle();

	/// <summary>
	/// Requests the unit to have no action. Will appear exhausted for a period of time and then play standard idle animation.
	/// </summary>
	/// <param name="exhaustTime">Amount of time the unit should appear exhausted. Should be within tempo.</param>
	public abstract void AttemptIdle(float exhaustTime);
}
