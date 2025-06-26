using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public class VfxSystem : MonoBehaviour, IVfxSystem
    {
        [SerializeField] private GameObject _vfxPrefab;

        public void SpawnVfx(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            GameObject vfx = Instantiate(prefab, position, rotation);
            Destroy(vfx, 5f); // Destroy after 5 seconds
        }
    }
}