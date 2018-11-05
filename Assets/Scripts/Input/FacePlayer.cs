using UnityEngine;

/// <summary>
/// Simply gets the player headset and rotates this gameobject to face it.
/// </summary>
public class FacePlayer : MonoBehaviour
{
	public GameObject playerCamera;
	[SerializeField] private Vector3 rotationOffset = new Vector3(0, 180, 0);

	public void Start()
	{
		playerCamera = FindObjectOfType<Valve.VR.InteractionSystem.Player>().GetComponentInChildren<Camera>().gameObject;
	}

	public void Update()
	{
		gameObject.transform.LookAt(playerCamera.transform.position);
		gameObject.transform.Rotate(rotationOffset);
        Debug.Log("I dun faced you");
	}
}
