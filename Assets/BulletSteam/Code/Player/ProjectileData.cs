using UnityEngine;

namespace BulletSteam.Player
{
    [CreateAssetMenu(menuName = "Create ProjectileData", fileName = "ProjectileData", order = 0)]
    public class ProjectileData : ScriptableObject
    {
        public float Speed = 10f;
        public float Lifetime = 5f;
        public float Damage = 10f;
    }
}