using BulletSteam.Prototype.World;

namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    public class DirectDamageTemplate : ImpactFeatureTemplate
    {
        public float Amount;
        public DamageType Type;


        public override IImpactResponder CreateResponder(IGameplayWorld world)
        {
            return new DirectDamage(this, world);
        }
    }
}