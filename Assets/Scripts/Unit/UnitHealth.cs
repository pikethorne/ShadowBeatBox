using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnitHealth is responsible for managing the health of a unit and taking action accordingly. 
/// </summary>
public class UnitHealth : MonoBehaviour
{
	private float currentHealth;
	[SerializeField] private UnitProperties properties;
	[SerializeField] private TMPro.TextMeshPro healthText;
    /// <summary>
    /// Number of times they have been knocked unconcious.
    /// </summary>
    private int timesKnockedDown = 0;
	private bool blocking;

	public float Health
	{
		get
        {
            return currentHealth;
        }
		set
		{
			currentHealth = value;
			//CheckForDeath();
		}
	}

	public bool Immune
	{
		get; set;
	}

	/// <summary>
	/// True when they have been eliminated for the round.
	/// </summary>
	public bool IsKnockedOut
    {
		get;
		set;
    }

    /// <summary>
    /// True when their health is above zero.
    /// </summary>
    public bool IsUnconcious
    {
        get
        {
            return Health <= 0;
        }
    }

    /// <summary>
    /// Decides if the player has been knocked out.
    /// Currently obsolete due to it destroying the GameObject.
    /// </summary>
    [Obsolete]
    private void CheckForDeath()
	{
		if(Health <= 0)
		{
			Debug.Log("Death has happened. Please make this do something other than destroying the object.");
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start ()
	{
		InitializeUnit();
	}

	public void InitializeUnit()
	{
		Health = properties.maxHealth;
		Immune = true;
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

	public IEnumerator Block(float blockingLength)
	{
		if (blocking) yield return null;
		blocking = true;
		Immune = true;
		yield return new WaitForSeconds(blockingLength);
		Immune = false;
		blocking = false;
	}
}
