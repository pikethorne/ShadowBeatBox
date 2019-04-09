using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnitStatus is responsible for managing the state of a unit and taking action accordingly. 
/// This includes managing properties such as health, knockdowns, immunity, etc.
/// </summary>
[RequireComponent(typeof(UnitManager))]
public class UnitStatus : MonoBehaviour
{
	#region Fields
	private float currentHealth;
	private Animator animator;
    private int timesKnockedDown = 0;
	private bool blocking;
	private static readonly int knockdownLimit = 3;
	private UnitProperties properties;
	List<Collider> gloveColliders;
	List<Glove> gloves;
	#endregion

	#region Properties
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

	public bool ImmunePenalty
	{
		get;
		set;
	}

	public int Exhaustion
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

	public UnitProperties GetProperties()
	{
		if (properties) return properties;
		else return null;
	}
	#endregion

	void Awake()
	{
		properties = GetComponent<UnitManager>().GetProperties();
		InitializeUnit();
		animator = GetComponent<Animator>();
		gloveColliders = new List<Collider>(GetComponentsInChildren<Collider>());
		gloves = new List<Glove>(GetComponentsInChildren<Glove>());
	}

	void OnEnable()
	{
		BeatController.BeatEvent += DecreaseExhaustion;
	}

	void OnDisable()
	{
		BeatController.BeatEvent -= DecreaseExhaustion;
	}

	private void DecreaseExhaustion()
	{
		if (Exhaustion != 0 && !Immune && !ImmunePenalty)
		{
			Exhaustion = Mathf.Max(Exhaustion - 1, 0);
		}
	}

	/// <summary>
	/// Decides if the player has been knocked out and will execute off of it when they have been knocked out.
	/// </summary>
	private void CheckForKnockout()
	{
		if(Health <= 0 && !KnockedDown)
		{
			//Increments the times knocked down and sets the unit to be immune.
			timesKnockedDown++;
			Immune = true;
			KnockedDown = true;

			if (GetComponent<SimplestAI>())
			{
				StartCoroutine(AIKnockdown());
			}
			else
			{
				PlayerKnockdown();
			}
		}
	}

	#region Player KO Methods
	private void PlayerKnockdown()
	{
		foreach(Glove glove in gloves)
		{
			glove.CombatEnabled = false;
		}

		if (timesKnockedDown <= 3)
		{
			foreach (Collider collider in GetComponents<Collider>())
			{
				collider.enabled = false;
			}

			GameObject playerRevive = Instantiate(Resources.Load<GameObject>("PlayerRevive"), transform.position + (0.5f * transform.forward), Quaternion.identity);
			PlayerRevive reviveComponent = playerRevive.GetComponent<PlayerRevive>();
			reviveComponent.SetTargetPlayer(this);
			reviveComponent.StartCoroutine("ReviveTimeout", properties.reviveDuration);
		}
		else
		{
			PlayerFailedGetUp();
		}
	}

	public void PlayerGetUp()
	{
		foreach (Glove glove in gloves)
		{
			glove.CombatEnabled = true;
		}
		KnockedDown = false;
		Immune = false;
		foreach (Collider collider in GetComponents<Collider>())
		{
			collider.enabled = true;
		}
		Health = properties.maxHealth * properties.getUpHealthRecovered;
	}

	public void PlayerFailedGetUp()
	{
		foreach (Glove glove in gloves)
		{
			glove.CombatEnabled = true;
		}
		IsUnconcious = true;
		foreach (Collider collider in GetComponents<Collider>())
		{
			collider.enabled = true;
		}
	}
	#endregion

	#region AI KO Methods
	IEnumerator AIKnockdown()
	{
		foreach(Collider collider in gloveColliders)
		{
			collider.enabled = false;
		}

		//Predetermines if the unit will get up.
		bool willGetUp = false;
		//Will get up if they haven't exceeded their get up limit and got lucky.
		if (timesKnockedDown < knockdownLimit && UnityEngine.Random.Range(0f, 1f) < (properties.getUpChance / timesKnockedDown)) 
		{
			willGetUp = true;
		}

		//Plays animations
		animator.Play("Knockdown");
		yield return new WaitForSeconds(2);
		animator.Play("KnockdownGetUpAttempt");
		yield return new WaitForSeconds(1);

		//First attempt to get up. Even if they have been determined to get up later they have a chance to fail getting up for the first time.
		if ((willGetUp == true && UnityEngine.Random.Range(0f, 1f) > properties.getUpChance) || willGetUp == false) 
		{
			animator.Play("KnockdownGetUpFail");
		}
		//If they did not fail to get up and were predetermined to get up, they will get up now.
		else if (willGetUp == true) 
		{
			SuccessfulAIGetUp(gloveColliders);
			yield break;
		}

		//After waiting a little bit, will try to get up again.
		yield return new WaitForSeconds(4);
		animator.Play("KnockdownGetUpAttempt");
		yield return new WaitForSeconds(1);

		//Second attempt to get up, if they were determined to get up before they will now. Otherwise they will fail to get up.
		if (willGetUp == true)
		{
			SuccessfulAIGetUp(gloveColliders);
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
		animator.Play("KnockdownGetUpFail");
		yield return new WaitForSeconds(2.5f);
		foreach (Collider collider in gloveColliders)
		{
			collider.enabled = true;
		}
		animator.Play("KnockdownGetUpSuccess");
		yield break;
	}

	private void SuccessfulAIGetUp(List<Collider> colliders)
	{
		foreach (Collider collider in colliders)
		{
			collider.enabled = true;
		}
		animator.Play("KnockdownGetUpSuccess");
		Health = properties.maxHealth * properties.getUpHealthRecovered;
		KnockedDown = false;
		Immune = false;
	}
#endregion
	
	/// <summary>
	/// Resets the propereties of the unit to their initial values.
	/// </summary>
	public void InitializeUnit()
	{
		Health = properties.maxHealth;
		Immune = true;
		IsUnconcious = false;
		KnockedDown = false;
		timesKnockedDown = 0;
		foreach (Collider collider in GetComponents<Collider>())
		{
			collider.enabled = true;
		}
	}

	/// <summary>
	/// Modifies health value and checks for a knockout.
	/// </summary>
	public void DealDamage(float damage)
	{
		Health -= damage;
		CheckForKnockout();
	}

	/// <summary>
	/// Will grant immunity for the provided duration.
	/// </summary>
	public IEnumerator Block(float blockingLength)
	{
		if (blocking) yield break;
		blocking = true;
		Immune = true;
		yield return new WaitForSeconds(blockingLength);
		Immune = false;
		blocking = false;
	}
}
