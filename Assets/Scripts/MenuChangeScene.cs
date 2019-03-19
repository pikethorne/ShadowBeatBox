using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[CreateAssetMenu(menuName = "BeatBox/Menu/Change Scene")]
public class MenuChangeScene : MenuScriptableObject
{
	[Header("Scene Change Fields")]
	public string sceneToChangeTo;
	public bool showVRGrid = false;
	public float fadeOutTime = 2;
	public Color color = new Color(0, 0, 0, 1);

	public override void TriggerEvent(GameObject gameObject)
	{
		SteamVR_LoadLevel.Begin(sceneToChangeTo, showVRGrid, fadeOutTime, color.r, color.g, color.b, color.a);
	}
}
