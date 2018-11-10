using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour {

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q)) {
            anim.Play("LeftJab", 0, 0f);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            anim.Play("RightJab", 0, 0f);
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            anim.Play("RightJabTell", 0, 0f);
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            anim.Play("NervIdle", 0, 0f);
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            anim.speed = 1 - anim.speed;
        }
    }
}
