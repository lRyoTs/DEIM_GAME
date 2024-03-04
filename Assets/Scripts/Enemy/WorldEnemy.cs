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
    private NavMeshAgent m_Agent;
    [SerializeField] private GameObject player;

    [Header("World Atrributes")]
    private Vector3 startPosition;
    [SerializeField] int roamRadius;
    [SerializeField] private float enemySpeed;
    [SerializeField] private LayerMask playerLayerMask;
    [SerializeField] private float visionRange;
    private bool inVisionRange = false; //Check if the player is in Vision range

    [Header("Battle Enemy Info")]
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();


    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        inVisionRange = Physics.CheckSphere(transform.position,visionRange,playerLayerMask);
        if (!inVisionRange)
        {
            Patrol();
        }
        else {
            Follow();
        }
    }

    private void Patrol() {
        
        if (!m_Agent.pathPending && !m_Agent.hasPath) //If agent doesnt have a destination provided
        {
            FreeRoam(); //Get new destination
        }
        
    }

    private void Follow() {
        m_Agent.SetDestination(player.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            //Store
            BattleHandler.PlayerWorldPosition = collision.gameObject.transform.position;
            BattleHandler.SetEnemyList(enemyList);
            BattleHandler.previousScene = Loader.GetCurrentScene();
            gameObject.SetActive(false);
            //Instantiate Battle Scene
            Loader.Load(Loader.Scene.BattleScene);
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
        if (!m_Agent.SetDestination(finalPosition))
        {
            m_Agent.SetDestination(startPosition); //If not, return to initial position
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPosition, roamRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

}
