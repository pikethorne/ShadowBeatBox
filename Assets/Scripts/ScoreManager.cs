using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public float timeSinceBeat;
    private BeatController beatObj;

    // Start is called before the first frame update
    void Start()
    {

        timeSinceBeat = 0;
        beatObj = new BeatController();

    }

    // Update is called once per frame
    void Update()
    {

        timeSinceBeat += Time.deltaTime;

    }

    public float CheckScore()
    {
        
        float score = 0;

        float badEarly = beatObj.TimeInMeasure * 0.7f;
        // -0.3 -> 0.3
        float badLate = beatObj.TimeInMeasure * 0.3f;

        float goodEarly = beatObj.TimeInMeasure * 0.8f;
        // -0.2 -> 0.2
        float goodLate = beatObj.TimeInMeasure * 0.2f;

        float niceEarly = beatObj.TimeInMeasure * 0.88f;
        // -0.12 -> 0.12
        float niceLate = beatObj.TimeInMeasure * 0.12f;

        float exEarly = beatObj.TimeInMeasure * 0.96f;
        // -0.04 -> 0.04
        float exLate = beatObj.TimeInMeasure * 0.04f;

        // Anything else  => MISS

        if ( timeSinceBeat >= 0 && timeSinceBeat < badLate )
        {
            if ( timeSinceBeat >= exLate )
            {
                if ( timeSinceBeat >= niceLate )
                {
                    if ( timeSinceBeat >= goodLate )
                    {
                        // Bad
                    }
                    else
                    {
                        // Good
                    }
                }
                else
                {
                    // Nice
                }
            }
            else
            {
                // Excellent
            }
        }
        else if ( timeSinceBeat >= badLate )
        {
            // Miss
        }

        if ( timeSinceBeat < 0 && timeSinceBeat > badEarly )
        {
            if ( timeSinceBeat <= exEarly )
            {
                if ( timeSinceBeat <= niceEarly )
                {
                    if ( timeSinceBeat <= goodEarly )
                    {
                        // Bad
                    }
                    else
                    {
                        // Good
                    }
                }
                else
                {
                    // Nice
                }
            }
            else
            {
                // Excellent
            }
        }
        else if ( timeSinceBeat <= badEarly )
        {
            // Miss
        }

        return score;

    }

    /*
    * If( counter not equal to last counter )
    * {
    *      Some Timing Variable = Time Update
    *      
    *      Need the timeInMeasure variable from BeatController
    *      This is because if the time a beat takes is 1.5 sec for example then
    *      you could say a hit that happens at 0.2 sec is slightly late and a
    *      hit at 1.3 seconds is slightly early
    * }
    */
}
