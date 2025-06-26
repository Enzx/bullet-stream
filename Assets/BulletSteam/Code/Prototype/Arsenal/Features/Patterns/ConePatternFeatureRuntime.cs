using BulletSteam.Prototype.Arsenal.WeaponEvents;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Features.Patterns
{
    public class ConePatternFeatureRuntime : WeaponFeatureRuntime
    {
        private readonly ConePatternFeatureTemplate _template;
        public override bool NeedsUpdate => false;

        private float _nextTime;

        public ConePatternFeatureRuntime(ConePatternFeatureTemplate template, in WeaponContext context) : base(context)
        {
            _template = template;
            context.Events.Subscribe<FireRequestEvent>(OnFireRequest);
        }

        private void OnFireRequest(FireRequestEvent req)
        {
            if (Time.time < _nextTime) return;
            _nextTime = Time.time + _template.Cooldown;

            float half = _template.ConeAngle * 0.5f;
            for (int index = 0; index < _template.Pellets; index++)
            {
                float interpolate = (_template.Pellets == 1) ? 0.5f : (float)index / (_template.Pellets - 1);
                float ang = Mathf.Lerp(-half, half, interpolate);
                Vector2 dir = Quaternion.Euler(0, 0, ang) * req.Aim.normalized;
                Context.Events.Publish(new RequestShotSpawnEvent
                {
                    Origin = req.Position,
                    Direction = dir
                });
            }
        }
    }
}