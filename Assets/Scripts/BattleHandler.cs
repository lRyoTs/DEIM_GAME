using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class BattleHandler
{
    public static Vector3 PlayerWorldPosition { get; set; }
    private static List<GameObject> enemyList = new List<GameObject>();
    public static int BattleExp { get; private set;}

    public static void SetEnemyList(List<GameObject> enemyList)
    {
        BattleHandler.enemyList.Clear();
        BattleExp = 0;
        foreach (GameObject enemy in enemyList)
        {
            if (enemy.TryGetComponent<Enemy>(out Enemy enemyInfo)) {
                enemyInfo.InitializeEnemy(1);
                BattleExp += enemyInfo.GetExpValue();
                BattleHandler.enemyList.Add(enemy);
            }
            
        }

        Debug.Log($" Total EXP:{BattleExp}, List Count {BattleHandler.enemyList.Count}");
        Debug.Log(BattleHandler.enemyList[0].GetComponent<Enemy>().GetExpValue());
    }

}
