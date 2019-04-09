using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRevive : MonoBehaviour
{
    private int goodHitsRequired = 4;
	private int goodHits = 0;
	private UnitStatus targetPlayer;
	private ScoreManager scoreManager;

	private void Start()
	{
		scoreManager = FindObjectOfType<ScoreManager>();
	}

	public void SetTargetPlayer(UnitStatus value)
	{
		targetPlayer = value;
	}

	public void SetHitsRequired(int value)
	{
		goodHitsRequired = value;
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
		List<Transform> children = new List<Transform>();
		for(int i = 0; i < transform.childCount; i++)
		{
			children.Add(transform.GetChild(i));
		}
		foreach(Transform trans in children)
		{
			trans.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f, 1, 0);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//If the trigger enter was a glove and the attacking glove is the unit trying to be revived.
		if (other.gameObject.GetComponent<Glove>() && other.transform.root.GetComponentInChildren<UnitStatus>() == targetPlayer)
		{
			//If the hit was acceptable timing, increment the good hits. Otherwise decrease it to discourage spamming.
			if (scoreManager.GetHitRating() != ScoreManager.HitRating.Miss)
			{
				goodHits++;
				Debug.LogFormat("Good {0}/{1}", goodHits, goodHitsRequired);
			}
			else
			{
				goodHits = 0;
				Debug.LogFormat("Bad {0}/{1}", goodHits, goodHitsRequired);
			}

			//After the hit has been judged check if the revive threshold has been reached.
			if (goodHits >= goodHitsRequired)
			{
				targetPlayer.PlayerGetUp();
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// After the specified amount of time will determine that the player has failed to be revived.
	/// </summary>
	IEnumerator ReviveTimeout(float duration)
	{
		yield return new WaitForSeconds(duration);

		targetPlayer.PlayerFailedGetUp();
		Destroy(gameObject);

		yield break;
	}
}
