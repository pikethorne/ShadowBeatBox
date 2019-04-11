using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System.Collections;

/// <summary>
/// Responsible for managing inputs and gameplay of the player's glove.
/// </summary>
[RequireComponent(typeof(AudioSource))]
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
	[SerializeField] private AudioClip hitSound;
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

	public bool CombatEnabled { get; set; }
	#endregion

	#region Methods
	private void Awake()
	{
		if(GetComponent<Hand>())
		{
			thisHand = GetComponent<Hand>().handType;
			isSteamVRPlayer = true;
		}
		Self = transform.root.GetComponentInChildren<UnitStatus>();
		CombatEnabled = true;
	}

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Update ()
	{
		CalculateDisplacement();
		if(isSteamVRPlayer)
		{
			CheckBlock();
		}
	}

	public void GoodPunch()
	{
		if(isSteamVRPlayer)
		{
			Instantiate(Resources.Load<GameObject>("Effects/StarEffect"), transform.position, transform.rotation);
            StartCoroutine(PunchVibration(0.25f, 3999));
		}
		audioSource.PlayOneShot(hitSound);
	}

    private IEnumerator PunchVibration(float length, float strength)
    {
        for (float i = 0; i < length; i += Time.deltaTime)
        {
            GetComponent<Hand>().TriggerHapticPulse((ushort)Mathf.Lerp(0, 3999, strength));
            yield return null;
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
		Global.PlayRandomAudio(goodBlock, audioSource);
		Instantiate(blockParticle, transform);
		transform.root.GetComponentInChildren<UnitStatus>().StartCoroutine(transform.root.GetComponentInChildren<UnitStatus>().Block(0.5f));
		nextBlockTime = Time.time + blockCooldown;
		
	}

	/// <summary>
	/// Plays a cooldown sound and displays time until next block in notification.
	/// </summary>
	private void FailedBlock()
	{
		Global.PlayRandomAudio(badBlock, audioSource);
		GameObject bad = Instantiate(cooldownText, gameObject.transform.position, gameObject.transform.rotation);
		if(bad.GetComponent<TMPro.TextMeshPro>())
		{
			bad.GetComponent<TMPro.TextMeshPro>().text = BlockCooldown.ToString("0.0") + "s";
		}
	}
	#endregion
}
