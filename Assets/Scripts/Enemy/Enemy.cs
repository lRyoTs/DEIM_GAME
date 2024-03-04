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

    [Header("Enemy Base Stats")]
    [SerializeField] protected int base_Exp;
    [SerializeField] protected int base_Dmg;

    [Header("Enemy Info")]
    protected string enemyName;
    protected int level = 1;
    [SerializeField] protected float enemySpeed;
    [SerializeField] protected float actionTimer;

    [Header("Attack")]
    [SerializeField] protected float attackCooldownTimer;
    [SerializeField] protected float attackRange;
    [SerializeField] protected LayerMask playerLayerMask;
    protected bool playerInAttackRange;
    protected bool canAttack;
    #endregion

    protected virtual void Start()
    {
        enemyLife = GetComponent<Life>(); //Setting enemy life requires level > 0
        rb = GetComponent<Rigidbody>();
        enemyLife.InitializeLife(level);
    }

    protected void ManageDeath() {
        //Death animation create animator
        gameObject.SetActive(false);
    }

    protected void FixedUpdate()
    {
        if (!enemyLife.IsDead())
        {
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayerMask);
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
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))//collision.gameObject.CompareTag("Projectile"))
        {
            Debug.Log("Collided");
            //Get projectile damage
            int damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().GetAttackDmg(); //Testing value
            enemyLife.TakeDamage(damage);
            if (enemyLife.IsDead()) {
                Debug.Log("Enemy died");
                ManageDeath();
            }
        }
    }

    protected abstract void Patrol();
    protected abstract void Attack();

    public void InitializeEnemy(int baseLevel) {
        name = gameObject.name;

        if (baseLevel <= 3)
        {
            level = Random.Range(1, baseLevel+3);
        }
        else {
            level = Random.Range(baseLevel-3,baseLevel+3); //Set to calculate randomly between player level  + 3 levels above;
        }
        
    }

    public int GetExpValue() {
        return level*base_Exp;
    }

    public string GetEnemyName() {
        return enemyName;
    }
    
    public int GetLevel()
    {
        return level;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}
