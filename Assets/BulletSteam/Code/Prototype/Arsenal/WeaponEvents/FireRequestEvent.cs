using System;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.WeaponEvents
{
    public struct FireRequestEvent
    {
        public Guid Shooter;
        public Vector2 Aim;
        public Vector2 Position;
    }
}