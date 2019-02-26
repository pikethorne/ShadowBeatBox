using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BeatBox/Song")]
public class Song : ScriptableObject
{
	public AudioClip soundFile;
	[Tooltip("How many beats per minute the song is.")]
	public float beatsPerMinute;
	[Tooltip("How many beats to play until the round begins, must have at least 8 beats to give time for countdown.")]
	[Range(8,64)]
	public int beatsToWait = 16;
}
