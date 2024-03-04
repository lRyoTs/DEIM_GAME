using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleHandler
{
    public static Vector3 PlayerWorldPosition { get; set; }
    public static string previousScene {get; set;}

    private static List<GameObject> enemyList = new List<GameObject>();
    public static int BattleExp { get; private set;}

    /// <summary>
    /// Get enemyLList to spawn in Battle Scene and calculate its experience
    /// </summary>
    /// <param name="enemyList">enemyList provided</param>
    public static void SetEnemyList(List<GameObject> enemyList)
    {
        BattleHandler.enemyList.Clear();
        BattleExp = 0;
        foreach (GameObject enemy in enemyList)
        {
            if (enemy.TryGetComponent<Enemy>(out Enemy enemyInfo)) {
                enemyInfo.InitializeEnemy(1); //Set PlayerLevel
                BattleExp += enemyInfo.GetExpValue();
                BattleHandler.enemyList.Add(enemy);
            }
            
        }
    }

}
