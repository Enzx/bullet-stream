using BulletSteam.GameFramework.CameraTools;
using BulletSteam.GameFramework.Collections;
using BulletSteam.GameFramework.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletSteam.GameFramework.Input
{
    public struct MouseMoveEvent
    {
        public Vector2 PreviousPosition;
        public Vector2 Position;
    }

    public struct MouseDownEvent
    {
        public Vector2 Position;
        public Vector3 WorldPosition;
    }

    public struct MouseUpEvent
    {
        public Vector2 ScreenPosition;
        public Vector3 WorldPosition;
    }

    public struct MouseDragEvent
    {
        public Vector2 Position;
        public Vector3 WorldPosition;
    }

    public class InputSystem : ServiceBase, IUpdate
    {
        public readonly Events Events = new();

        private Vector2 _mousePosition;
        private CameraSystem _cameraSystem;

        public void Update(float deltaTime)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            if (mousePosition != _mousePosition)
            {
                Events.Publish(new MouseMoveEvent { Position = mousePosition, PreviousPosition = _mousePosition });
                _mousePosition = mousePosition;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                MouseDownEvent evt = new()
                {
                    Position = mousePosition,
                    WorldPosition = _cameraSystem.Camera.ScreenToWorldPoint(mousePosition)
                };
                Events.Publish(evt);
            }

            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                MouseUpEvent evt = new()
                {
                    ScreenPosition = mousePosition,
                    WorldPosition = _cameraSystem.Camera.ScreenToWorldPoint(mousePosition)
                };
                Events.Publish(evt);
            }

            if (Mouse.current.leftButton.isPressed)
            {
                MouseDragEvent evt = new()
                {
                    Position = mousePosition,
                    WorldPosition = _cameraSystem.Camera.ScreenToWorldPoint(mousePosition)
                };
                Events.Publish(evt);
            }
        }

        public override void OnBuild(ServiceLocator<IService> locator)
        {
            _cameraSystem = locator.Get<CameraSystem>();
        }
    }
}