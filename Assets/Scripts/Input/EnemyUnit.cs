using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class EnemyUnit : MonoBehaviour
{
	/// <summary>
	/// Requests the enemy to do a punch. Will only execute if conditions are met.
	/// </summary>
	public abstract void AttemptPunch();

	/// <summary>
	/// Requests the enemy to windup AND then punch. Will only execute if conditions are met.
	/// </summary>
	public abstract void AttemptWindupPunch();


	/// <summary>
	/// Requests the unit to only windup and not punch after. Will only execute if conditions are met.
	/// </summary>
	public abstract void AttemptWindupExclusive();


	/// <summary>
	/// Requests the unit to have no action. Will play an idle animation.
	/// </summary>
	public abstract void AttemptIdle();

	/// <summary>
	/// Requests the unit to have no action. Will appear exhausted for a period of time and then play standard idle animation.
	/// </summary>
	/// <param name="exhaustTime">Amount of time the unit should appear exhausted. Should be within tempo.</param>
	public abstract void AttemptIdle(float exhaustTime);
}
