using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glove : MonoBehaviour {
	private float velocity;
	public float Velocity{
		get{
			if (velocity < 0)
				return velocity * -1;
			return velocity;
		}
		set{
			velocity = value;
		}
	}
	public Vector3 lastPosition;

	// Update is called once per frame
	void Update () {
		//TODO: This velocity code works but can sometimes be inconsistent. Might be better to use multiple values and get the average for a more accurate value.
		velocity = (lastPosition.magnitude - gameObject.transform.position.magnitude) / Time.deltaTime;
		lastPosition = gameObject.transform.position;
	}
}
