using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Dummy metronome used to keep beat (transforms approximately every .5seconds)
/// </summary>
/// 

public class Metronome : Timer
{

    bool scaleControl = true;

	// Use this for initialization
	void Start () {
        InvokeRepeating("scaleMetronome", 0.0f, noMusicTimer);
    }

    void scaleMetronome()
    {
        if(scaleControl)
        {
            // Bigger
            gameObject.transform.localScale += new Vector3(3, 3, 3);
            scaleControl = false;
        }
        else if(!scaleControl)
        {
            // Smaller
            gameObject.transform.localScale -= new Vector3(3, 3, 3);
            scaleControl = true;
        }
    }
}
