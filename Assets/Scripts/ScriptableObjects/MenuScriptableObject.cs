using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuScriptableObject : ScriptableObject
{
	[Header("Trigger Fields")]
	public int hitsToTrigger = 4;
	public float resetTime = 1.5f;
	public float velocityThreshold = 2.5f;

	[Header("Sounds to Play")]
	public AudioClip hitSound;
	public AudioClip triggerSound;

	[Header("Objects to Instantiate")]
	public GameObject hitObject;
	public GameObject triggerObject;

	public abstract void TriggerEvent(GameObject gameObject);
}
