using BulletSteam.GameFramework.Messaging;
using BulletSteam.Prototype.Arsenal.View;
using BulletSteam.Prototype.World;

namespace BulletSteam.Prototype.Arsenal
{
    public readonly struct WeaponContext
    {
        public readonly Events Events;
        public readonly WeaponView View;
        public readonly IGameplayWorld GameplayWorld;

        public WeaponContext(IGameplayWorld gameplayWorld, Events events, WeaponView view)
        {
            GameplayWorld = gameplayWorld;
            Events = events;
            View = view;
        }
    }
}