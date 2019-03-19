using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BeatBox/Arena Sounds")]
public class ArenaSoundset : ScriptableObject
{
	[Header("Round State SFX")]
	public AudioClip roundStart;
	public AudioClip roundEnd;
	public AudioClip scoreboardUpdate;
	public AudioClip greenWinner;
	public AudioClip redWinner;
	public AudioClip draw;

	[Header("Countdown SFX")]
	public AudioClip countdownThree;
	public AudioClip countdownTwo;
	public AudioClip countdownOne;
}
