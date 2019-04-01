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
	private int redScore = 0;
	private int greenScore = 0;
	private string redName = "Defender", greenName = "Challenger";
	private Sprite redIcon, greenIcon;
	private List<UnitStatus> redTeam = new List<UnitStatus>();
	private List<UnitStatus> greenTeam = new List<UnitStatus>();
	private ScoreboardManager scoreboard;
	private AudioSource audioSource, scoreboardAudio;
	private BeatController beatController;
	private Animator animator, scoreboardAnimator;
	[SerializeField] private FightSettings settings;
	[SerializeField] private FightPlaylist playlist;
	[SerializeField] private ArenaSoundset soundset;
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
		return settings.maxNumberOfRounds;
	}
	public Sprite GetGreenIcon()
	{
		return greenIcon;
	}
	public Sprite GetRedIcon()
	{
		return redIcon;
	}
	public UnitStatus GetGreenFighter()
	{
		return greenTeam[0];
	}
	public UnitStatus GetRedFighter()
	{
		return redTeam[0];
	}
	public bool RedActive
	{
		get
		{
			foreach (UnitStatus boxer in redTeam)
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
			foreach (UnitStatus boxer in greenTeam)
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
		if(FindObjectsOfType<UnitStatus>().Length < 2)
		{
			Debug.LogError("There is not enough Unit Health components present in the scene to make rounds work. Destroying the Round Manager to prevent chaos.");
			Destroy(this);
		}
		redTeam.Clear();
		greenTeam.Clear();
		int i = 1;
		foreach (UnitStatus unit in FindObjectsOfType<UnitStatus>())
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

		foreach (UnitStatus unit in FindObjectsOfType<UnitStatus>())
		{
			unit.InitializeUnit();
		}

		scoreboardAnimator.Play("RoundBeginCountdown");

		//TODO: Disable combat preventing early hits.

		yield return new WaitForSeconds(60 / beatController.BPM * (targetSong.beatsToWait - 6));

		scoreboardAudio.PlayOneShot(soundset.countdownThree);
		yield return new WaitForSeconds(60 / beatController.BPM * 2);

		scoreboardAudio.PlayOneShot(soundset.countdownTwo);
		yield return new WaitForSeconds(60 / beatController.BPM * 2);

		scoreboardAudio.PlayOneShot(soundset.countdownOne);
		yield return new WaitForSeconds(60 / beatController.BPM * 2);

		scoreboardAnimator.Play("RoundBegin");
		scoreboardAudio.PlayOneShot(soundset.roundStart);
		beatController.TriggerBeats = true;
		foreach (UnitStatus unit in FindObjectsOfType<UnitStatus>())
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
		else if(GetCurrentRound() == settings.maxNumberOfRounds)
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
		foreach (UnitStatus unit in FindObjectsOfType<UnitStatus>())
		{
			unit.InitializeUnit();
		}
		scoreboardAudio.PlayOneShot(soundset.roundEnd, 1);

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
		scoreboardAudio.PlayOneShot(soundset.scoreboardUpdate, 0.5f);

		yield return new WaitForSeconds(4);

		audioSource.Stop();
		currentRound++;

		//Match end conditions
		{
			if (redScore >= settings.winningThreshold && redScore > greenScore)
			{
				//TODO: Red victory
				scoreboardAnimator.Play("RedMatch");
				scoreboardAudio.PlayOneShot(soundset.redWinner);
				yield break;
			}
			else if (greenScore >= settings.winningThreshold && greenScore > redScore)
			{
				//TODO: Green victory
				scoreboardAnimator.Play("GreenMatch");
				scoreboardAudio.PlayOneShot(soundset.greenWinner);
				yield break;
			}
			else if (currentRound > settings.maxNumberOfRounds)
			{
				//TODO: Draw, such as 1-1 in best of 2
				scoreboardAnimator.Play("DrawMatch");
				scoreboardAudio.PlayOneShot(soundset.draw);
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