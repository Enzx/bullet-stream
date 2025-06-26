namespace BulletSteam.Prototype.Arsenal.Projectiles
{
    public interface IImpactResponder
    {
        void OnImpact(in ImpactData impactData);
    }
}