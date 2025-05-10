using System;
using BulletSteam.Player;
using UnityEngine;

namespace BulletSteam.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _attackRange = 1f;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 velocity = _direction * _speed;
            _rigidbody.linearVelocity = velocity;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, _attackRange, LayerMask.GetMask("Player"));
            Debug.DrawLine(transform.position, transform.position + (Vector3)_direction * _attackRange, Color.red);
            if (hit.collider != null)
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                if (player is null) return;
                player.TakeDamage(_damage * Time.deltaTime);
                _direction = Vector2.zero;
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


        public void Move(Vector2 direction)
        {
            _direction = direction.normalized;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}