using System;
using System.Collections.Generic;
using BulletSteam.Prototype.Arsenal.Projectiles.Motions;
using BulletSteam.Prototype.Arsenal.View;
using BulletSteam.Prototype.World;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    public class ProjectileRuntime : IDisposable
    {
        private ProjectileTemplate _template;
        private readonly List<IImpactResponder> _impactResponders;
        private readonly List<IMotionRuntime> _motionControllers;
        private Vector2 _position;
        private Vector2 _velocity;
        private Vector2 _direction;
        private float _lifeTime;
        private int _pierceLeft;
        private MotionScratch _scratch;
        private CollisionMode _collisionMode;
        private bool _isActive;

        public Action<ProjectileRuntime> OnDeactivated;
        public Guid Shooter { get; private set; }
        public ProjectileView View { get; private set; }

        public ProjectileTemplate Template => _template;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                View.gameObject.SetActive(_isActive);
                if (!_isActive)
                {
                    OnDeactivated?.Invoke(this);
                }
            }
        }

        public ProjectileRuntime(ProjectileTemplate template, ProjectileView view, IGameplayWorld world)
        {
            _template = template;
            View = view;
            _impactResponders = new List<IImpactResponder>();
            _motionControllers = new List<IMotionRuntime>();
            for (int index = 0; index < template.MotionNodes.Count; index++)
            {
                MotionTemplate motion = template.MotionNodes[index];
                _motionControllers.Add(motion.CreateRuntime());
            }

            foreach (ImpactFeatureTemplate node in template.ImpactNodes)
            {
                _impactResponders.Add(node.CreateResponder(world));
            }
        }

        public void Initialize(Guid shooter, Vector2 position, Vector2 direction)
        {
            Shooter = shooter;
            _position = position;
            _direction = direction;
            _lifeTime = _template.LifeTime;
            _pierceLeft = _template.PierceLeft;
            _collisionMode = _template.Collision;
            View.transform.position = position;
            View.SetVelocity(Vector2.zero);
            _scratch = new MotionScratch
            {
                Distance = 0,
            };

            IsActive = true;
        }

        public void Update(float deltaTime)
        {
            if (!IsActive) return;
            _lifeTime -= deltaTime;
            if (_lifeTime <= 0)
            {
                IsActive = false;
                return;
            }

            _velocity = _direction * _template.Speed;
            _velocity += _template.Gravity;
            _position += _velocity * deltaTime;
            for (int index = 0; index < _motionControllers.Count; index++)
            {
                IMotionRuntime runtime = _motionControllers[index];
                runtime.Update(ref _position, ref _velocity, deltaTime, ref _scratch);
            }

            View.SetPosition(_position);

            switch (_collisionMode)
            {
                case CollisionMode.ImmediateRay:
                    View.transform.position = _position;
                    RaycastHit2D hit = Physics2D.Raycast(_position, _velocity.normalized,
                        _velocity.magnitude * deltaTime, _template.HitMask);
                    HandleHit(hit);

                    break;
                case CollisionMode.SweptRay:

                    Vector2 start = _position;
                    Vector2 end = _position + _velocity * deltaTime;
                    RaycastHit2D sweptHit = Physics2D.Linecast(start, end, _template.HitMask);
                    HandleHit(sweptHit);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleHit(RaycastHit2D hit)
        {
            if (!hit || hit.collider is null) return;
            ImpactData data = new()
            {
                Projectile = this,
                Point = hit.point,
                Normal = hit.normal,
                Target = hit.collider.gameObject,
            };
            for (int index = 0; index < _impactResponders.Count; index++)
            {
                IImpactResponder responder = _impactResponders[index];
                responder.OnImpact(data);
            }

            _pierceLeft--;
            if (_pierceLeft > 0) return;
            IsActive = false;
        }

        public void Dispose()
        {
            View = null;
            _template = null;
            _impactResponders.Clear();
            _motionControllers.Clear();
        }
        
        
    }
}