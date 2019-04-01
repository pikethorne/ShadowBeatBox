using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global
{
	/// <summary>
	/// Plays a random audio from the array of sound files.
	/// </summary>
	/// <param name="clips">The array of sound files to trigger from</param>
	public static void PlayRandomAudio(AudioClip[] clips, AudioSource audioSource)
	{
		try
		{
			audioSource.PlayOneShot(clips[UnityEngine.Random.Range(0, clips.Length)]);
		}
		catch
		{
			return;
		}
	}
}