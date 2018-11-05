using UnityEngine;

/// <summary>
/// The TestDummy class is primarily for playtesting purposes.
/// It checks for a trigger entry and will assert if it was a good hit or not.
/// Plays an audio cue and visual remarking if it was a good hit or a bad hit.
/// </summary>
public class TestDummy : EnemyUnit
{
	#region Fields
	[SerializeField] private float velocityRequirement = 1f;
	[SerializeField] private AudioClip[] punches;
	[SerializeField] private AudioClip[] hits;
	[SerializeField] private AudioClip[] hitAffirmation;
	[SerializeField] private AudioClip[] hitFailures;
	[SerializeField] private float talkingDelay = 2.5f;
	[SerializeField] private GameObject textSpawner;
	[SerializeField] private GameObject goodText;
	[SerializeField] private GameObject badText;
	/// <summary>
	/// When should the next talking line play?
	/// </summary>
	private float waitUntil;
	private AudioSource audioSource;
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

	#region Methods
	//Default unity method
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.name + " trigger hit me with " + other.gameObject.GetComponent<Glove>().Velocity);
		PlayRandomAudio(punches);
		if (other.gameObject.GetComponent<Glove>().Velocity > velocityRequirement)
		{
			SuccessfulHit();
		}
		else
		{
			FailedHit();
		}
	}

	/// <summary>
	/// Triggered when a hit did not exceed the required velocity
	/// </summary>
	private void FailedHit()
	{
		Instantiate(badText, textSpawner.transform.position, textSpawner.transform.rotation);
		AttemptFailLine();
	}

	/// <summary>
	/// Attempts to trigger a voice line asserting a bad hit.
	/// </summary>
	private void AttemptFailLine()
	{
		if (Time.time > waitUntil)
		{
			PlayRandomAudio(hitFailures);
			waitUntil = Time.time + talkingDelay;
		}
	}

	/// <summary>
	/// Triggered when a hit exceeds the required velocity.
	/// </summary>
	private void SuccessfulHit()
	{
		Instantiate(goodText, textSpawner.transform.position, textSpawner.transform.rotation);
		PlayRandomAudio(hits);
		AttemptSuccessLine();
	}

	/// <summary>
	/// Attempts to trigger a voice line asserting a good hit.
	/// </summary>
	private void AttemptSuccessLine()
	{
		if (Time.time > waitUntil)
		{
			PlayRandomAudio(hitAffirmation);
			waitUntil = Time.time + talkingDelay;
		}
	}

	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	private void PlayRandomAudio(AudioClip[] clips)
	{
		Audio.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
	}

	/// <summary>
	/// Requests the enemy to do a punch. Will only execute if conditions are met.
	/// </summary>
	public override void AttemptPunch()
	{
        Debug.Log("Attempt Punch");
	}

	/// <summary>
	/// Requests the enemy to windup AND then punch. Will only execute if conditions are met.
	/// </summary>
	public override void AttemptWindupPunch()
	{
        Debug.Log("Attempt Wind Up with Punch");
	}

	/// <summary>
	/// Requests the unit to only windup and not punch after. Will only execute if conditions are met.
	/// </summary>
	public override void AttemptWindupExclusive()
	{
        Debug.Log("Attempt Wind Up Only");

        FacePlayer windUpInstance = new FacePlayer();

        windUpInstance.FacePlayerCamera();
	}

	/// <summary>
	/// Requests the unit to have no action. Will play an idle animation.
	/// </summary>
	public override void AttemptIdle()
	{
        Debug.Log("Attempt Idle");
	}

	/// <summary>
	/// Requests the unit to have no action. Will appear exhausted for a period of time and then play standard idle animation.
	/// </summary>
	/// <param name="exhaustTime">Amount of time the unit should appear exhausted. Should be within tempo.</param>
	public override void AttemptIdle(float exhaustTime)
	{

	}

	#endregion
}
