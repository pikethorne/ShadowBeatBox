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
	[SerializeField] private TextMeshProUGUI redName, greenName, score, roundNumber, timer;
	[SerializeField] private Image redFighterImage, greenFighterImage;
	private RoundManager roundManager;

	private void Start()
	{
		roundManager = FindObjectOfType<RoundManager>();
		roundManager.SetScoreboard(this);
		UpdateScoreboardText();
	}

	private void Update()
	{
		//TODO: This should probably countdown to some sort of time-out, such as time until the end of the song.
		// For now it just displays the amount of time since the scene started running.
		timer.text = Time.time.ToString("N2");
	}

	/// <summary>
	/// Updates the text fields to their current values.
	/// </summary>
	public void UpdateScoreboardText()
	{
		redName.text = roundManager.GetRedName();
		greenName.text = roundManager.GetGreenName();
		score.text = roundManager.GetRedScore() + " - " + roundManager.GetGreenScore();
		roundNumber.text = "ROUND " + roundManager.GetCurrentRound() + "/" + roundManager.GetMaxRounds(); 
	}
}
