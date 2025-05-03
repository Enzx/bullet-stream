using System;
using UnityEngine;

namespace BulletSteam.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _damage = 10f;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }

        public void Move(Vector2 direction)
        {
            _rigidbody.linearVelocity = direction.normalized * _speed;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}