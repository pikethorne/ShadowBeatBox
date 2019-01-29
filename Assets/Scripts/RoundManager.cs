using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for keeping track of round score and management of round starting/ending.
/// </summary>
public class RoundManager : MonoBehaviour
{
	#region Fields
	private int currentRound = 1;
	private int numberOfRounds = 3;
	private int victoryThreshold = 2;
	private int redScore = 0;
	private int greenScore = 0;
	private string redName = "Defender";
	private string greenName = "Challenger";
	private List<UnitHealth> redTeam = new List<UnitHealth>();
	private List<UnitHealth> greenTeam = new List<UnitHealth>();
	private ScoreboardManager scoreboard;
	bool redActive = false;
	bool greenActive = false;
	#endregion

	#region Getters/Setters
	public int GetCurrentRound()
	{
		return currentRound;
	}
	public string GetRedName()
	{
		return redName;
	}
	public string GetGreenName()
	{
		return greenName;
	}
	public int GetRedScore()
	{
		return redScore;
	}
	public int GetGreenScore()
	{
		return greenScore;
	}
	public int GetMaxRounds()
	{
		return numberOfRounds;
	}
	#endregion

	/// <summary>
	/// Scans the scene for boxers and assigns them to teams.
	/// </summary>
	public void ScanSceneForBoxers()
	{
		//TODO: As of right now it only support one unit per team, if we change the logic there can be multiple fighters on a team.
		int i = 1;
		foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
		{
			switch(i)
			{
				case 1:
					redTeam.Add(unit);
					i++;
					break;
				case 2:
					greenTeam.Add(unit);
					i++;
					break;
				case 3:
					Debug.LogError("There are too many units present in the scene.");
					break;
			}
		}
	}

	/// <summary>
	/// Begins a new round by resetting the health of all units present.
	/// </summary>
	public IEnumerator StartRound()
	{
		foreach(UnitHealth unit in FindObjectsOfType<UnitHealth>())
		{
			unit.InitializeUnit();
		}
		//TODO: Disable combat preventing early hits.
		//TODO: Play animation for round countdown.
		yield return new WaitForSeconds(5);
		//TODO: Begin music.
		//TODO: Play whistle sound.
		yield break;
	}

	//Called every physics update
	private void FixedUpdate()
	{
		//Checks if the round has ended. This is quite costly to be run in fixed update and there is likely a more efficient approach.
		if (CheckForRoundEnd())
		{
			if		(redActive && !greenActive) StartCoroutine(EndRound(BoxingTeams.red));
			else if (!redActive && greenActive) StartCoroutine(EndRound(BoxingTeams.green));
			else	Debug.LogWarning("Round was thought to have ended but there is not a winning team.");
		}
	}

	//Called before Start().
	private void Awake()
	{
		ScanSceneForBoxers();
		StartCoroutine(StartRound());
	}

	/// <summary>
	/// Increases the score for the winner. Then determines if there is a winner of the match. If there isn't will automatically start next round.
	/// </summary>
	public IEnumerator EndRound(BoxingTeams roundWinner)
	{
		//Awards score and plays animation based off of the winner.
		switch (roundWinner)
		{
			case BoxingTeams.red:
				scoreboard.GetComponent<Animator>().Play("RedRound");
				redScore++;
				break;
			case BoxingTeams.green:
				scoreboard.GetComponent<Animator>().Play("GreenRound");
				greenScore++;
				break;
		}

		yield return new WaitForSeconds(1);

		scoreboard.UpdateScoreboardText();
		scoreboard.GetComponent<AudioSource>().Play();

		yield return new WaitForSeconds(4);

		currentRound++;

		//Match end conditions
		{
			if (redScore >= victoryThreshold)
			{
				//TODO: Red victory
				scoreboard.GetComponent<Animator>().Play("RedMatch");
				yield break;
			}
			else if (greenScore >= victoryThreshold)
			{
				//TODO: Green victory
				scoreboard.GetComponent<Animator>().Play("GreenMatch");
				yield break;
			}
			else if (currentRound > numberOfRounds)
			{
				//TODO: Draw, such as 1-1 in best of 2
				yield break;
			}
		}

		scoreboard.UpdateScoreboardText();

		StartCoroutine(StartRound()); //Starts a new round since the match end conditions didn't end the IEnumerator.

		yield break;
	}

	/// <summary>
	/// Checks if a team has been kncoked out. 
	/// </summary>
	/// <returns>Returns true when a team is no longer able to fight.</returns>
	public bool CheckForRoundEnd()
	{
		//Check if red team is active.
		foreach (UnitHealth boxer in redTeam)
		{
			if(!boxer.IsKnockedOut)
			{
				redActive = true;
				break;
			}
		}
		
		//Check if green team is active.
		foreach (UnitHealth boxer in greenTeam)
		{
			if (!boxer.IsKnockedOut)
			{
				greenActive = true;
				break;
			}
		}

		return !redActive || !greenActive; //If either red or green is not active return true, since one team is no longer fighting.
	}

	public enum BoxingTeams
	{
		red, green
	}

	/// <summary>
	/// Sets the scoreboard component to the argument. Should be called from other classes to assign them as the scoreboard.
	/// </summary>
	/// <param name="board"></param>
	public void SetScoreboard(ScoreboardManager board)
	{
		scoreboard = board;
	}

	/// <summary>
	/// DO NOT USE THIS IT IS JUST FOR TESTING!
	/// </summary>
	[System.Obsolete]
	[ContextMenu("Red Point")]
	public void RedWinner()
	{
		StartCoroutine(EndRound(BoxingTeams.red));
	}

	/// <summary>
	/// DO NOT USE THIS IT IS JUST FOR TESTING!
	/// </summary>
	[System.Obsolete]
	[ContextMenu("Green Point")]
	public void GreenWinner()
	{
		Debug.Log("It happened");
		StartCoroutine(EndRound(BoxingTeams.green));
	}


}