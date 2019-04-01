using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// Responsible for managing inputs and gameplay of the player's glove.
/// </summary>
public class Glove : MonoBehaviour
{
	#region Fields
	//Hidden from inspector
	private float displacement;
	private Vector3 lastPosition;
	private AudioSource audioSource;
	private SteamVR_Input_Sources thisHand;
	private bool isSteamVRPlayer = false;
	private float nextBlockTime;
	public UnitStatus Self
	{
		get; set;
	}

	//Inspector Visible
	[Header("Audio")]
	[SerializeField] private AudioClip[] goodBlock;
	[SerializeField] private AudioClip[] badBlock;
	[Header("Prefabs")]
	[SerializeField] private GameObject blockParticle;
	[SerializeField] private GameObject cooldownText;
	[Header("Attributes")]
	[Range(0f, 15f)] [SerializeField] private float blockCooldown = 2.5f;
	#endregion

	#region Properties
	/// <summary>
	/// Returns the absolute value of the difference in position since last frame.
	/// </summary>
	public float Velocity
	{
		get
		{
			return Mathf.Abs(displacement);
		}
	}
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
	/// <summary>
	/// Returns true if the current time exceeds the time for the next potential block.
	/// </summary>
	public bool BlockReady{
		get
		{
			if (Time.time > nextBlockTime)
				return true;
			return false;
		}
	}
	/// <summary>
	/// The difference in time between the next available usage of block and the current time.
	/// </summary>
	public float BlockCooldown
	{
		get
		{
			return nextBlockTime - Time.time;
		}
	}
	#endregion

	#region Methods
	private void Awake()
	{
		if(GetComponent<Hand>())
		{
			thisHand = GetComponent<Hand>().handType;
			isSteamVRPlayer = true;
		}
		Self = GetComponentInParent<UnitStatus>();
	}

	private void Update ()
	{
		CalculateDisplacement();
		if(isSteamVRPlayer)
		{
			CheckBlock();
		}
	}

	/// <summary>
	/// Records the positional change since this was last called.
	/// </summary>
	private void CalculateDisplacement()
	{
		displacement = (lastPosition - gameObject.transform.position).magnitude / Time.deltaTime;
		lastPosition = gameObject.transform.position;
	}

	/// <summary>
	/// Checks if the user has the input for blocking and has block ready.
	/// </summary>
	private void CheckBlock()
	{
		if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(thisHand) && BlockReady)
		{
			TriggerBlock();
		}
		else if (SteamVR_Input._default.inActions.GrabGrip.GetStateDown(thisHand) && !BlockReady)
		{
			FailedBlock();
		}
	}

	/// <summary>
	/// Plays a sound, creates particles, and records the next time to block. Currently doesn't actually do anything gameplay wise.
	/// </summary>
	[ContextMenu(itemName: "Trigger Block")]
	public void TriggerBlock()
	{
		PlayRandomAudio(goodBlock);
		Instantiate(blockParticle, transform);
		transform.root.GetComponentInChildren<UnitStatus>().StartCoroutine(transform.root.GetComponentInChildren<UnitStatus>().Block(0.5f));
		nextBlockTime = Time.time + blockCooldown;
		
	}

	/// <summary>
	/// Plays a cooldown sound and displays time until next block in notification.
	/// </summary>
	private void FailedBlock()
	{
		PlayRandomAudio(badBlock);
		GameObject bad = Instantiate(cooldownText, gameObject.transform.position, gameObject.transform.rotation);
		if(bad.GetComponent<TMPro.TextMeshPro>())
		{
			bad.GetComponent<TMPro.TextMeshPro>().text = BlockCooldown.ToString("0.0") + "s";
		}
	}

	//TODO: If we continue to use this method I may add it to a namespace.
	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	private void PlayRandomAudio(AudioClip[] clips)
	{
		Audio.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
	}
	#endregion
}
