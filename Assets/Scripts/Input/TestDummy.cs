using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TestDummy class is primarily for playtesting purposes.
/// It checks for a trigger entry and will assert if it was a good hit or not.
/// Plays an audio cue and visual remarking if it was a good hit or a bad hit.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TestDummy : MonoBehaviour {
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

	#region Methods
	//Default unity method
	private void Awake(){
		audioSource = GetComponent<AudioSource>();
	}

	//Default unity method
	private void OnTriggerEnter(Collider other){
		Debug.Log(other.name + " trigger hit me with " + other.gameObject.GetComponent<Glove>().Velocity);
		PlayRandomAudio(punches);
		if (other.gameObject.GetComponent<Glove>().Velocity > velocityRequirement)
			SuccessfulHit();
		else
			FailedHit();
	}

	/// <summary>
	/// Triggered when a hit did not exceed the required velocity
	/// </summary>
	private void FailedHit(){
		Instantiate(badText, textSpawner.transform.position, textSpawner.transform.rotation);
		AttemptFailLine();
	}

	/// <summary>
	/// Attempts to trigger a voice line asserting a bad hit.
	/// </summary>
	private void AttemptFailLine(){
		if (Time.time > waitUntil){
			PlayRandomAudio(hitFailures);
			waitUntil = Time.time + talkingDelay;
		}
	}

	/// <summary>
	/// Triggered when a hit exceeds the required velocity.
	/// </summary>
	private void SuccessfulHit(){
		Instantiate(goodText, textSpawner.transform.position, textSpawner.transform.rotation);
		PlayRandomAudio(hits);
		AttemptSuccessLine();
	}

	/// <summary>
	/// Attempts to trigger a voice line asserting a good hit.
	/// </summary>
	private void AttemptSuccessLine(){
		if (Time.time > waitUntil){
			PlayRandomAudio(hitAffirmation);
			waitUntil = Time.time + talkingDelay;
		}
	}

	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	private void PlayRandomAudio(AudioClip[] clips){
		audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
	}
	#endregion
}
