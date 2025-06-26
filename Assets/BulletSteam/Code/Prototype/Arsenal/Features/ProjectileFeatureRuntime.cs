using System;
using BulletSteam.Prototype.Arsenal.WeaponEvents;

namespace BulletSteam.Prototype.Arsenal.Features
{
    public class ProjectileFeatureRuntime : WeaponFeatureRuntime
    {
        private readonly WeaponContext _context;
        private readonly ProjectileFeatureTemplate _template;

        private readonly IDisposable _shotSpawnSubscription;
        public override bool NeedsUpdate => false;

        public ProjectileFeatureRuntime(in WeaponContext context, ProjectileFeatureTemplate template) : base(context)
        {
            _context = context;
            _template = template;

            _shotSpawnSubscription = _context.Events.Subscribe<RequestShotSpawnEvent>(OnRequestShotSpawn);
        }

        private void OnRequestShotSpawn(RequestShotSpawnEvent request)
        {
            _context.GameplayWorld.ProjectileSpawner.Spawn(
                _template.ProjectileTemplate,
                _template.ProjectileView,
                request.Origin,
                request.Direction,
                request.Shooter);
        }


        public override void Dispose()
        {
            _shotSpawnSubscription.Dispose();
        }
    }
}