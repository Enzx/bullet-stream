using System.Collections.Generic;
using BulletSteam.Prototype.Arsenal.Projectiles.Motions;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    [CreateAssetMenu(menuName = "BulletStream/Arsenal/Projectile Template")]
    public class ProjectileTemplate : ScriptableObject
    {
        [Header("Ballistics")] public float Speed = 40f;
        public Vector2 Gravity = new Vector2(0, -9.81f);
        public float LifeTime = 1.5f;
        public int PierceLeft = 0;
        

        public CollisionMode Collision = CollisionMode.SweptRay;
        public LayerMask HitMask = ~0;

        [Header("Prefab & FX")] public GameObject GfxPrefab;

        [Header("Motion Controllers")] public List<MotionTemplate> MotionNodes = new();

        [Header("Impact Responders")] public List<ImpactFeatureTemplate> ImpactNodes = new();
        
    }
}