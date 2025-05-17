using UnityEngine;

namespace BulletSteam.Player
{
    public class WeaponDriver : MonoBehaviour
    {
        [SerializeField] private WeaponData _weaponData;
        [SerializeField] private WeaponView _weaponView;

        private WeaponRuntime _runtime;
        private void Awake()
        {
            _runtime = new WeaponRuntime(_weaponData, _weaponView);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _runtime.Fire(Vector2.down);
            }
        }
    }
}