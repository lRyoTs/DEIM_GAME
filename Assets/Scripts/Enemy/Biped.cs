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
        enemyLife = GetComponent<Life>();
        enemyLife.InitializeLife(level);
    }

    private void Start()
    {
            
    }

    protected override void Patrol()
    {
        transform.Translate(transform.forward * 4 * Time.deltaTime);
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }


}
