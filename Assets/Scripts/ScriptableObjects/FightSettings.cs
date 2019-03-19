using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BeatBox/Fight Settings")]
public class FightSettings : ScriptableObject
{
	[Header("Fight Settings")]
	[Range(1, 9)] public int maxNumberOfRounds = 3;
	[Tooltip("Number of round wins in order to win game. Should not exceed max number of rounds.")]
	[Range(1, 9)] public int winningThreshold = 2;
}
