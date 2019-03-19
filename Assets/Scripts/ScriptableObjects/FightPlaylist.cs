using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// List of songs that play during a match
/// </summary>
[CreateAssetMenu(menuName = "BeatBox/Playlist")]
public class FightPlaylist : ScriptableObject
{
	public List<ContextualSublist> playlistSongs = new List<ContextualSublist>
	{
		new ContextualSublist(SongTrigger.MatchStart),
		new ContextualSublist(SongTrigger.Fighting),
		new ContextualSublist(SongTrigger.FinalRound)
	};

	public Song GetSong(SongTrigger trigger)
	{
		foreach (ContextualSublist sublist in playlistSongs)
		{
			if(sublist.trigger == trigger)
			{
				return sublist.songsToPlay[UnityEngine.Random.Range(0, sublist.songsToPlay.Count - 1)];
			}
		}
		Debug.LogErrorFormat("The playlist {0} was searched for {1} trigger but none was found.", name, trigger.ToString());
		throw new NullReferenceException();
	}
}

[Serializable]
public class ContextualSublist
{
	public string name;
	[Tooltip("The context of the trigger, should be unique between several sublists otherwise only the first gets seen.")]
	public SongTrigger trigger;
	[Tooltip("A list of equally probable songs that randomly play in this context, if there is only one will always play that one.")]
	public List<Song> songsToPlay;

	public ContextualSublist(SongTrigger songTrigger)
	{
		name = songTrigger.ToString();
		trigger = songTrigger;
		songsToPlay = new List<Song>();
	}
}

public enum SongTrigger
{
	MatchStart, Fighting, FinalRound
}