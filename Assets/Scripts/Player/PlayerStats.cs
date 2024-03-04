using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [Header("Player Base Stats")]
    [SerializeField] private int baseHp;
    [SerializeField] private int baseAttack;

    [Header("Player Stats")]
    [SerializeField] private LevelSystem levelInfo;
    [SerializeField] private int maxHp;
    [SerializeField] private int attackCeilling;
    [SerializeField] private int attackFloor;

    private void Awake()
    {
        levelInfo = GetComponent<LevelSystem>();
        CalculateStats();
    }

    public void CalculateStats() {
        maxHp = levelInfo.Level * baseHp;
        attackCeilling = levelInfo.Level * baseAttack * 5;
        attackFloor = baseAttack * levelInfo.Level;
    }

    public int GetAttackDmg() {
        return Random.Range(attackFloor, attackCeilling);
    }

}
