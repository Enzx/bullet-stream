using System;
using BulletSteam.Enemies;
using BulletSteam.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletSteam.Gameplay
{
    public class GameplayWorld : MonoBehaviour
    {
       [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private float _enemySpawnInterval = 2f;
        [SerializeField] private float _enemySpawnRadius = 5f;
        
        private PlayerController _playerController;
        private float _enemySpawnTimer;


        private void Awake()
        {
            _playerController = FindAnyObjectByType<PlayerController>();
            Debug.Assert(_playerController != null, "PlayerController not found in the scene.");
            _enemySpawnTimer = _enemySpawnInterval;
        }


        private void Update()
        {
            _enemySpawnTimer -= Time.deltaTime;
            if (_enemySpawnTimer <= 0f)
            {
                SpawnEnemy();
                _enemySpawnTimer = _enemySpawnInterval;
            }
        }

        private void SpawnEnemy()
        {
            Debug.Assert(_enemyPrefab != null, "Enemy prefab not assigned.");
            Vector3 spawnPosition = Random.insideUnitCircle * _enemySpawnRadius;
            Enemy enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            Vector2 direction = (_playerController.transform.position - spawnPosition).normalized;
            enemy.Move(direction);
        }
    }
}