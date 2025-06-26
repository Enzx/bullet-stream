using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles.Motions
{
    public abstract class MotionTemplate : ScriptableObject
    {
        public abstract IMotionRuntime CreateRuntime();
    }
}