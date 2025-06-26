using BulletSteam.GameFramework.Logging;
using BulletSteam.GameFramework.Messaging;
using BulletSteam.Prototype.Arsenal.Projectiles;
using BulletSteam.Prototype.Arsenal.WeaponEvents;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.View
{
    public class ProjectileView : MonoBehaviour
    {
        public float LifeTime { get; set; }
        public int Pierce { get; set; }
        public Events Events { get; } = new();
        public ProjectileTemplate Template { get; set; }

        private Rigidbody2D _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Log.Assert(ReferenceEquals(_rigidbody, null) == false,
                "Rigidbody2D component is missing on the ProjectileView GameObject.");
        }


        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody.linearVelocity = velocity;
        }

        private void Update()
        {
            if (!(LifeTime > 0)) return;
            LifeTime -= Time.deltaTime;
            if (!(LifeTime <= 0)) return;
            Events.Publish(new RecycleProjectileViewEvent
            {
                ProjectileView = this
            });
        }

        public void SetPosition(Vector2 position)
        {
            _rigidbody.position = position;
        }
    }
}