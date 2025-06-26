using System;
using System.Collections.Generic;
using BulletSteam.GameFramework.Logging;
using BulletSteam.Prototype.Arsenal.Projectiles;
using BulletSteam.Prototype.Arsenal.View;
using UnityEngine;
using UnityEngine.Pool;

namespace BulletSteam.Prototype.World
{
    public class ProjectileSpawner : MonoBehaviour, IProjectileSpawner
    {
        private Dictionary<ProjectileTemplate, ObjectPool<ProjectileRuntime>> _pool;
        private readonly Dictionary<ProjectileView, IDisposable> _subscriptions = new(128);
        private IGameplayWorld _world;

        private readonly List<ProjectileRuntime> _activeProjectiles = new(128);


        private void Awake()
        {
            _pool = new Dictionary<ProjectileTemplate, ObjectPool<ProjectileRuntime>>();
        }


        public void Spawn(ProjectileTemplate tpl, ProjectileView viewPrefab, Vector2 origin, Vector2 dir, Guid shooter)
        {
            if (!_pool.TryGetValue(tpl, out ObjectPool<ProjectileRuntime> pool))
            {
                pool = new ObjectPool<ProjectileRuntime>(CreateFunc
                    , OnGet, OnRelease, OnProjectileDestroy);
                _pool.Add(tpl, pool);
            }
            
            ProjectileRuntime projectile = pool.Get();
            projectile.Initialize(shooter, origin, dir);
            projectile.OnDeactivated +=  OnProjectileDeactivated;
            
            _activeProjectiles.Add(projectile);
            
            return;

            ProjectileRuntime CreateFunc()
            {
                ProjectileView view = Instantiate(viewPrefab);
                view.gameObject.SetActive(false);
                view.transform.position = origin;
                view.transform.rotation = Quaternion.identity;
                view.SetVelocity(dir * tpl.Speed);
                ProjectileRuntime runtime = new(tpl, view, _world);
                runtime.Initialize(shooter, origin, dir);
                return runtime;
            }
        }

        private void OnProjectileDeactivated(ProjectileRuntime projectile)
        {
            projectile.OnDeactivated -= OnProjectileDeactivated;
            _activeProjectiles.Remove(projectile);
            
            // Recycle the projectile
            if (_pool.TryGetValue(projectile.Template, out ObjectPool<ProjectileRuntime> pool))
            {
                pool.Release(projectile);
            }
            else
            {
                Log.Error($"Pool not found for {projectile.Template}");
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            for (int i = 0; i < _activeProjectiles.Count; i++)
            {
                _activeProjectiles[i].Update(deltaTime);
            }
        }


        private static void OnProjectileDestroy(ProjectileRuntime projectile)
        {
            Destroy(projectile.View.gameObject);
            projectile.Dispose();
        }

        private static void OnRelease(ProjectileRuntime projectile)
        {
            projectile.View.gameObject.SetActive(false);
        }

        private static void OnGet(ProjectileRuntime projectile)
        {
            projectile.View.gameObject.SetActive(true);
        }
    }
}