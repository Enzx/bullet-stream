using System;
using BulletSteam.Prototype.Arsenal.Projectiles;
using BulletSteam.Prototype.Arsenal.View;
using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public interface IProjectileSpawner
    {
        void Spawn(ProjectileTemplate tpl, ProjectileView viewPrefab, Vector2 origin, Vector2 dir, Guid shooter);
    }
    
}