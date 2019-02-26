using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RoundManager))]
public class BeatController : MonoBehaviour
{
    float measureLength = 0;
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
		get; set;
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
	public void StartSong(int numerator = 1, int denominator = 8, int newBPM = 0)
    {
        if (newBPM != 0)
            BPM = newBPM;
        measureLength = 240f / BPM;
		beat = StartCoroutine(QueueBeat(numerator, denominator));
    }

    /// <summary>
    /// Stops the current Song being played on the AudioSource and stops the QueueBeat coroutine
    /// </summary>
    public void StopSong()
    {
        StopCoroutine(beat);
    }

    /// <summary>
    /// Coroutine that loops a specific amount of time depending on the Beat specified.
    /// </summary>
    /// <param name="beatNumerator"></param>
    /// <param name="beatDenominator"></param>
    /// <param name="g">Object Reference if Necessary</param>
    /// <returns></returns>
    IEnumerator QueueBeat(float beatNumerator, float beatDenominator, GameObject g = null)
    {
		// timeInMeasure is the time a beat takes
		TimeInMeasure = (measureLength / beatDenominator) * beatNumerator;
		while (true)
        {
            // THIS IS WHERE AN ACTION WOULD HAPPEN WHEN THE BEAT HAPPENS
            // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv

			if(TriggerBeats)
			{
				Global.counterBPM += 1;
			}

			// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

			yield return new WaitForSeconds(TimeInMeasure);
        }
    }
}
