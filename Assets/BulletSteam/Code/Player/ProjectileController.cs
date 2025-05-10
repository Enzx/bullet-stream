using System;
using BulletSteam.Enemies;
using UnityEngine;

namespace BulletSteam.Player
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifetime = 5f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private LayerMask _targetMask;
        public float Damage => _damage;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 direction)
        {
            _direction = direction;
            Destroy(gameObject, _lifetime);
        }

        private void Update()
        {
            Vector2 velocity = _direction * _speed;
            _rigidbody.linearVelocity = velocity;
          RaycastHit2D hit =  Physics2D.Raycast(transform.position, _direction, velocity.magnitude,  _targetMask);
          
          if (hit.collider is not null)
          {
              Enemy enemy = hit.collider.GetComponent<Enemy>();
              if (enemy is not null)
              {
                  enemy.TakeDamage(_damage);
              }
              Destroy(gameObject);
          }

        }
    }
}