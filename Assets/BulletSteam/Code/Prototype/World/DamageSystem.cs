using UnityEngine;

namespace BulletSteam.Prototype.World
{
    public class DamageSystem : MonoBehaviour, IDamageSystem
    {
        public bool Apply(GameObject target, in DamageInfo damage)
        {
            if (!target.TryGetComponent(out HealthAttrib health)) return false;
            health.ApplyDamage(damage);
            return true;

        }
    }

    public class HealthAttrib : MonoBehaviour
    {
        [SerializeField] private float _maxAmount;
        [SerializeField] private GameObject _damageVfx;
        [SerializeField] private GameObject _deathVfx;
        
        private float _amount;
        public float Health => _amount;
        
        private void Awake()
        {
            _amount = _maxAmount;
        }
        
        
        public void ApplyDamage(DamageInfo damage)
        {
            if (damage.Amount <= 0) return;
            _amount -= damage.Amount;
            if (_damageVfx) _damageVfx.SetActive(true);
            if (_amount <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (_deathVfx) _deathVfx.SetActive(true);
            Destroy(gameObject);
        }
    }
  
}