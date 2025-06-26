using System;

namespace BulletSteam.Prototype.World
{
    public enum DamageType
    {
        System,
        Bullet,
        Melee,
        Explosion,
        Fire,
        Falling,
    }
    
    public enum Team
    {
        Neutral,
        Player,
        Enemy,
    }
    
    public struct DamageInfo
    {
        public float Amount;
        public Guid Source;
        public DamageType Type;
    }
}