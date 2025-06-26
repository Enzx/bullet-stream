using BulletSteam.GameFramework.Actor.DataModel;
using BulletSteam.GameFramework.Actor.View;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BulletSteam.GameFramework.Actor
{
    public class ActorFactory
    {
        public static void Init()
        {
            Addressables.InitializeAsync().WaitForCompletion();
        }

        public static Actor Create(ActorData data, IWorld world)
        {
            GameObject gameObject = Addressables.InstantiateAsync(data.ViewReference.RuntimeKey).WaitForCompletion();
            Debug.Assert(gameObject != null, "Prefab not found in addressable");
            ActorView view = gameObject.GetComponent<ActorView>();
            Debug.Assert(view != null, "ActorView not found on prefab");
            Actor actor = new(data, view, world);

            return actor;
        }
    }
}