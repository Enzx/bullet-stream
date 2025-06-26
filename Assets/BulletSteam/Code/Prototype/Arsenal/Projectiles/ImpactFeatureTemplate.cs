using BulletSteam.Prototype.World;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    public abstract class ImpactFeatureTemplate : ScriptableObject
    {
        public abstract IImpactResponder CreateResponder(IGameplayWorld projectile);
    }
}