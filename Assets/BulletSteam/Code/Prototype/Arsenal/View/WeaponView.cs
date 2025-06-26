using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.View
{
    public enum Direction
    {
        Left,
        Right,
        Top, 
        Down,
        None,
        Bottom,
        Vertical,
        Horizontal,
        All
    }
    public class WeaponView : MonoBehaviour
    {
        private SpriteRenderer _sprite;
        
        private void Awake()
        {
            _sprite = GetComponent<SpriteRenderer>();
            if (_sprite == null)
            {
                Debug.LogError("SpriteRenderer component not found on the GameObject.");
            }
        }
        
        
        public void SetFacing(Direction direction)
        {
            _sprite.flipX = direction == Direction.Left;
        }
    }
}