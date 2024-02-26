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
    [SerializeField] private float enemySpeed;
    [SerializeField] private float visionRange;
    private bool inVisionRange = false; //Check if the player is in Vision range

    [Header("Battle Enemy Info")]
    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();


    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inVisionRange)
        {
            Patrol();
        }
        else {
            Follow();
        }
    }

    private void Patrol() {
        
    }

    private void Follow() {
        m_Agent.destination = player.transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) {
            //Store

            gameObject.SetActive(false);
            //Instantiate Battle Scene
        }
    }


}
