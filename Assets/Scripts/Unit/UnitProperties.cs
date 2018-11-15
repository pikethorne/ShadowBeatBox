using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The UnitProperties class is a scriptable object which is responsible for storing values for different units such as sound files, gameplay, and other metadata such as name or description.
/// </summary>
[CreateAssetMenu(menuName = "BeatBox/Unit Properties")]
public class UnitProperties : ScriptableObject
{
	[Header("Metadata")]
	public string title;
	[TextArea] public string description;
	public Sprite icon;

	[Header("Gameplay")]
	public int maxHealth;
	[Tooltip("The amount of velocity required for a punch to impact this unit.")]
	[Range(0f, 2f)]public float hitThreshold = 1;

	[Header("Audio")]
	public AudioEvent[] hitAudioLines;
	public AudioEvent[] hitFailedAudioLines;
}

[Serializable]
public struct AudioEvent
{
	public AudioClip audioClip;
	[Tooltip("When this sound is selected, what are the odds it will go through with it?")]
	public float playChance;

	public AudioEvent(AudioClip clip, float chance = 1)
	{
		audioClip = clip;
		playChance = chance;
	}
}