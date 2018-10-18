using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Causes a game object to constantly fly up and be destroyed after expire time.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class NotificationFlyUp : MonoBehaviour {
	#region Fields
	[SerializeField][Tooltip("How long until the game object should be destroyed")] private float lifetime = 1f;
	[SerializeField][Range(0f, 5f)][Header("How fast the game object should travel when it is created.")] private float initialVelocity = 0.5f;
	[SerializeField][Range(0f, 5f)][Header("How fast the game object should try to go until it is destroyed")] private float targetVelocity = 1f;
	/// <summary>
	/// When the object was created.
	/// </summary>
	private float initialTime;
	private Rigidbody rb;
	private TextMeshPro text;
	#endregion

	#region Properties
	/// <summary>
	/// Returns how fast the velocity should be, based on how far into its lifespan it is.
	/// </summary>
	private float CurrentVelocity{
		get{
			return (initialVelocity * (1 - PercentElapsed)) + (targetVelocity * PercentElapsed);
		}
	}
	/// <summary>
	/// Returns a number from 0 to 1 where 0 is creation and 1 is expiration. 0%-100%
	/// </summary>
	private float PercentElapsed{
		get{
			return (Time.time - initialTime) / lifetime;
		}
	}
	/// <summary>
	/// When the object is expected to expire. Might be off based on when update occurs.
	/// </summary>
	private float ExpirationTime {
		get{
			return initialTime + lifetime;
		}
	}
	/// <summary>
	/// Checks if the object has exceeded its lifetime.
	/// </summary>
	private bool Expired{
		get{
			if (Time.time > ExpirationTime)
				return true;
			return false;
		}
	}
	#endregion

	#region Methods
	private void Awake(){
		rb = GetComponent<Rigidbody>();
		initialTime = Time.time;
		text = GetComponent<TextMeshPro>();
	}

	private void FixedUpdate(){
		if (Expired)
			Destroy(gameObject);
	}

	// Update is called once per frame
	void Update (){
		SetVelocity();
		SetTextAlpha();
	}

	private void SetTextAlpha(){
		if(!text)
			return;
		text.alpha = 1 - PercentElapsed;
	}

	private void SetVelocity(){
		Vector3 current = new Vector3(0, CurrentVelocity);
		rb.velocity = current;
	}
	#endregion
}
