namespace BulletSteam.Prototype.Arsenal.Features
{
    public interface IWeaponFeatureTemplate
    {
        IWeaponFeatureRuntime CreateRuntime(in WeaponContext context);
    }
}