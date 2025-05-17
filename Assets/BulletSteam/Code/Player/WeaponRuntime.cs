using System.Collections.Generic;
using UnityEngine;

namespace BulletSteam.Player
{
    public class WeaponRuntime : IWeapon //Controller
    {
        private readonly WeaponData _data;
        private readonly WeaponView _view;

        private float _lastFireTime;

        private readonly List<ProjectileRuntime> _projectiles;

        public WeaponRuntime(WeaponData data, WeaponView view)
        {
            _data = data;
            _view = view;
            _projectiles = new List<ProjectileRuntime>();
        }

        public void Fire(Vector2 direction)
        {
            if (Time.timeSinceLevelLoad - _lastFireTime >= _data.FireRate)
            {
                _view.OnFire();
                ProjectileRuntime projectileRuntime = new(_data.ProjectileData, _data.ProjectileView);
                projectileRuntime.Spawn();
                _projectiles.Add(projectileRuntime);
                _lastFireTime = Time.timeSinceLevelLoad;
            }
        }
    }
}