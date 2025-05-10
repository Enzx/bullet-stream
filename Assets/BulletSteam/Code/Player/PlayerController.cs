using System;
using BulletSteam.Enemies;
using BulletSteam.Gameplay;
using UnityEngine;

namespace BulletSteam.Player
{
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
            _elapsed += Time.deltaTime;
            if (_elapsed >= _fireRate)
            {
                _elapsed = 0;
                float minDistance = float.MaxValue;
                Enemy nearestEnemy = null;

                foreach (Enemy enemy in _gameplayWorld.Enemies)
                {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestEnemy = enemy;
                    }
                }

                if (nearestEnemy != null)
                {
                    Vector3 targetPosition = nearestEnemy.transform.position;

                    ProjectileController projectile =
                        Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
                    Vector3 direction = targetPosition - transform.position;
                    projectile.SetVelocity(direction);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _enemyLayerMask);

                foreach (Collider2D col in colliders)
                {
                    Enemy enemy = col.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                        rb.AddForce(
                            (col.transform.position - transform.position).normalized * _blastRadius,
                            ForceMode2D.Impulse);
                    }
                }
            }
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