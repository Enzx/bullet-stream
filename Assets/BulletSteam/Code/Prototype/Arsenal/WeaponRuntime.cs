using System;
using System.Collections.Generic;
using BulletSteam.GameFramework;
using BulletSteam.Prototype.Arsenal.Features;

namespace BulletSteam.Prototype.Arsenal
{
    public class WeaponRuntime : IDisposable, IUpdate
    {
        private readonly WeaponContext _context;
        private readonly WeaponAsset _asset;
        private readonly List<IWeaponFeatureRuntime> _features;
        
        public FireMode FireMode => _asset.FireMode;

        public WeaponRuntime(in WeaponContext context, WeaponAsset asset, List<IWeaponFeatureRuntime> features)
        {
            _context = context;
            _asset = asset;
            _features = features;
        }
        
        public void PublishEvent<T>(T evt) where T : struct
        {
            _context.Events.Publish(evt);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void Dispose()
        {
            foreach (IWeaponFeatureRuntime feature in _features)
            {
                if (feature is IDisposable disposableFeature)
                {
                    disposableFeature.Dispose();
                }
            }
        }

        public void Update(float deltaTime)
        {
            foreach (IWeaponFeatureRuntime feature in _features)
            {
                if (feature.NeedsUpdate)
                {
                    feature.Update(deltaTime);
                }
            }
        }
    }
}