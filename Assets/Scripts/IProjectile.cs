using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    GameObject ProjectileSpawnPosition { get; }
    GameObject BulletPrefab { get; }
    void Shoot() { }
}
