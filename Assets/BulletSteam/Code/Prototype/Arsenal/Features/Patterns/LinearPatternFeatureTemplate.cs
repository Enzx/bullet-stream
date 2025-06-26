using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Features.Patterns
{
    [CreateAssetMenu(menuName = "BulletStream/Arsenal/Linear Feature Pattern", fileName = "LinearPatternFeatureTemplate", order = 0)]
    public class LinearPatternFeatureTemplate : WeaponFeatureTemplate
    {
        public float Cooldown = 0.25f;

        public override IWeaponFeatureRuntime CreateRuntime(in WeaponContext context)
        {
            return new LinearPatternFeatureRuntime(this, context);
        }
    }
}