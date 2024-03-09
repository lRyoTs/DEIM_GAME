using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileParticle
{
    Transform ProjectileSpawnPosition { get; }
    ParticleSystem BulletPrefab { get; }
    void Shoot() { }
}
