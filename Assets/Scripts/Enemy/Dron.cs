using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dron : Enemy ,IProjectile
{
    Transform IProjectile.ProjectileSpawnPosition => throw new System.NotImplementedException();

    GameObject IProjectile.BulletPrefab => throw new System.NotImplementedException();

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }

    protected override void Patrol()
    {
        rb.MovePosition(transform.position + transform.forward * enemySpeed * Time.deltaTime);
    }
}
