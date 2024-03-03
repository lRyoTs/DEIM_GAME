using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class Biped : Enemy
{
    #region Constants
    private const int Base_Exp = 100;
    private const int Base_Hp = 100;
    private const int Base_Damage = 5;
    #endregion
    
    private void Awake()
    {
        name = gameObject.name;
        actionTimer = 2.5f;
        level = 2;
        expValue = level * Base_Exp;
        Debug.Log(expValue);
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
