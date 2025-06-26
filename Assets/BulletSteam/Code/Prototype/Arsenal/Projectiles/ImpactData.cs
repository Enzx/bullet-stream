using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    public struct ImpactData
    {
        public ProjectileRuntime Projectile;
        public Vector2 Point;
        public Vector2 Normal;
        public GameObject Target;
    }
}