using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scripts to make sure that World UI look at player
/// </summary>
public class WorldUI : MonoBehaviour
{
    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(player.position.x,0,player.position.z), Vector3.up);
    }
}
