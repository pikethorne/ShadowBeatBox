using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
	internal int beatsImmune;
	internal AudioSource audioSource;
	internal UnitHealth unitHealth;
	internal ScoreManager scoreManager;
	private static readonly Dictionary<ScoreManager.HitRating, Color> ratingColors = new Dictionary<ScoreManager.HitRating, Color>()
	{
		{ ScoreManager.HitRating.Excellent, new Color(0.4f, 0.4f, 0.6f) },
		{ ScoreManager.HitRating.Good, new Color(0.4f, 0.4f, 0.5f) },
		{ ScoreManager.HitRating.Okay, new Color(0.4f, 0.4f, 0.4f) },
		{ ScoreManager.HitRating.Bad, new Color(0.4f, 0.3f, 0.3f) },
		{ ScoreManager.HitRating.Miss, new Color(0.4f, 0.2f, 0.2f) }
	};
	#endregion

	private void Start()
	{
		unitHealth = GetComponent<UnitHealth>();
		audioSource = GetComponent<AudioSource>();
		scoreManager = FindObjectOfType<ScoreManager>();
		if (!GetComponent<Rigidbody>() || !GetComponentInChildren<Collider>())
		{
			Debug.LogWarning("This unit does not have a rigidbody or a collider. This will cause punches to not register.");
		}
	}

	void OnEnable()
	{
		BeatController.BeatEvent += BeatController_BeatEvent;
	}

	void OnDisable()
	{
		BeatController.BeatEvent -= BeatController_BeatEvent;
	}

	private void BeatController_BeatEvent()
	{
		if(beatsImmune > 0)
		{
			unitHealth.ImmunePenalty = true;
			beatsImmune--;
		}
		else
		{
			unitHealth.ImmunePenalty = false;
		}
	}

	//Default unity method
	private void OnTriggerEnter(Collider collision)
	{
		//Check if the incoming collider is a glove, and is not owned by this unit.
		if (collision.gameObject.GetComponent<Glove>() && collision.gameObject.GetComponent<Glove>().Self != unitHealth) 
		{
			// Don't judge a hit if immune.
			if (unitHealth.Immune || unitHealth.ImmunePenalty)
			{
				return;
			}

			Glove enemyGlove = collision.gameObject.GetComponent<Glove>();

			if (enemyGlove.Velocity <= properties.hitThreshold)
			{
				FailedHit(enemyGlove.Velocity, collision);
			}
			else
			{
                // Checks if User has punched 
                if (unitHealth.Exhaustion < 3)
                {
                    float score = scoreManager.GetScore();
                    ScoreManager.HitRating rating = scoreManager.GetHitRating(score);

                    switch (rating)
                    {
                        case ScoreManager.HitRating.Miss:
							beatsImmune++;
							FailedHit(enemyGlove.Velocity, collision);
                            break;
                        default:
							unitHealth.Exhaustion++;
                            SuccessfulHit(enemyGlove.Velocity, collision);
                            break;
                    }
                }
                else
                {
					// If the user has punched too many times the Enemy becomes Immune as a punish
					beatsImmune += 3;
					unitHealth.Exhaustion = 0;
                }
			}
		}
	}

	/// <summary>
	/// Triggered when a hit exceeds the required velocity.
	/// </summary>
	internal void SuccessfulHit(float hitStrength, Collider collision)
	{
		if(playFeedback)
		{
			GenerateTimingFeedback();
			//Instantiate(Resources.Load<GameObject>("Generic/GoodText"), transform.position + textSpawnOffset, transform.rotation);
			if (properties.goodHitSounds.Length != 0)
			{
				PlayRandomAudio(properties.goodHitSounds);
			}
			else
			{
				Debug.LogWarningFormat("{0} does not have good hit sounds.", properties.name);
			}
		}
		unitHealth.DealDamage(1);
	}

	/// <summary>
	/// Triggered when a hit did not exceed the required velocity
	/// </summary>
	internal void FailedHit(float hitStrength, Collider collision)
	{
		if (playFeedback)
		{
			GenerateTimingFeedback();
			//Instantiate(Resources.Load<GameObject>("Generic/BadText"), transform.position + textSpawnOffset, transform.rotation);
			if(properties.failedHitSounds.Length != 0)
			{
				PlayRandomAudio(properties.failedHitSounds);
			}
			else
			{
				Debug.LogWarningFormat("{0} does not have failed hit sounds.", properties.name);
			}
		}
	}

	private void GenerateTimingFeedback()
	{
		GameObject timingText = Instantiate(Resources.Load<GameObject>("Generic/TimingText"), transform.position + textSpawnOffset, transform.rotation);
		TextMeshPro textMesh = timingText.GetComponent<TextMeshPro>();
		ScoreManager.HitRating rating = scoreManager.GetHitRating(scoreManager.GetScore());
		switch(rating)
		{
			case ScoreManager.HitRating.Miss:
				textMesh.SetText("Too " + rating.ToString() + "!");
				break;
			default:
				textMesh.SetText(rating.ToString() + "!");
				break;
		}
		textMesh.color = ratingColors[rating];
	}

	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	internal void PlayRandomAudio(AudioClip[] clips)
	{
		if (clips.Length == 0) return;
		audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
	}

	public UnitProperties GetProperties()
	{
		if (properties) return properties;
		else return null;
	}
}
