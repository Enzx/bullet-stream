using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles.Motions
{
    [CreateAssetMenu(menuName = "BulletStream/Arsenal/Motions/Wave", fileName = "WaveMotionTemplate", order = 0)]
    public class WaveMotionTemplate : MotionTemplate
    {
        public float Amplitude = 0.5f;
        public float Frequency = 10f;
        
        public override IMotionRuntime CreateRuntime()
        {
            return new WaveMotionRuntime(this);
        }
    }
}