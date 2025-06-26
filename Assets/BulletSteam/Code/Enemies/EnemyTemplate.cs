using UnityEngine;

namespace BulletSteam.Enemies
{
    [CreateAssetMenu(menuName = "BulletStream/Create EnemyTemplate", fileName = "EnemyTemplate", order = 0)]
    public class EnemyTemplate : ScriptableObject
    {
        public float InitialHealth = 100f;
        public float Speed = 5f;
        public float Damage = 10f;
        public float AttackRange = 1f;
    }
}