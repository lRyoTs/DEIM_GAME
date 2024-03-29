
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("References")]
    private PlayerLife _playerLife;
    private LevelSystem _levelInfo;

    [Header("Player Base Stats")]
    [SerializeField] private int baseHp;
    [SerializeField] private int baseAttack;

    [Header("Player Stats")]
    private int maxHp;
    private int attackCeilling;
    private int attackFloor;


    private void Awake()
    {
        _levelInfo = GetComponent<LevelSystem>();
        _playerLife = GetComponent<PlayerLife>();
        
        EventManager.AddHandler(EventManager.EVENT.OnLevelUp,CalculateStats);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(EventManager.EVENT.OnLevelUp, CalculateStats);   
    }

    public void CalculateStats() {
        maxHp = _levelInfo.Level * baseHp;
        attackCeilling = _levelInfo.Level * baseAttack * 5;
        attackFloor = baseAttack * _levelInfo.Level;
        _playerLife.SetMaxHealth(maxHp);
    }

    public int GetAttackDmg() {
        return Random.Range(attackFloor, attackCeilling);
    }

}
