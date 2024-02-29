using System.Collections;
using UnityEngine;

/// <summary>
/// Script the serves as a base class for future enemies implementation
/// </summary>
[RequireComponent(typeof(Rigidbody),typeof(Life))]
public abstract class Enemy : MonoBehaviour
{
    #region Variables
    [Header("References") ]
    protected Life enemyLife;
    protected Rigidbody rb;

    [Header("Enemy Info")]
    protected string enemyName;
    protected int expValue; //Experience points given to the player once the enemy is destroyed
    protected int level;

    [Header("Attack")]
    [SerializeField] protected float attackCooldownTimer;
    [SerializeField] protected float attackRange;
    protected bool playerInAttackRange;
    protected bool canAttack;
    protected float actionTimer;
    #endregion

    private void Start()
    {
        enemyLife = GetComponent<Life>(); //Setting enemy life requires level > 0
        rb = GetComponent<Rigidbody>();
        enemyLife.InitializeLife(level);
    }

    protected void ManageDeath() {
        //Death animation create animator
        gameObject.SetActive(false);
    }


    private void FixedUpdate()
    {
        if (!enemyLife.IsDead())
        {
            if (!playerInAttackRange || !canAttack)
            {
                Patrol();
            }
            else {
                Attack();
            }
        }
    }


    protected void OnDisable() {
        //Store expvalue in a variable to call once the battle is done
        Debug.Log("Store enemyvalue in dataPersistence");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Collided");
            //Get projectile damage
            int damage = 105; //Testing value
            enemyLife.TakeDamage(damage);
            if (enemyLife.IsDead()) {
                Debug.Log("Enemy died");
                ManageDeath();
            }
        }
    }

    protected abstract void Patrol();
    protected abstract void Attack();

    public int GetExpValue() {
        return expValue;
    }
}
