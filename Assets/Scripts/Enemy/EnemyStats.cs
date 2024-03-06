using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    #region Variables
    [Header("References")]
    protected Life enemyLife;
    protected Rigidbody rb;

    [Header("Enemy Base Stats")]
    [SerializeField] protected int base_Exp;
    [SerializeField] protected int base_Dmg;

    [Header("Enemy Info")]
    protected string enemyName;
    [SerializeField] protected int level = 1;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float actionTimer;
    #endregion


}
