namespace BulletSteam.Prototype.World
{
    public interface IGameplayWorld
    {
        IProjectileSpawner ProjectileSpawner { get; }
        IDamageSystem DamageSystem { get; }
        IVfxSystem VfxSystem { get; }
        IAudioSystem AudioSystem { get; }
    }
}