using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    private const int LEVEL_VARIANCE = 3;

    #region Variables
    [Header("References")]
    private EnemyLife _enemyLife;

    [Header("Enemy Base Stats")]
    [SerializeField] private int baseHp = 10;
    [SerializeField] private int baseExp = 30;
    [SerializeField] private int baseDmg = 10;

    [Header("Enemy Info")]
    [SerializeField] private int level = 1;
    [SerializeField] private float enemySpeed;

    [Header("Player Stats")]
    private int maxHp;
    private int attackCeilling;
    private int attackFloor;

    [Header("UI")]
    [SerializeField] private string enemyName;
    [SerializeField] private TextMeshProUGUI enemyInfo;
    #endregion

    private void Awake()
    {
        _enemyLife = GetComponent<EnemyLife>();
    }

    private void Start()
    {
        SetEnemyLevel();
        CalculateStats();
        enemyInfo.text = $"Lv{level} {enemyName}";
    }

    public void CalculateStats()
    {
        maxHp = level * baseHp;
        attackCeilling = level * baseDmg* 5;
        attackFloor = baseDmg * level;
        _enemyLife.SetMaxHealth(maxHp);
    }

    private void SetEnemyLevel() {
        int playerLevel = DataPersistence.Instance.PlayerCurrentLevel;
        int maxLevel = playerLevel + LEVEL_VARIANCE + 1;
        int minLevel = playerLevel <= LEVEL_VARIANCE ? playerLevel : playerLevel - LEVEL_VARIANCE;
        level = Random.Range(minLevel, maxLevel);
    }

    public int GetDamage()
    {
        return Random.Range(attackFloor, attackCeilling);
    }

    public int GetExperienceValue()
    {
        return level * baseExp;
    }
}
