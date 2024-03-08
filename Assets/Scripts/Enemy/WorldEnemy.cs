using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Script that controls Enemy Behaviour in World
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class WorldEnemy : MonoBehaviour
{
    [Header("References")]
    private NavMeshAgent _navMeshAgent;
    [SerializeField] private GameObject player;
    private Animator _animator;

    [Header("World Atrributes")]
    private Vector3 startPosition;
    [SerializeField] int roamRadius;
    [SerializeField] private float enemySpeed;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float visionRange;
    private bool inVisionRange = false; //Check if the player is in Vision range
    [SerializeField] private int baseDamage = 15;
    [SerializeField] private float enemyOffsetY; 

    [Header("Attack")]
    [SerializeField] protected float attackCooldownTimer = 2f;
    [SerializeField] protected float attackRange;
    protected bool inAttackRange;
    protected bool canAttack;



    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        startPosition = transform.position;
        canAttack = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        inAttackRange = Physics.Raycast(transform.position + Vector3.up*enemyOffsetY ,transform.forward,attackRange,playerLayerMask);
        inVisionRange = Physics.CheckSphere(transform.position,visionRange,playerLayerMask);
        Debug.Log($"InAttackRAnge: {inAttackRange} inVisionRange: {inVisionRange} canAttack {canAttack}");
        if (inAttackRange && canAttack)
        {
            Attack();
            Debug.Log("In attack Range");
        }
        else if(inVisionRange){
            
            Follow();
        }
        else
        {
            Patrol();
        }
    }

    private void LateUpdate()
    { 
        _animator.SetBool("Walk",_navMeshAgent.hasPath);
    }

    private void Patrol() {
        
        if (!_navMeshAgent.pathPending && !_navMeshAgent.hasPath) //If agent doesnt have a destination provided
        {
            FreeRoam(); //Get new destination
        }
        
    }

    private void Follow() {
        _navMeshAgent.SetDestination(player.transform.position);
    }

    private void Attack() {
        StartCoroutine("AttackCooldown");
        _animator.SetTrigger("Attack");
        Debug.Log("attack");

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            //player.GetComponent<PlayerLife>().TakeDamage(baseDamage);
            PostProcesingManager.instance.VignetteOn();
        }
    }

    private void FreeRoam()
    {
        //Get a random position near enemy startPosition
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += startPosition;

        //Get new destination in Navmesh area
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector3 finalPosition = hit.position;

        //Check if the path is posible
        if (!_navMeshAgent.SetDestination(finalPosition))
        {
            _navMeshAgent.SetDestination(startPosition); //If not, return to initial position
        }
    }

    private IEnumerator AttackCooldown() {

        canAttack = false;
        yield return new WaitForSeconds(attackCooldownTimer);
        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPosition, roamRadius);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, visionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,transform.forward*attackRange);
    }

}
