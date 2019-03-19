using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(AudioSource))]
public class MenuSphere : MonoBehaviour
{
	[SerializeField] private MenuScriptableObject menuScriptableObject;
	private AudioSource audioSource;
	private int recentHits = 0;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		transform.DOScale(transform.localScale * 1.1f, 3f).SetLoops(-1);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<Glove>() && other.gameObject.GetComponent<Glove>().Velocity > menuScriptableObject.velocityThreshold)
		{
			StopAllCoroutines();
			recentHits++;
			if(recentHits >= menuScriptableObject.hitsToTrigger)
			{
				TriggerAction();
			}
			else
			{
				PlayFeedback();
				StartCoroutine(ResetHitTimer(menuScriptableObject.resetTime));
			}
		}
	}

	private void PlayFeedback()
	{
		audioSource.PlayOneShot(menuScriptableObject.hitSound);
		Instantiate(menuScriptableObject.hitObject, transform);
	}

	private void TriggerAction()
	{
		menuScriptableObject.TriggerEvent(gameObject);
		audioSource.PlayOneShot(menuScriptableObject.triggerSound);
		Instantiate(menuScriptableObject.triggerObject, transform);
		Destroy(this);
	}

	private IEnumerator ResetHitTimer(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		recentHits = 0;
		yield break;
	}
}
