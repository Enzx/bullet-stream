using System.Collections.Generic;
using BulletSteam.GameFramework.Messaging;
using BulletSteam.Prototype.Arsenal.Features;
using BulletSteam.Prototype.Arsenal.View;
using BulletSteam.Prototype.World;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal
{
    public enum FireMode
    {
        Single,
        Burst,
        Auto
    }
    [CreateAssetMenu(menuName = "BulletStream/Arsenal/Weapon", fileName = "WeaponAsset", order = 0)]
    public class WeaponAsset : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private FireMode _fireMode;
        [SerializeField] private List<WeaponFeatureTemplate> _features = new();
        [SerializeField] public WeaponView WeaponViewPrefab;
        public FireMode FireMode => _fireMode;


        public WeaponRuntime CreateRuntime(IGameplayWorld gameplayWorld, WeaponView view)
        {
            Events events = new();
            WeaponContext context = new(
                gameplayWorld,
                events,
                view
            );
            List<IWeaponFeatureRuntime> featureInstances = new(_features.Count);
            foreach (IWeaponFeatureTemplate feature in _features)
            {
                featureInstances.Add(feature.CreateRuntime(context));
            }

            return new WeaponRuntime(context, this, featureInstances);
        }
    }
}