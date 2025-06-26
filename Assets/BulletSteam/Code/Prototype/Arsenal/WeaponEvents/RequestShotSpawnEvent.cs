using System;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.WeaponEvents
{
    public struct RequestShotSpawnEvent
    {
        public Guid Shooter;
        public Vector2 Origin;
        public Vector2 Direction;

        public RequestShotSpawnEvent(Guid shooter, Vector2 origin, Vector2 direction)
        {
            Shooter = shooter;
            Origin = origin;
            Direction = direction;
        }
    }
}