using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The UnitManager class is responsible for managing a unit's properties and registering events such as hits.
/// </summary>
[RequireComponent(typeof(UnitHealth), typeof(AudioSource))]
public class UnitManager : MonoBehaviour
{
	#region Fields
	[Tooltip("The offset from the origin of the object at which text should spawn.")]
	[SerializeField] internal Vector3 textSpawnOffset;
	[Tooltip("The offset from the origin of the object at which text should spawn.")]
	[SerializeField] internal bool playFeedback = true;
	[SerializeField] internal UnitProperties properties;
	internal AudioSource audioSource;
	internal UnitHealth unitHealth;
	#endregion

	private void Start()
	{
		unitHealth = GetComponent<UnitHealth>();
		audioSource = GetComponent<AudioSource>();
		if (!GetComponent<Rigidbody>() || !GetComponentInChildren<Collider>())
		{
			Debug.LogWarning("This unit does not have a rigidbody or a collider. This will cause punches to not register.");
		}
	}

	//Default unity method
	private void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.GetComponent<Glove>() && collision.gameObject.GetComponent<Glove>().Self != unitHealth) //Check if the incoming collider is a glove, and is not owned by this unit.
		{
			if (unitHealth.Immune) { return; } // Don't judge a hit if immune.

			Glove enemyGlove = collision.gameObject.GetComponent<Glove>();

			if (enemyGlove.Velocity > properties.hitThreshold)
			{
				SuccessfulHit(enemyGlove.Velocity);
			}
			else
			{
				FailedHit(enemyGlove.Velocity);
			}
		}
	}

	/// <summary>
	/// Triggered when a hit exceeds the required velocity.
	/// </summary>
	internal void SuccessfulHit(float hitStrength)
	{
		if(playFeedback)
		{
			Instantiate(Resources.Load<GameObject>("Generic/GoodText"), transform.position + textSpawnOffset, transform.rotation);
			PlayRandomAudio(properties.goodHitSounds);
		}
		unitHealth.DealDamage(1);
	}

	/// <summary>
	/// Triggered when a hit did not exceed the required velocity
	/// </summary>
	internal void FailedHit(float hitStrength)
	{
		if (playFeedback)
		{
			Instantiate(Resources.Load<GameObject>("Generic/BadText"), transform.position + textSpawnOffset, transform.rotation);
			PlayRandomAudio(properties.failedHitSounds);
		}
	}

	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	internal void PlayRandomAudio(AudioClip[] clips)
	{
		if (clips.Length == 0) return;
		audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
	}

	public UnitProperties GetProperties()
	{
		if (properties) return properties;
		else return null;
	}
}
