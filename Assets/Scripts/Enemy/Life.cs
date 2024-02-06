using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life: MonoBehaviour
{
    [SerializeField] private int baseHealth;
    private int maxHealth;
    private int currentHealth;


    /// <summary>
    /// Initialize Healt base of entity's level
    /// </summary>
    /// <param name="level"></param>
    public void InitializeLife(int level)
    {
        maxHealth = SetMaxHealth(level);
        currentHealth = maxHealth;
        Debug.Log($"currentHealth: {currentHealth}");
    }

    private int SetMaxHealth(int level)
    {
        return baseHealth * level;
    }

    /// <summary>
    /// Restore Health
    /// </summary>
    /// <param name="restorePoints">Has to be a Positive number</param>
    public void RestoreHealth(int restorePoints) {
        if (restorePoints < 0) {
            restorePoints *= -1; //Convert to positive value
        }
        UpdateLife(restorePoints);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damage">Has to be a Negative number</param>
    public void TakeDamage(int damage) {
        if(damage > 0) {
            damage *= -1; //Convert to negative value
        }
        UpdateLife(damage);
    }

    /// <summary>
    /// Update healt information in UI
    /// </summary>
    /// <param name="pointsToUpdate">Positive values restores health and Negative values apply damage</param>
    private void UpdateLife(int pointsToUpdate)
    {
        if (currentHealth <= maxHealth)
        {
            currentHealth += pointsToUpdate;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        //Update UI
        Debug.Log(currentHealth);
    }

    public bool IsDead()
    {
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"Is dead {currentHealth}");
            return true;
        }
        return false;
    }
}
