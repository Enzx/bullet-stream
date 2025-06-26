using System;
using UnityEngine;

namespace BulletSteam.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        [SerializeField] private EnemyTemplate _data;

        private float _currentHealth;
        public delegate void Callback(Enemy enemy);

        public Callback OnDied;
        private Transform _target;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _currentHealth = _data.InitialHealth;
        }


        private void Update()
        {
            _direction = _target.position - transform.position;
            Vector2 velocity = _direction * _data.Speed;
            _rigidbody.linearVelocity = velocity;
            RaycastHit2D hit =
                Physics2D.Raycast(transform.position, _direction, _data.AttackRange, LayerMask.GetMask("Player"));
            Debug.DrawLine(transform.position, transform.position + (Vector3)_direction * _data.AttackRange, Color.red);
            
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                Die();
            }
        }


        public void Move(Transform target)
        {
            _target = target;
        }

        private void Die()
        {
            OnDied(this);
            Destroy(gameObject);
        }
    }
}