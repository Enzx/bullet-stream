using BulletSteam.GameFramework.Collections;
using UnityEngine;

namespace BulletSteam.GameFramework.CameraTools
{
    public sealed class CameraSystem : ServiceBase
    {
        private float _bottomOffset = 0.0f;
        private Vector3 _center = Vector3.zero;
        private SpriteRenderer _sprite;
        public Camera Camera { get; private set; }

        public void SetCamera(Camera cam)
        {
            Camera = cam;
        }

        public void SetBottomOffset(float offset)
        {
            _bottomOffset = offset;
        }
        public void SetTargetSprite(SpriteRenderer spriteRenderer)
        {
            _sprite = spriteRenderer;
            FitCameraToSprite();
        }


        private void FitCameraToSprite()
        {
            Bounds spriteBounds = _sprite.bounds;

            float spriteHeight = spriteBounds.size.y;
            float spriteWidth = spriteBounds.size.x;
            _center = spriteBounds.center;

            FitCameraToScreen(spriteWidth, spriteHeight, _center);
        }

        public Rect GetCameraWoldRect()
        {
            float screenAspect = Screen.width / (float)Screen.height;
            float cameraHeight = Camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;

            float camMinX = Camera.transform.position.x - cameraWidth / 2f;
            float camMinY = Camera.transform.position.y - cameraHeight / 2f + _bottomOffset;
            return new Rect(camMinX, camMinY, cameraWidth, cameraHeight - _bottomOffset);
        }

        public void FitCameraToScreen(float width, float height, Vector3 worldCenter)
        {
            float screenAspect = Screen.width / (float)Screen.height;

            float targetAspect = width / height;

            if (screenAspect >= targetAspect)
            {
                Camera.orthographicSize = height / 2;
            }
            else
            {
                Camera.orthographicSize = width / 2 / screenAspect;
            }
            Camera.transform.position = new Vector3(worldCenter.x, worldCenter.y, Camera.transform.position.z);
        }
    }
}