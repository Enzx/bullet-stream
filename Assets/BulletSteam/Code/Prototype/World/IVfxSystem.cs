using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public interface IVfxSystem
    {
        void SpawnVfx(GameObject prefab, Vector2 position, Quaternion rotation);
    }
}