using System;
using BulletSteam.Prototype.Arsenal.View;
using BulletSteam.Prototype.Arsenal.WeaponEvents;
using BulletSteam.Prototype.World;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal
{
    public class WeaponDriver : MonoBehaviour
    {
        private WeaponView _weaponView;
        private IGameplayWorld _gameplayWorld;
        private WeaponRuntime _weaponRuntime;
        private Direction _facing;
        private Guid _shooter;

        public void SetWeapon(WeaponAsset weaponAsset)
        {
            Debug.Assert(ReferenceEquals(weaponAsset, null) == false,
                "WeaponAsset is already set. Please use SetWeapon() only once.");
            Debug.Assert(ReferenceEquals(_gameplayWorld, null) == false,
                "GameplayWorld is not set. Please ensure that the GameplayWorld is initialized before setting the weapon.");
            _weaponView = Instantiate(weaponAsset.WeaponViewPrefab, transform);
            _weaponRuntime = weaponAsset.CreateRuntime(_gameplayWorld, _weaponView);
        }

        public void Initialize(Guid shooter, IGameplayWorld gameplayWorld)
        {
            _shooter = shooter;
            if (_gameplayWorld is not null)
                Debug.LogWarning("WeaponDriver: Initialize() called. This method should only be called once.");
            _gameplayWorld = gameplayWorld;
        }

        public void SetFacing(Direction facing)
        {
            if (_facing == facing)
                return;
            _facing = facing;
            _weaponView.SetFacing(facing);
            transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, 0);
        }

        private void Update()
        {
            _weaponRuntime.Update(Time.deltaTime);
        }

        private void LateUpdate()
        {
            bool isFiring = _weaponRuntime.FireMode switch
            {
                FireMode.Single => UnityEngine.Input.GetKeyDown(KeyCode.Mouse0),
                FireMode.Burst => UnityEngine.Input.GetKey(KeyCode.Mouse0),
                FireMode.Auto => UnityEngine.Input.GetKey(KeyCode.Mouse0),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (isFiring)
            {
                Vector2 aimDirection = _facing switch
                {
                    Direction.Right => Vector2.right,
                    Direction.Left => Vector2.left,
                    Direction.None => Vector2.zero,
                    Direction.Top => Vector2.up,
                    Direction.Bottom => Vector2.down,
                    Direction.Vertical => Vector2.up,
                    Direction.Horizontal => Vector2.right,
                    Direction.All => Vector2.one,
                    _ => throw new ArgumentOutOfRangeException()
                };
                Debug.DrawLine(transform.position, transform.position + (Vector3)aimDirection * 10, Color.cyan, 1f);
                _weaponRuntime.PublishEvent(new FireRequestEvent
                {
                    Shooter = _shooter,
                    Aim = aimDirection,
                    Position = transform.position,
                });
            }
        }
    }
}