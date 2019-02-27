using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnitHealth is responsible for managing the health of a unit and taking action accordingly. 
/// </summary>
[RequireComponent(typeof(UnitManager))]
public class UnitHealth : MonoBehaviour
{
	private float currentHealth;
	[SerializeField] private TMPro.TextMeshPro healthText;
	private Animator animator;
    /// <summary>
    /// Number of times they have been knocked unconcious.
    /// </summary>
    private int timesKnockedDown = 0;
	private bool blocking;
	private static readonly int knockdownLimit = 3;
	private UnitProperties properties;

	public float Health
	{
		get
        {
            return currentHealth;
        }
		set
		{
			currentHealth = value;
		}
	}
	public bool Immune
	{
		get;
		set;
	}

	/// <summary>
	/// True when they have been knockeddown
	/// </summary>
	public bool KnockedDown
	{
		get;
		private set;
	}

    /// <summary>
    /// True if they have been knocked down and failed to get back up.
    /// </summary>
    public bool IsUnconcious
    {
		get;
		set;
    }

    /// <summary>
    /// Decides if the player has been knocked out.
    /// </summary>
    private void CheckForKnockout()
	{
		if(Health <= 0 && !KnockedDown)
		{
			//Increments the times knocked down and sets the unit to be immune.

			timesKnockedDown++;
			Immune = true;
			KnockedDown = true;
			if (GetComponent<SimplestAI>()) //AI
			{
				StartCoroutine(AIKnockdown());
			}
			else //Player
			{
				IsUnconcious = true; //TODO: Until we implement player revives, the player will just not get up.
			}
		}
	}

	IEnumerator AIKnockdown()
	{
		//Disables colliders preventing accidental hits
		List<Collider> childColliders = new List<Collider>(GetComponentsInChildren<Collider>());
		foreach(Collider collider in childColliders)
		{
			collider.enabled = false;
		}

		//Predetermines if the unit will get up.
		bool willGetUp = false;
		if(timesKnockedDown < knockdownLimit && UnityEngine.Random.Range(0f, 1f) < (properties.getUpChance / timesKnockedDown)) //Will get up if they haven't exceeded their get up limit and got lucky.
		{
			willGetUp = true;
		}

		//Plays animations
		animator.Play("Knockdown");
		yield return new WaitForSeconds(2);
		animator.Play("KnockdownGetUpAttempt");
		yield return new WaitForSeconds(1);

		//First attempt to get up.
		if((willGetUp == true && UnityEngine.Random.Range(0f, 1f) > properties.getUpChance) || willGetUp == false) //Sometimes does a failed attempt to get up even if they will get up later.
		{
			animator.Play("KnockdownGetUpFail");
		}
		else if(willGetUp == true) //If they did not fail to get up, will get up now.
		{
			SuccessfulAIGetUp(childColliders);
			yield break;
		}

		//After waiting a little bit, will try to get up again.
		yield return new WaitForSeconds(4);
		animator.Play("KnockdownGetUpAttempt");
		yield return new WaitForSeconds(1);

		//Second attempt to get up
		if (willGetUp == true)
		{
			SuccessfulAIGetUp(childColliders);
			yield break;
		}
		else
		{
			animator.Play("KnockdownGetUpFail");
		}
		yield return new WaitForSeconds(2);

		//This code will only be reached if the unit did not get up.
		IsUnconcious = true;
		animator.Play("KnockdownGetUpAttempt");
		yield return new WaitForSeconds(1);
		foreach (Collider collider in childColliders)
		{
			collider.enabled = true;
		}
		animator.Play("KnockdownGetUpSuccess");
		yield break;
	}

	private void SuccessfulAIGetUp(List<Collider> childColliders)
	{
		foreach (Collider collider in childColliders)
		{
			collider.enabled = true;
		}
		animator.Play("KnockdownGetUpSuccess");
		Health = properties.maxHealth * properties.getUpHealthRecovered;
		KnockedDown = false;
		Immune = false;
	}

	// Use this for initialization
	void Start ()
	{
		InitializeUnit();
		animator = GetComponent<Animator>();
		properties = GetComponent<UnitManager>().GetProperties();
	}

	public void InitializeUnit()
	{
		Health = properties.maxHealth;
		Immune = true;
		IsUnconcious = false;
		KnockedDown = false;
		timesKnockedDown = 0;
	}

    /// <summary>
    /// This method is not intended to be used outside of testing. It should eventually be removed when a true health UI is created.
    /// </summary>
    private void TestHealthText()
	{
		if (healthText)
		{
			healthText.text = "HP:" + Mathf.FloorToInt(Health).ToString();
		}
	}

	public void DealDamage(float damage)
	{
		Health -= damage;
		TestHealthText();
		CheckForKnockout();
	}

	/// <summary>
	/// Returns the current health percentage of this unit from 1 to 0 (100% to 0%). 
    /// Can return values out of the 1 to 0 range.
	/// </summary>
	public float GetHealthPercentage()
	{
		return currentHealth / properties.maxHealth;
	}

	/// <summary>
	/// Returns the current health percentage of this unit from 1 to 0 (100% to 0%).
	/// Will never be greater than 1 or less than 0. 
	/// </summary>
	public float GetHealthPercentageClamped()
	{
		return Mathf.Clamp(currentHealth / properties.maxHealth, 0, 1);
	}

	public IEnumerator Block(float blockingLength)
	{
		if (blocking) yield return null;
		blocking = true;
		Immune = true;
		yield return new WaitForSeconds(blockingLength);
		Immune = false;
		blocking = false;
	}

	public UnitProperties GetProperties()
	{
		if (properties) return properties;
		else return null;
	}
}
