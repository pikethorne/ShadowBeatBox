using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoader : MonoBehaviour {

    public const string path = "MoveDatabase";

	// Use this for initialization
	void Start ()
    {
        MoveContainer mc = MoveContainer.Load(path);

        foreach (Move move in mc.moves)
        {
            
        }
	}
}
