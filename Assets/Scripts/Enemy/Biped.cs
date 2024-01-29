using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biped : Enemy
{
    private void Awake()
    {
        name = gameObject.name;
        actionTimer = 2.5f;
        level = 2;
        enemyLifeInfo = GetComponent<Life>();
        enemyLifeInfo.InitializeLife(level);
    }

    private void Update()
    {
        transform.Translate(transform.forward * 4 * Time.deltaTime);
    }
}
