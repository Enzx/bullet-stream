using UnityEngine;

namespace BulletSteam.Player
{
    public class ProjectileRuntime
    {
        private readonly ProjectileData _data;
        private readonly ProjectileView _view;

        public ProjectileRuntime(ProjectileData data, ProjectileView view)
        {
            _data = data;
            _view = view;
        }

        public void Spawn()
        {
            Object.Instantiate(_view);
        }
    }
}