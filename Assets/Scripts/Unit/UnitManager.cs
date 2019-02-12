using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The UnitManager class is a potential base class which is responsible for managing a unit's properties and registering events such as hits.
/// </summary>
[RequireComponent(typeof(UnitHealth))]
public class UnitManager : MonoBehaviour
{
	#region Fields
	[SerializeField] internal AudioClip[] punches;
	[SerializeField] internal AudioClip[] hits;
	[SerializeField] internal AudioClip[] hitAffirmation;
	[SerializeField] internal AudioClip[] hitFailures;
	[SerializeField] internal float talkingDelay = 2.5f;
	[Tooltip("The offset from the origin of the object at which text should spawn.")]
	[SerializeField] internal Vector3 textSpawnOffset;
	/// <summary>
	/// When should the next talking line play?
	/// </summary>
	internal float waitUntil;
	internal AudioSource audioSource;
	internal UnitHealth unitHealth;
	#endregion


	/// <summary>
	/// Returns the reference to the audio source.
	/// </summary>
	public AudioSource Audio
	{
		get
		{
			//Returns the reference to the audio source if it exists
			if (audioSource)
			{
				return audioSource;
			}
			//If there isn't a reference to the audio source but it exists, creates it and returns it.
			else if (!audioSource && GetComponent<AudioSource>())
			{
				return audioSource = GetComponent<AudioSource>();
			}
			//If there isn't an audio source at all it creates one. This is just a fallback and should not be used.
			else
			{
				Debug.LogWarning(gameObject.name + " doesn't have an audio source! Generating one with default settings. To prevent this add an audio source when the game is not running.");
				return audioSource = gameObject.AddComponent<AudioSource>();
			}
		}
		set
		{
			audioSource = value;
		}
	}

	private void Start()
	{
		unitHealth = GetComponent<UnitHealth>();
		if(!GetComponent<Rigidbody>() || !GetComponentInChildren<Collider>())
		{
			Debug.LogWarning("This unit does not have a rigidbody or a collider. This will cause punches to not register.");
		}
	}

	//Default unity method
	private void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.GetComponent<Glove>() && other.gameObject.GetComponent<Glove>().Self != unitHealth)
		{
			Glove enemyGlove = other.gameObject.GetComponent<Glove>();
			Debug.Log(other.name + " trigger hit me with " + enemyGlove.Velocity);
			//PlayRandomAudio(punches);
			if (unitHealth.Immune)
			{
				return;
			}
			if (enemyGlove.Velocity > unitHealth.GetProperties().hitThreshold)
			{
				SuccessfulHit(enemyGlove.Velocity);
			}
			else
			{
				FailedHit();
			}
		}
	}

	/// <summary>
	/// Triggered when a hit did not exceed the required velocity
	/// </summary>
	internal void FailedHit()
	{
		Instantiate(Resources.Load<GameObject>("Generic/BadText"), transform.position + textSpawnOffset, transform.rotation);
		AttemptFailLine();
	}

	/// <summary>
	/// Attempts to trigger a voice line asserting a bad hit.
	/// </summary>
	internal void AttemptFailLine()
	{
		if (Time.time > waitUntil)
		{
			//PlayRandomAudio(hitFailures);
			waitUntil = Time.time + talkingDelay;
		}
	}

	/// <summary>
	/// Triggered when a hit exceeds the required velocity.
	/// </summary>
	internal void SuccessfulHit(float hitStrength)
	{
		Instantiate(Resources.Load<GameObject>("Generic/GoodText"), transform.position + textSpawnOffset, transform.rotation);
		//PlayRandomAudio(hits);
		AttemptSuccessLine();
		unitHealth.DealDamage(hitStrength);
	}

	/// <summary>
	/// Attempts to trigger a voice line asserting a good hit.
	/// </summary>
	internal void AttemptSuccessLine()
	{
		if (Time.time > waitUntil)
		{
			//PlayRandomAudio(hitAffirmation);
			waitUntil = Time.time + talkingDelay;
		}
	}

	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	internal void PlayRandomAudio(AudioClip[] clips)
	{
		Audio.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
	}

}
