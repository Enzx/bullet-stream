using System;
using BulletSteam.Enemies;
using UnityEngine;

namespace BulletSteam.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ProjectileController _projectilePrefab;
        [SerializeField] LayerMask _enemyLayerMask;
        [SerializeField] private float _blastRadius = 5f;
        [SerializeField] private float _health = 100f;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ProjectileController projectile =
                    Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
                Vector2 direction = (_camera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                projectile.SetVelocity(direction);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _enemyLayerMask);
                
                foreach (Collider2D collider in colliders)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                        rb.AddForce(
                            (collider.transform.position - transform.position).normalized * _blastRadius,
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