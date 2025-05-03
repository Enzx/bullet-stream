using System;
using UnityEngine;

namespace BulletSteam.Player
{
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifetime = 5f;
        [SerializeField] private float _damage = 10f;

        public float Damage => _damage;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 direction)
        {
            _rigidbody.linearVelocity = direction * _speed;
            Destroy(gameObject, _lifetime);
        }
    }
}