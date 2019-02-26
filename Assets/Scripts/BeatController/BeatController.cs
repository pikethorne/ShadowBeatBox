using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoundManager))]
public class BeatController : MonoBehaviour
{
	float nextBeatTime;
	Coroutine beat;
	public bool TriggerBeats
	{
		get; set;
	}
	public Song Song
	{
		get; set;
	}
	public float BPM
	{
		get;
		private set;
	}
	public float BeatLength
	{
		get
		{
			return 60 / BPM;
		}
	}
	public float TimeInMeasure
	{
		get; set;
	}

	/// <summary>
	/// Method that Creates and Runs a Beatlist for 8 beats a measure, then starts the AudioSource Song.
	/// </summary>
	/// <param name="numerator">Optional</param>
	/// <param name="denominator">Optional</param>
	/// <param name="newBPM">Optional</param>
	public void StartSong(float newBPM = 0)
    {
        if (newBPM != 0)
		{
			BPM = newBPM;
		}
		beat = StartCoroutine(TrackBeats());
    }

    /// <summary>
    /// Stops the current Song being played on the AudioSource and stops the QueueBeat coroutine
    /// </summary>
    public void StopSong()
    {
        StopCoroutine(beat);
    }

	/// <summary>
	/// Coroutine that will keep track of the timing of the next beat. 
	/// Runs every frame, if the frame is after the beat we trigger an event.
	/// </summary>
	IEnumerator TrackBeats()
	{
		nextBeatTime = Time.time + BeatLength;
		while (true)
		{
			bool beatReady = Time.time >= nextBeatTime;
			if (!TriggerBeats && beatReady)
			{
				nextBeatTime += BeatLength;
			}
			else if (TriggerBeats && beatReady)
			{
				Global.counterBPM += 1;
				nextBeatTime += BeatLength;
			}

			//Wait until next frame to check again.
			yield return null;
		}
	}
}
