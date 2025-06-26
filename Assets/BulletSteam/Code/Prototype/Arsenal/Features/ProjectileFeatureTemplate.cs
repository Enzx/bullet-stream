using BulletSteam.Prototype.Arsenal.Projectiles;
using BulletSteam.Prototype.Arsenal.View;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Features
{
    [CreateAssetMenu(menuName = "BulletStream/Arsenal/Projectile Feature", fileName = "ProjectileFeatureTemplate", order = 0)]
    public class ProjectileFeatureTemplate : WeaponFeatureTemplate
    {
        public ProjectileView ProjectileView;
        public ProjectileTemplate ProjectileTemplate;


        public override IWeaponFeatureRuntime CreateRuntime(in WeaponContext context)
        {
            return new ProjectileFeatureRuntime(context, this);
        }
    }
}