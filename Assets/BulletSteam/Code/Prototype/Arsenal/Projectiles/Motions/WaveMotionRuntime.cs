using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles.Motions
{
    public class WaveMotionRuntime : IMotionRuntime
    {
        private readonly float _amplitude;
        private readonly float _frequency;
        
        public WaveMotionRuntime(WaveMotionTemplate template)
        {
            _amplitude = template.Amplitude;
            _frequency = template.Frequency;
        }

        public void Update(ref Vector2 position, ref Vector2 velocity, float dt, ref MotionScratch scratch)
        {
            float travelStep = velocity.magnitude * dt;
            scratch.Distance += travelStep;
            Vector2 forward = velocity.normalized;
            Vector2 per = Vector2.Perpendicular(forward).normalized;
            float waveOffset = Mathf.Sin(scratch.Distance * _frequency * Mathf.PI) * _amplitude;
            position += per * (waveOffset * dt);
        }
    }
}