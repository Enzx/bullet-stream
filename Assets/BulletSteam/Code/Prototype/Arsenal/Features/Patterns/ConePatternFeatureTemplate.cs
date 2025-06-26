using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Features.Patterns
{
    [CreateAssetMenu(menuName = "BulletStream/Arsenal/Create ConePattern", fileName = "ConePatternFeature", order = 0)]
    public class ConePatternFeatureTemplate : WeaponFeatureTemplate
    {
        public int Pellets = 8;
        public float ConeAngle = 15f;
        public float Cooldown = 0.25f;

        public override IWeaponFeatureRuntime CreateRuntime(in WeaponContext context)
        {
            return new ConePatternFeatureRuntime(this, context);
        }
    }
}