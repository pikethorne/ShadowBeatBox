using UnityEngine;
using TMPro;

/// <summary>
/// Causes a game object to constantly fly up and be destroyed after expire time.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class NotificationFlyUp : MonoBehaviour
{
	#region Fields
	[Tooltip("How long until the game object should be destroyed")]
	[SerializeField][Range(0.1f,5f)] private float lifetime = 1f;

	[Tooltip("How fast the game object should travel when it is created.")]
	[SerializeField][Range(0f, 5f)] private float initialVelocity = 0.5f;

	[Tooltip("How fast the game object should try to go until it is destroyed")]
	[SerializeField][Range(0f, 5f)] private float targetVelocity = 1f;

	[Tooltip("How much the game object will rotate.")]
	private float maxTargetRotation = 8f;

	/// <summary>
	/// When the object was created.
	/// </summary>
	private float initialTime;
	private float targetRotation;
	private Rigidbody rb;
	private TextMeshPro text;
	#endregion

	#region Properties
	/// <summary>
	/// Returns how fast the velocity should be, based on how far into its lifespan it is.
	/// </summary>
	private float CurrentVelocity
	{
		get
		{
			return (initialVelocity * (1 - PercentElapsed)) + (targetVelocity * PercentElapsed);
		}
	}
	/// <summary>
	/// Returns a number from 0 to 1 where 0 is creation and 1 is expiration. 0%-100%
	/// </summary>
	private float PercentElapsed
	{
		get
		{
			return (Time.time - initialTime) / lifetime;
		}
	}
	/// <summary>
	/// When the object is expected to expire. Might be off based on when update occurs.
	/// </summary>
	private float ExpirationTime
	{
		get
		{
			return initialTime + lifetime;
		}
	}
	/// <summary>
	/// Checks if the object has exceeded its lifetime.
	/// </summary>
	private bool Expired
	{
		get
		{
			if (Time.time > ExpirationTime)
				return true;
			return false;
		}
	}
	#endregion

	#region Methods
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		initialTime = Time.time;
		text = GetComponent<TextMeshPro>();
		targetRotation = UnityEngine.Random.Range(-maxTargetRotation, maxTargetRotation);
	}

	private void FixedUpdate()
	{
		if (Expired)
		{
			Destroy(gameObject);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		SetVelocity();
		SetTextAlpha();
		transform.Rotate(new Vector3(0, 0, transform.rotation.z - Mathf.Lerp(transform.rotation.z, targetRotation, 0.05f)), Space.World);
	}

	private void SetTextAlpha()
	{
		if(!text)
			return;
		text.alpha = 1 - PercentElapsed;
	}

	private void SetVelocity()
	{
		rb.velocity = transform.up * CurrentVelocity;
	}
	#endregion
}
