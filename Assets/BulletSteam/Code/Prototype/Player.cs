using System;
using BulletSteam.Prototype.Arsenal;
using BulletSteam.Prototype.World;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private WeaponAsset _weaponAsset;
    private WeaponDriver _weaponDriver;
    private IGameplayWorld _gameplay;

    private void Awake()
    {
        if (TryGetComponent(out _weaponDriver)  == false)
        {
            _weaponDriver = GetComponentInChildren<WeaponDriver>();
        }

        Guid guid = Guid.NewGuid();
        _gameplay = FindFirstObjectByType<GameplayWorld>();
        _weaponDriver.Initialize(guid, _gameplay);
        _weaponDriver.SetWeapon(_weaponAsset);
    }
}