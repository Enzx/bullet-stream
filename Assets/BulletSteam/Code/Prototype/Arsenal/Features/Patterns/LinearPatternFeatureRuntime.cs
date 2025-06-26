using BulletSteam.Prototype.Arsenal.WeaponEvents;
using UnityEngine;

namespace BulletSteam.Prototype.Arsenal.Features.Patterns
{
    public class LinearPatternFeatureRuntime : WeaponFeatureRuntime
    {
        private readonly LinearPatternFeatureTemplate _template;
        private float _nextTime;
        public LinearPatternFeatureRuntime(LinearPatternFeatureTemplate template, in WeaponContext context) :
            base(in context)
        {
            _template = template;
            context.Events.Subscribe<FireRequestEvent>(OnFireRequest);

        }

        private void OnFireRequest(FireRequestEvent evt)
        {
            if (Time.time < _nextTime) return;
            _nextTime = Time.time + _template.Cooldown;
            Context.Events.Publish(new RequestShotSpawnEvent()
            {
                Shooter = evt.Shooter,
                Origin = evt.Position,
                Direction = evt.Aim.normalized,
            });
           
        }
    }
}