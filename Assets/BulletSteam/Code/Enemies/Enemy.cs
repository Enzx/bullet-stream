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
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rigidbody.linearVelocity = _direction * _speed;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 1f, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                if (player is null) return;
                player.TakeDamage(_damage * Time.deltaTime);
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