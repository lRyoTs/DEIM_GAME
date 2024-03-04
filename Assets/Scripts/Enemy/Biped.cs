using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class Biped : Enemy
{
    
    protected override void Patrol()
    {
        rb.MovePosition(transform.position + transform.forward * enemySpeed  * Time.deltaTime);
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }


}
