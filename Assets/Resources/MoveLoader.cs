using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLoader : MonoBehaviour
{

    public const string path = "MoveDatabase";
    public List<string> parsedDuration = new List<string>();
    public List<string> parsedMoves = new List<string>();

    // Use this for initialization
    void Start()
    {
        MoveContainer mc = MoveContainer.Load(path);

        foreach (Move moves in mc.moves)
        {
            parsedMoves.AddRange(moves.moves.Split(','));
            parsedDuration.AddRange(moves.duration.Split(','));

        }
    }
}
