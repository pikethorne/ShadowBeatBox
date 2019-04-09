using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
	#region Fields and Properties
	private BeatController beatController;

	/// <summary>
	/// Dictionary of HitRating values in order from best to worst. 
	/// </summary>
	public static readonly Dictionary<HitRating, float> ratingThresholds = new Dictionary<HitRating, float>
	{
		//The order of these is critical and should always go from highest to lowest.
		{ HitRating.Excellent, 0.8f },
		{ HitRating.Good, 0.7f },
		{ HitRating.Okay, 0.6f },
		{ HitRating.Bad, 0.5f },
	};

	/// <summary>
	/// Using the HitRating key will return the appropriate color for that rating.
	/// </summary>
	public static readonly Dictionary<HitRating, Color> ratingColors = new Dictionary<HitRating, Color>()
	{
		{ HitRating.Excellent, new Color(0.4f, 0.4f, 0.6f) },
		{ HitRating.Good, new Color(0.4f, 0.4f, 0.5f) },
		{ HitRating.Okay, new Color(0.4f, 0.4f, 0.4f) },
		{ HitRating.Bad, new Color(0.4f, 0.3f, 0.3f) },
		{ HitRating.Miss, new Color(0.4f, 0.2f, 0.2f) }
	};

	public enum HitRating
	{
		Excellent, Good, Okay, Bad, Miss
	}

	public enum HitRelativeTiming
	{
		Early, Late
	}
	#endregion

	void Start()
    {
        beatController = FindObjectOfType<BeatController>();
	}

	/// <summary>
	/// Gets the rating of the timing relative to the beat. 
	/// Returns from (1 to -1) where -1 is just before next beat and 1 is immediately after most recent.
	/// </summary>
	public float GetScore()
    {
        return Mathf.Cos((Time.time - beatController.LastBeatTime) / (beatController.NextBeatTime - beatController.LastBeatTime) * Mathf.PI);
    }

	/// <summary>
	/// Determins the HitRating(Excellent - Miss) for the current timing.
	/// </summary>
	public HitRating GetHitRating()
	{
		//In order of best score to worst: if our score is greater than the threshold, return that rating.
		foreach (KeyValuePair<HitRating, float> entry in ratingThresholds)
		{
			if (Mathf.Abs(GetScore()) >= entry.Value)
			{
				return entry.Key;
			}
		}
		return HitRating.Miss;
	}

	/// <summary>
	/// Determins the HitRating(Excellent - Miss) for the timing of the provided hit score.
	/// </summary>
	public static HitRating GenerateHitRating(float score)
	{
		//In order of best score to worst: if our score is greater than the threshold, return that rating.
		foreach (KeyValuePair<HitRating, float> entry in ratingThresholds)
		{
			if(Mathf.Abs(score) >= entry.Value)
			{
				return entry.Key;
			}
		}
		return HitRating.Miss;
	}

	/// <summary>
	/// Returns if the hit was closer to being after a beat (late) or if a hit was closer to being before a beat (early).
	/// </summary>
	public HitRelativeTiming GetRelativeTiming()
	{
		//If the score is positive, it is after a recent beat. Otherwise it must be before a beat.
		return GetScore() > 0 ? HitRelativeTiming.Late : HitRelativeTiming.Early;
	}

	/// <summary>
	/// Using the provided score returns if the hit was closer to being after a beat (late) or if a hit was closer to being before a beat (early).
	/// </summary>
	public static HitRelativeTiming GenerateRelativeTiming(float score)
	{
		//If the score is positive, it is after a recent beat. Otherwise it must be before a beat.
		return score > 0 ? HitRelativeTiming.Late : HitRelativeTiming.Early;
	}
}
