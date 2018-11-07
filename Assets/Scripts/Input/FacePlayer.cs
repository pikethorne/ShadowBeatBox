using UnityEngine;

/// <summary>
/// Simply gets the player headset and rotates this gameobject to face it.
/// </summary>
public class FacePlayer : MonoBehaviour
{
	private GameObject playerCamera;
	[SerializeField] private Vector3 rotationOffset = new Vector3(0, 180, 0);

	private void Start()
	{
		playerCamera = FindObjectOfType<Valve.VR.InteractionSystem.Player>().GetComponentInChildren<Camera>().gameObject;
	}

	private void Update()
	{
		gameObject.transform.LookAt(playerCamera.transform.position);
		gameObject.transform.Rotate(rotationOffset);
	}
}
