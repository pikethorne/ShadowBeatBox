using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Responsible for updating the values of the scoreboard and playing feedback to the player.
/// </summary>
public class ScoreboardManager : MonoBehaviour
{
	//Red team is on left side of scoreboard while green is on right side.
#pragma warning disable 0649
	[SerializeField] private TextMeshProUGUI redName, greenName, score, roundNumber, timer, redHealth, greenHealth;
	[SerializeField] private Image redFighterImage, greenFighterImage;
#pragma warning restore 0649
	private UnitHealth greenFighter, redFighter;
	private RoundManager roundManager;

	private void Awake()
	{
		roundManager = FindObjectOfType<RoundManager>();
		roundManager.SetScoreboard(this);
	}

	private void Start()
	{
		UpdateScoreboardText();
		redFighter = roundManager.GetRedFighter();
		greenFighter = roundManager.GetGreenFighter();
	}

	private void Update()
	{
		//TODO: This should probably countdown to some sort of time-out, such as time until the end of the song.
		// For now it just displays the amount of time since the scene started running.
		TimeSpan time = TimeSpan.FromSeconds(Time.time);
		timer.text = string.Format("{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
		redHealth.text = "HP:" + Mathf.FloorToInt(redFighter.Health).ToString();
		greenHealth.text = "HP:" + Mathf.FloorToInt(greenFighter.Health).ToString();
	}

	/// <summary>
	/// Updates the text fields to their current values.
	/// </summary>
	public void UpdateScoreboardText()
	{
		redName.text = roundManager.GetRedName();
		greenName.text = roundManager.GetGreenName();
		redFighterImage.sprite = roundManager.GetRedIcon();
		greenFighterImage.sprite = roundManager.GetGreenIcon();
		score.text = roundManager.GetRedScore() + " - " + roundManager.GetGreenScore();
		roundNumber.text = "ROUND " + roundManager.GetCurrentRound() + "/" + roundManager.GetMaxRounds(); 
	}
}
