using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRevive : MonoBehaviour
{
    private int goodHitsRequired = 4;
	private int goodHits = 0;
	private UnitHealth targetPlayer;
	private ScoreManager scoreManager;

	private void Start()
	{
		scoreManager = FindObjectOfType<ScoreManager>();
	}

	public void SetTargetPlayer(UnitHealth value)
	{
		targetPlayer = value;
	}

	public void SetHitsRequired(int value)
	{
		goodHitsRequired = value;
	}

	private void FixedUpdate()
	{
		if(goodHits >= goodHitsRequired)
		{
			targetPlayer.PlayerGetUp();
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<Glove>() && other.transform.root.GetComponentInChildren<UnitHealth>() == targetPlayer)
		{
			if(scoreManager.GetHitRating(scoreManager.GetScore()) != ScoreManager.HitRating.Miss)
			{
				goodHits++;
				Debug.LogFormat("Good {0}/{1}", goodHits, goodHitsRequired);
			}
			else
			{
				goodHits--;
				Debug.LogFormat("Bad {0}/{1}", goodHits, goodHitsRequired);
			}
		}
	}

	IEnumerator ReviveTimeout(float duration)
	{
		yield return new WaitForSeconds(duration);

		targetPlayer.PlayerFailedGetUp();
		Destroy(gameObject);

		yield break;
	}
}
