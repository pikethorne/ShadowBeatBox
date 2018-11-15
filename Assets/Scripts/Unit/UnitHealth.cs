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

	public float Health
	{
		get { return currentHealth; }
		set
		{
			currentHealth = value;
			CheckForDeath();
		}
	}

	/// <summary>
	/// Decides if the player has died and will execute action if it has.
	/// </summary>
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
		Health = properties.maxHealth;
		TestHealthText();
	}

	/// <summary>
	/// This method is not intended to be used outside of testing. It should eventually be removed when a true health UI is created.
	/// </summary>
	private void TestHealthText()
	{
		if (healthText)
		{
			healthText.text = "HP: " + Health.ToString("n1") + "/" + properties.maxHealth.ToString("n1");
		}
	}

	public void DealDamage(float damage)
	{
		Health -= damage;
		TestHealthText();
	}

	/// <summary>
	/// Returns the current health percentage of this unit from 1 to 0 (100% to 0%).
	/// Will return higher than 1 assuming they have higher than their initial health or less than zero if their health has gone negative.
	/// </summary>
	/// <returns>Returns the current health percentage of this unit.</returns>
	public float GetHealthPercentage()
	{
		return currentHealth / properties.maxHealth;
	}

	/// <summary>
	/// Returns the current health percentage of this unit from 1 to 0 (100% to 0%).
	/// Will never be greater than 1 or less than 0. 
	/// </summary>
	/// <returns>Returns the current health percentage of this unit.</returns>
	public float GetHealthPercentageClamped()
	{
		return Mathf.Clamp(currentHealth / properties.maxHealth, 0, 1);
	}
	
	public UnitProperties GetProperties()
	{
		if (properties) return properties;
		else return null;
	}
}
