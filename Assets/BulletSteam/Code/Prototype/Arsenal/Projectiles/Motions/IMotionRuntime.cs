using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles.Motions
{
    public interface IMotionRuntime
    {
        void Update(ref Vector2 position, ref Vector2 velocity, float dt, ref MotionScratch scratch);
    }
}