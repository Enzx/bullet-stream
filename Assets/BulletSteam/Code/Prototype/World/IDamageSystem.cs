using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public interface IDamageSystem
    {
        bool Apply(GameObject target, in DamageInfo damage);
    }
}