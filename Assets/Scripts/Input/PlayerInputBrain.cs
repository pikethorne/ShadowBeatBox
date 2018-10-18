using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerInputBrain : MonoBehaviour {
	private SteamVR_TrackedObject trackedObj;
	private GameObject collidingObject;
	private GameObject objectInHand;

	//private SteamVR_Controller.Device Controller
	//{
	//	get
	//	{
	//		return SteamVR_Controller.Input((int)trackedObj.index);
	//	}
	//}

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.GetComponent<Rigidbody>())
		{
			collidingObject = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		collidingObject = null;
	}

	//private void Update()
	//{
	//	if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
	//	{
	//		Debug.Log("Grip pressed");
	//	}

	//	if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
	//	{
	//		Debug.Log("Grip released");
	//	}
	//}
}
