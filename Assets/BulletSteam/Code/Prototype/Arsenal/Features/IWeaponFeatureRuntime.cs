using System;

namespace BulletSteam.Prototype.Arsenal.Features
{
    public interface IWeaponFeatureRuntime : IDisposable
    {
        bool NeedsUpdate { get; }
        void Update(float deltaTime);
    }
}