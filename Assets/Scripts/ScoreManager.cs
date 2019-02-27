using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private BeatController beatController;
	[System.Obsolete]
	[SerializeField] private bool logState;
	[System.Obsolete] private int lastBPM;

	/// <summary>
	/// Dictionary of values in order from best to worst. The order of these is critical to proper execution.
	/// </summary>
	public static readonly Dictionary<HitRating, float> ratingThresholds = new Dictionary<HitRating, float>
	{
		{ HitRating.Excellent, 0.8f },
		{ HitRating.Good, 0.7f },
		{ HitRating.Okay, 0.6f },
		{ HitRating.Bad, 0.5f },
	};

    void Start()
    {
        beatController = FindObjectOfType<BeatController>();
	}

	private void Update()
	{
		//Print the state every frame for testing/debug
		if(logState)
		{
			PrintState();
		}
	}

	/// <summary>
	/// This method is temporary and should be removed later.
	/// </summary>
	[System.Obsolete]
	private void PrintState()
	{
		float score = GetScore();
		if (lastBPM != Global.counterBPM)
		{
			lastBPM = Global.counterBPM;
			Debug.LogWarning("-- Beat Happened --");
		}
		Debug.LogFormat("Score: {0:F2} Rating: {1} {2}", score, GetHitTiming(score).ToString(), GetHitRating(score).ToString());
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
	/// Determines the rating of the score by iterating through a dictionary of threshold values.
	/// </summary>
	public HitRating GetHitRating(float score)
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
	/// Determines if the beat was too early or too late.
	/// </summary>
	public HitRelativeTiming GetHitTiming(float score)
	{
		//If the score is positive, it is after a recent beat. Otherwise it must be before a beat.
		return score > 0 ? HitRelativeTiming.Late : HitRelativeTiming.Early;
	}

	public enum HitRating
	{
		Excellent, Good, Okay, Bad, Miss
	}

	public enum HitRelativeTiming
	{
		Early, Late
	}
}
