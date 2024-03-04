using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    enum BattleState {
       InBattle,Win,Lose,NextWave,WaitingBattle
    }
    [Header("Battle Info")]
    public List<GameObject>  enemyList = new List<GameObject>();
    private int battleExp;
    private bool isBattleEnd;
    private BattleState _battleState;
    private int enemyInScene = 0;

    [Header("Player Info")]
    [SerializeField] private Transform playerInitialPos;
    private Life playerLife;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else {
            Debug.LogError("There is more than 1 instance of BattleManager");
        }

        _battleState = BattleState.WaitingBattle;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializedPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_battleState) {
            default:
            case BattleState.InBattle:
                CheckEnemyInScene();
                break;
            case BattleState.Win:
                //Show Win Panel
                break;
            case BattleState.Lose:
                //Show Lose Panel
                break;
            case BattleState.NextWave:
                SpawnEnemies();
                break; 
            case BattleState.WaitingBattle:
                isBattleEnd = IsBattleFinished();
                break;
        }
    }

    public void InitializedPlayer()
    {
        GameObject player = FindObjectOfType<PlayerController>().gameObject;
        playerLife = player.GetComponent<Life>();
        //Move Player to BattleScene
        player.transform.position = playerInitialPos.position;
        player.transform.rotation = playerInitialPos.rotation;
    }

    public void SpawnEnemies() {
        while (enemyInScene < 3 && enemyList.Count > 0) {
            Instantiate(enemyList.First(),new Vector3(0,0,0),Quaternion.identity);
            enemyInScene++;
        }

        _battleState = BattleState.InBattle;

    }

    public bool IsBattleFinished() {
        if (playerLife.IsDead()) {
            _battleState = BattleState.Lose;
            return true;
        }

        if (enemyList.Count <= 0) {
            _battleState= BattleState.Win;
            return true;
        }
        _battleState = BattleState.NextWave;
        return false;
    }

    private void CheckEnemyInScene() {
        if (enemyInScene <= 0) {
            _battleState=BattleState.WaitingBattle;
        }
    }

    public void GetEnemyList(List<GameObject> enemyList) {
        enemyList.Clear();
        foreach (GameObject enemy in enemyList) {
            enemyList.Add(enemy);
        }
        CalculateBattleExp();
    }

    private void CalculateBattleExp() {
        battleExp = 0;
        Enemy enemyInfo;
        foreach (GameObject enemy in enemyList) {
            if (enemy.TryGetComponent<Enemy>(out enemyInfo)) {
                battleExp += enemyInfo.GetExpValue();
                Debug.Log(battleExp);
            }
        }
    }

    public int GetBattleExp()
    {
        return battleExp;
    }
}
