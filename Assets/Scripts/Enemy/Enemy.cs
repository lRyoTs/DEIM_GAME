using System.Collections;
using UnityEngine;

/// <summary>
/// Script the serves as a base class for future enemies implementation
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    #region Variables
    protected string enemyName;
    protected int level;
    protected Life enemyLife;
    protected int expValue; //Experience points given to the player once the enemy is destroyed

    protected float attackCooldownTimer;
    [SerializeField] protected float attackRange;
    protected bool playerInAttackRange;
    protected float actionTimer;
    #endregion

    private void Start()
    {
        actionTimer = 2.5f;
        level = 2;
        enemyLife = GetComponent<Life>();
        enemyLife.InitializeLife(level);
    }

    protected void ManageDeath() {
        //Death animation create animator
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!enemyLife.IsDead())
        {
            if (!playerInAttackRange)
            {
                Patrol();
            }
            else
            {
                Attack();
            }
        }
        else
        {
            Debug.Log("Enemy died");
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
            int damage = -105; //Testing value
            enemyLife.UpdateLife(damage);
            if (enemyLife.IsDead()) {
                Debug.Log("Enemy died");
                ManageDeath();
            }
        }
    }

    protected abstract void Patrol();
    protected abstract void Attack();
}
