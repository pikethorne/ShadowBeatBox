using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatController : MonoBehaviour {

    AudioSource song;

    public GameObject nome1;
    public GameObject nome2;
    public GameObject nome3;
    public GameObject nome4;
    public GameObject nomeAct;
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

        public BeatObject(GameObject g, float beatNumerator, float beatDenominator, float measuresToWait = 0)
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
        BeatObject beat1 = new BeatObject(nome1, 1, 8);
        BeatObject beat2 = new BeatObject(nome2, 2, 8);
        BeatObject beat3 = new BeatObject(nome3, 3, 8);
        BeatObject beat4 = new BeatObject(nome4, 4, 8);
        BeatObject beat5 = new BeatObject(nome4, 5, 8);
        BeatObject beat6 = new BeatObject(nome4, 6, 8);
        BeatObject beat7 = new BeatObject(nome4, 7, 8);
        BeatObject beat8 = new BeatObject(nome4, 8, 8);
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
            StartCoroutine(QueueBeat(b.ObjectPassed, b.Numerator, b.Denominator, b.MeasuresToWait));
        }
    }
    
    /// <summary>
    /// Coroutine that loops a specific amount of time depending on the Beat specified.
    /// </summary>
    /// <param name="g">Game Object if Necessary</param>
    /// <param name="beatNumerator"></param>
    /// <param name="beatDenominator"></param>
    /// <param name="measuresToWait">For Specific Beats To Happen Once</param>
    /// <returns></returns>
    IEnumerator QueueBeat(GameObject g, float beatNumerator, float beatDenominator, float measuresToWait = 0)
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
