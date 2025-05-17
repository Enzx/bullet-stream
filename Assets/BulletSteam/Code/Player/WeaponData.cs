using UnityEngine;

namespace BulletSteam.Player
{
    [CreateAssetMenu(menuName = "Create WeaponData", fileName = "WeaponData", order = 0)]
    public class WeaponData : ScriptableObject
    {
        public float FireRate;

        public ProjectileData ProjectileData;
        public ProjectileView ProjectileView;
    }
}