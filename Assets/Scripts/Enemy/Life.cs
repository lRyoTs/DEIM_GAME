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

    public void UpdateLife(int pointsToUpdate)
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
            Debug.Log(currentHealth);
            return true;
        }
        return false;
    }
}
