namespace BulletSteam.Prototype.Arsenal.Features
{
    public class WeaponFeatureRuntime : IWeaponFeatureRuntime
    {
        public WeaponContext Context { get; }

        public virtual bool NeedsUpdate => false;

        protected WeaponFeatureRuntime(in WeaponContext context)
        {
            Context = context;
        }

        public virtual void Update(float deltaTime)
        {
        }

        public virtual void Dispose()
        {
        }
    }
}