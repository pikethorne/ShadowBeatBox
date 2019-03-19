using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for keeping track of round score and management of round starting/ending.
/// </summary>
[RequireComponent(typeof(BeatController))]
public class RoundManager : MonoBehaviour
{
	#region Fields
	private int currentRound = 1;
	private int numberOfRounds = 3;
	private int victoryThreshold = 2;
	private int redScore = 0;
	private int greenScore = 0;
	private string redName = "Defender", greenName = "Challenger";
	private Sprite redIcon, greenIcon;
	private List<UnitHealth> redTeam = new List<UnitHealth>();
	private List<UnitHealth> greenTeam = new List<UnitHealth>();
	private ScoreboardManager scoreboard;
	private AudioSource audioSource, scoreboardAudio;
	private BeatController beatController;
	private Animator animator, scoreboardAnimator;
	[SerializeField] private FightPlaylist playlist;
	[SerializeField] private AudioClip whistle, bell, three, two, one, winner, loser, draw;
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
	public Sprite GetGreenIcon()
	{
		return greenIcon;
	}
	public Sprite GetRedIcon()
	{
		return redIcon;
	}
	public UnitHealth GetGreenFighter()
	{
		return greenTeam[0];
	}
	public UnitHealth GetRedFighter()
	{
		return redTeam[0];
	}
	public bool RedActive
	{
		get
		{
			foreach (UnitHealth boxer in redTeam)
			{
				if (!boxer.IsUnconcious)
				{
					return true;
				}
			}
			return false;
		}
	}
	public bool GreenActive
	{
		get
		{
			foreach (UnitHealth boxer in greenTeam)
			{
				if (!boxer.IsUnconcious)
				{
					return true;
				}
			}
			return false;
		}
	}
	#endregion

	/// <summary>
	/// Scans the scene for boxers and assigns them to teams.
	/// </summary>
	public void ScanSceneForBoxers()
	{
		//TODO: As of right now it only support one unit per team, if we change the logic there can be multiple fighters on a team.
		if(FindObjectsOfType<UnitHealth>().Length < 2)
		{
			Debug.LogError("There is not enough Unit Health components present in the scene to make rounds work. Destroying the Round Manager to prevent chaos.");
			Destroy(this);
		}
		redTeam.Clear();
		greenTeam.Clear();
		int i = 1;
		foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
		{
			switch(i)
			{
				case 1:
					redTeam.Add(unit);
					redName = unit.GetProperties().title;
					redIcon = unit.GetProperties().icon;
					i++;
					break;
				case 2:
					greenTeam.Add(unit);
					greenName = unit.GetProperties().title;
					greenIcon = unit.GetProperties().icon;
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
		Song targetSong = playlist.GetSong(GetCurrentState());
		beatController.TriggerBeats = false;
		beatController.StartSong(targetSong.beatsPerMinute);
		animator.Play("SongFadeIn");
		audioSource.clip = targetSong.soundFile;
		audioSource.Play();

		foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
		{
			unit.InitializeUnit();
		}

		scoreboardAnimator.Play("RoundBeginCountdown");

		//TODO: Disable combat preventing early hits.

		yield return new WaitForSeconds(60 / beatController.BPM * (targetSong.beatsToWait - 6));

		scoreboardAudio.PlayOneShot(three);
		yield return new WaitForSeconds(60 / beatController.BPM * 2);

		scoreboardAudio.PlayOneShot(two);
		yield return new WaitForSeconds(60 / beatController.BPM * 2);

		scoreboardAudio.PlayOneShot(one);
		yield return new WaitForSeconds(60 / beatController.BPM * 2);

		scoreboardAnimator.Play("RoundBegin");
		scoreboardAudio.PlayOneShot(bell);
		beatController.TriggerBeats = true;
		foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
		{
			unit.Immune = false;
		}


		yield break;
	}

	private SongTrigger GetCurrentState()
	{
		if(GetCurrentRound() == 1)
		{
			return SongTrigger.MatchStart;
		}
		else if(GetCurrentRound() == numberOfRounds)
		{
			return SongTrigger.FinalRound;
		}
		else
		{
			return SongTrigger.Fighting;
		}
	}

	//Called every physics update
	private void FixedUpdate()
	{
		//Checks if the round has ended. This is quite costly to be run in fixed update and there is likely a more efficient approach.
		if		(!GreenActive) StartCoroutine(EndRound(BoxingTeams.red));
		else if (!RedActive) StartCoroutine(EndRound(BoxingTeams.green));
	}

	//Called before Start().
	private void Awake()
	{
		animator = GetComponent<Animator>();
		beatController = GetComponent<BeatController>();
		audioSource = GetComponent<AudioSource>();
		ScanSceneForBoxers();
	}

	private void Start()
	{
		scoreboardAudio = scoreboard.GetComponent<AudioSource>();
		scoreboardAnimator = scoreboard.GetComponent<Animator>();
		StartCoroutine(StartRound());
	}

	/// <summary>
	/// Increases the score for the winner. Then determines if there is a winner of the match. If there isn't will automatically start next round.
	/// </summary>
	public IEnumerator EndRound(BoxingTeams roundWinner)
	{
		animator.Play("SongFadeOut");
		beatController.StopSong();
		foreach (UnitHealth unit in FindObjectsOfType<UnitHealth>())
		{
			unit.InitializeUnit();
		}
		scoreboardAudio.PlayOneShot(whistle, 1);

		//Awards score and plays animation based off of the winner.
		switch (roundWinner)
		{
			case BoxingTeams.red:
				scoreboardAnimator.Play("RedRound");
				redScore++;
				break;
			case BoxingTeams.green:
				scoreboardAnimator.Play("GreenRound");
				greenScore++;
				break;
		}

		yield return new WaitForSeconds(1);

		scoreboard.UpdateScoreboardText();
		scoreboardAudio.PlayOneShot(whistle, 0.5f);

		yield return new WaitForSeconds(4);

		audioSource.Stop();
		currentRound++;

		//Match end conditions
		{
			if (redScore >= victoryThreshold && redScore > greenScore)
			{
				//TODO: Red victory
				scoreboardAnimator.Play("RedMatch");
				scoreboardAudio.PlayOneShot(winner);
				yield break;
			}
			else if (greenScore >= victoryThreshold && greenScore > redScore)
			{
				//TODO: Green victory
				scoreboardAnimator.Play("GreenMatch");
				scoreboardAudio.PlayOneShot(loser);
				yield break;
			}
			else if (currentRound > numberOfRounds)
			{
				//TODO: Draw, such as 1-1 in best of 2
				scoreboardAnimator.Play("DrawMatch");
				scoreboardAudio.PlayOneShot(draw);
				yield break;
			}
		}

		scoreboard.UpdateScoreboardText();

		StartCoroutine(StartRound()); //Starts a new round since the match end conditions didn't end the IEnumerator.

		yield break;
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
		StartCoroutine(EndRound(BoxingTeams.green));
	}
}