using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour {

    AudioSource song;
    public float BPM;
    bool stop = true;

    float measureLength = 0;
    // Use this for initialization
    void Start () {
        song = GetComponent<AudioSource>();
        StartSong();
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
        StartCoroutine(QueueBeat(numerator, denominator));
        song.Play();
    }

    /// <summary>
    /// Stops the current Song being played on the AudioSource and stops the QueueBeat coroutine
    /// </summary>
    public void StopSong()
    {
        song.Stop();
        StopCoroutine("QueueBeat");
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
        float timeInMeasure = (measureLength / beatDenominator) * beatNumerator;
        while (song.isPlaying)
        {
            // THIS IS WHERE AN ACTION WOULD HAPPEN WHEN THE BEAT HAPPENS
            // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv

            Global.switchStateBPM = true;
            Global.counterBPM += 1;

            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            
            yield return new WaitForSeconds(timeInMeasure);
        }
        yield break;
    }

    // Update is called once per frame
    void Update () {
        
	}
}
