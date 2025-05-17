using System;
using BulletSteam.Enemies;
using BulletSteam.Gameplay;
using UnityEngine;

namespace BulletSteam.Player
{
    //Logic


    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ProjectileController _projectilePrefab;
        [SerializeField] LayerMask _enemyLayerMask;
        [SerializeField] private float _blastRadius = 5f;
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _fireRate = 1f;

        private Camera _camera;
        private float _elapsed;
        private GameplayWorld _gameplayWorld;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Initialize(GameplayWorld gameplayWorld)
        {
            _gameplayWorld = gameplayWorld;
        }


        private void Update()
        {
//             _elapsed += Time.deltaTime;
//             if (_elapsed >= _fireRate)
//             {
//                 _elapsed = 0;
//                 float minDistance = float.MaxValue;
//                 Enemy nearestEnemy = null;
// // Targeting 
//                 foreach (Enemy enemy in _gameplayWorld.Enemies)
//                 {
//                     float distance = Vector2.Distance(transform.position, enemy.transform.position);
//                     if (distance < minDistance)
//                     {
//                         minDistance = distance;
//                         nearestEnemy = enemy;
//                     }
//                 }
//                 
// // Spawn Bullet
//                 if (nearestEnemy != null)
//                 {
//                     Vector3 targetPosition = nearestEnemy.transform.position;
//
//                     ProjectileController projectile =
//                         Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
//                     Vector3 direction = targetPosition - transform.position;
//                     projectile.SetVelocity(direction);
//                 }
//             }


         
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("Player died");
            gameObject.SetActive(false);
        }
    }
}