using System.Collections.Generic;
using BulletSteam.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletSteam.Prototype.World
{
    public class GameplayWorld : MonoBehaviour, IGameplayWorld
    {
        public IProjectileSpawner ProjectileSpawner { get;private  set; }
        public IDamageSystem DamageSystem { get; private set; }
        public IVfxSystem VfxSystem { get; private set; }
        public IAudioSystem AudioSystem { get; private set; }

        [SerializeField] private ProjectileSpawner _projectileSpawner;
        [SerializeField] private DamageSystem _damageSystem;
        [SerializeField] private VfxSystem _vfxSystem;
        [SerializeField] private AudioSystem _audioSystem;
        [SerializeField] private Player _player;

        [SerializeField] private Enemy _enemyPrefab;

        [SerializeField] private float _enemySpawnTimer = 1f;
        [SerializeField] private float _enemySpawnRange = 8f;
        
        private List<Enemy> _enemies = new();
        private float _elapsed;
        
        private void Awake()
        {
            ProjectileSpawner = _projectileSpawner;
            DamageSystem = _damageSystem;
            VfxSystem = _vfxSystem;
            AudioSystem = _audioSystem;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;

            if (_elapsed < _enemySpawnTimer) return;
            _elapsed = 0;
            Vector3 position = Random.insideUnitCircle * _enemySpawnRange;
            Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
            enemy.Move(_player.transform);
            _enemies.Add(enemy);
        }
    }
}