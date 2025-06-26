using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Features
{
    public abstract class WeaponFeatureTemplate : ScriptableObject, IWeaponFeatureTemplate
    {
        public abstract IWeaponFeatureRuntime CreateRuntime(in WeaponContext context);
    }
}