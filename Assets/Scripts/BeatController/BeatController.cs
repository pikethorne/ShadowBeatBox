using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour {

    AudioSource song;
    public float BPM;
    
    List<BeatObject> beatList = new List<BeatObject>();

    /// <summary>
    /// Class that holds information for each Beat.
    /// </summary>
    class BeatObject {
        public GameObject ObjectPassed;
        public float Numerator;
        public float Denominator;
        public float MeasuresToWait;

        public BeatObject(float beatNumerator, float beatDenominator, float measuresToWait = 0, GameObject g = null)
        {
            ObjectPassed = g;
            Numerator = beatNumerator;
            Denominator = beatDenominator;
            MeasuresToWait = measuresToWait;
        }
    }

    float measureLength = 0;
    // Use this for initialization
    void Start () {
        measureLength = 240 / BPM;
        song = GetComponent<AudioSource>();
        StartSong();
    }
    
    /// <summary>
    /// Method that Creates and Runs a Beatlist for 8 beats a measure, then starts the AudioSource Song.
    /// </summary>
    public void StartSong()
    {
        BeatObject beat1 = new BeatObject(1, 8);
        BeatObject beat2 = new BeatObject(2, 8);
        BeatObject beat3 = new BeatObject(3, 8);
        BeatObject beat4 = new BeatObject(4, 8);
        BeatObject beat5 = new BeatObject(5, 8);
        BeatObject beat6 = new BeatObject(6, 8);
        BeatObject beat7 = new BeatObject(7, 8);
        BeatObject beat8 = new BeatObject(8, 8);
        beatList.Add(beat1);
        beatList.Add(beat2);
        beatList.Add(beat3);
        beatList.Add(beat4);
        beatList.Add(beat5);
        beatList.Add(beat6);
        beatList.Add(beat7);
        beatList.Add(beat8);

        RunBeatList();
        song.Play();
    }

    /// <summary>
    /// Method that iterates through the Beat List and runs the QueueBeat Coroutine for each Beat.
    /// </summary>
    void RunBeatList()
    {
        foreach(BeatObject b in beatList){
            StartCoroutine(QueueBeat(b.Numerator, b.Denominator, b.MeasuresToWait, b.ObjectPassed));
        }
    }

    /// <summary>
    /// Coroutine that loops a specific amount of time depending on the Beat specified.
    /// </summary>
    /// <param name="beatNumerator"></param>
    /// <param name="beatDenominator"></param>
    /// <param name="measuresToWait"></param>
    /// <param name="g">Object Reference if Necessary</param>
    /// <returns></returns>
    IEnumerator QueueBeat(float beatNumerator, float beatDenominator, float measuresToWait = 0, GameObject g = null)
    {
        // Math Stuff to determine the time to do beat things
        float timeInMeasure = ((240f / BPM) / beatDenominator) * beatNumerator;
        float buffer = (240f / BPM) / beatDenominator;
        if (measuresToWait != 0)
            yield return new WaitForSeconds((240f / BPM) * measuresToWait);
        yield return new WaitForSeconds(timeInMeasure - buffer);
        int ratioOverflow = (int)beatNumerator / (int)beatDenominator;
        if (ratioOverflow <= 1)
            ratioOverflow = 1;
        while (song.isPlaying)
        {
            // THIS IS WHERE AN ACTION WOULD HAPPEN WHEN THE BEAT HAPPENS
            // vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv

            Global.switchStateFlipper = true;

            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            if (measuresToWait != 0)
                yield break;
            yield return new WaitForSeconds((240f / BPM) * ratioOverflow);
        }
        yield break;
    }

    // Update is called once per frame
    void Update () {
        
	}
}
