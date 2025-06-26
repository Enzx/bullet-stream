using BulletSteam.Prototype.World;

namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    public class DirectDamage : IImpactResponder
    {
        private readonly IGameplayWorld _world;
        private readonly DirectDamageTemplate _template;

        public DirectDamage(DirectDamageTemplate template, IGameplayWorld world)
        {
            _template = template;
            _world = world;
        }


        public void OnImpact(in ImpactData impactData)
        {
            DamageInfo damageInfo = new ()
            {
                Amount = _template.Amount,
                Source = impactData.Projectile.Shooter,
                Type =  _template.Type,
                
            };


            if (_world.DamageSystem.Apply(impactData.Target, damageInfo))
            {
                impactData.Projectile.IsActive = false;
            }
        }
    }
}