using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI state machine for Shadowboxing
/// Uses enum for state machines along with switch statement
/// </summary>
[RequireComponent(typeof(UnitStatus))]
public class SimplestAI : MonoBehaviour
{
    MoveContainer moveContainer;
    Animator animator;
    MoveList activeMoveList;
	UnitStatus unitHealth;

    public const string path = "MoveDatabase";

    int stateDuration = 0;
    int activeMoveIndex = 4;
	internal Transform cameraTransform;
	public int randomMoveList;

	public bool trackingPlayer = true;

    /// <summary>
    /// Start function, initializes state as well as starts state machine
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        moveContainer = MoveContainer.Load(path);
		unitHealth = GetComponent<UnitStatus>();
		cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}

	private void FixedUpdate()
	{
		if (!unitHealth.KnockedDown && trackingPlayer)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, Mathf.Max(0.675f, cameraTransform.position.y - 0.5f), transform.position.z), 0.01f);
			transform.LookAt(cameraTransform);
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);
		}
	}

	private void OnEnable()
	{
		BeatController.BeatEvent += BeatController_BeatEvent;
	}

	private void OnDisable()
	{
		BeatController.BeatEvent -= BeatController_BeatEvent;
	}

	private void BeatController_BeatEvent()
	{
		if(unitHealth.KnockedDown)
		{
			return;
		}

		if (stateDuration <= 1)
		{
			UpdateState();
		}
		else
		{
			stateDuration--;
		}
	}

	public void SetTracking(int tracking)
	{
		trackingPlayer = tracking != 0;
	}

    /// <summary>
    /// Handles the state change
    /// </summary>
    private void UpdateState()
    {
        if (activeMoveIndex >= 4)
        {
            randomMoveList = Random.Range(0, moveContainer.moves.Count);
            activeMoveList = moveContainer.moves[randomMoveList];
            activeMoveIndex = 0;

            EnemyActions(activeMoveList.moves.Split(',')[activeMoveIndex]);
            stateDuration = int.Parse(activeMoveList.duration.Split(',')[activeMoveIndex]);
        }
        else
        {
            EnemyActions(activeMoveList.moves.Split(',')[activeMoveIndex]);

            stateDuration = int.Parse(activeMoveList.duration.Split(',')[activeMoveIndex]);
        }

        activeMoveIndex = activeMoveIndex + 1;
    }

    /// <summary>
    /// Enemy Action state machine
    /// </summary>
    private void EnemyActions(string animationTransition)
    {
        if ( animationTransition == "RightJabTell" || animationTransition == "RightJab" )
        {
            trackingPlayer = false;
        }
        else
        {
            trackingPlayer = true;
        }
		animator.Play(animationTransition, 0, 0f);
	}
}