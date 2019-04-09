using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// The UnitManager class is responsible for managing a unit's properties and registering events such as hits.
/// </summary>
[RequireComponent(typeof(UnitStatus), typeof(AudioSource))]
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
	internal UnitStatus unitHealth;
	internal ScoreManager scoreManager;
	#endregion


	private void Start()
	{
		unitHealth = GetComponent<UnitStatus>();
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

	private void OnTriggerEnter(Collider collision)
	{
		Glove enemyGlove;
		//Check if the incoming collider is a glove, and is not owned by this unit.
		if ((enemyGlove = collision.gameObject.GetComponent<Glove>()) && collision.gameObject.GetComponent<Glove>().Self != unitHealth && enemyGlove.CombatEnabled) 
		{
			// Don't judge a hit if the unit is immune.
			if (unitHealth.Immune || unitHealth.ImmunePenalty)
			{
				return;
			}
			// Return as a bad hit if the velocity was not sufficient.
			else if (enemyGlove.Velocity <= properties.hitThreshold)
			{
				FailedHit(enemyGlove.Velocity, collision, enemyGlove);
				return;
			}
			else
			{
				// Checks if User has punched 
                if (unitHealth.Exhaustion < 3)
                {
                    ScoreManager.HitRating rating = scoreManager.GetHitRating();

                    switch (rating)
                    {
                        case ScoreManager.HitRating.Miss:
							beatsImmune++;
							FailedHit(enemyGlove.Velocity, collision, enemyGlove);
                            break;
                        default:
							unitHealth.Exhaustion++;
                            SuccessfulHit(enemyGlove.Velocity, collision, enemyGlove);
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
	/// Attempts to play positive feedback. Then will always deal damage to this unit.
	/// </summary>
	internal void SuccessfulHit(float hitStrength, Collider collision, Glove glove)
	{
		if (playFeedback)
		{
			GenerateTimingFeedback();
			Global.PlayRandomAudio(properties.goodHitSounds, audioSource);
		}

		glove.GoodPunch();

		unitHealth.DealDamage(1);
	}

	/// <summary>
	/// Attempts to play negative feedback to play feedback to the player.
	/// </summary>
	internal void FailedHit(float hitStrength, Collider collision, Glove glove)
	{
		if (playFeedback)
		{
			GenerateTimingFeedback();
			Global.PlayRandomAudio(properties.failedHitSounds, audioSource);
		}
	}

	/// <summary>
	/// Instantiates a GameObject that tells the player how good or bad their hit was.
	/// </summary>
	private void GenerateTimingFeedback()
	{
		GameObject timingText = Instantiate(Resources.Load<GameObject>("Generic/TimingText"), transform.position + textSpawnOffset, transform.rotation);
		TextMeshPro textMesh = timingText.GetComponent<TextMeshPro>();
		ScoreManager.HitRating rating = scoreManager.GetHitRating();
		switch(rating)
		{
			case ScoreManager.HitRating.Miss:
				textMesh.SetText("Too " + rating.ToString() + "!");
				break;
			default:
				textMesh.SetText(rating.ToString() + "!");
				break;
		}
		textMesh.color = ScoreManager.ratingColors[rating];
	}

	/// <summary>
	/// Returns the UnitProperties component if it exists otherwise returns null.
	/// </summary>
	public UnitProperties GetProperties()
	{
		if (properties) return properties;
		else return null;
	}
}
