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
    }

    protected override void Patrol()
    {
        rb.MovePosition(transform.position + transform.forward* 4  * Time.deltaTime);
        //transform.Translate(transform.forward * 4 * Time.deltaTime);
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }


}
