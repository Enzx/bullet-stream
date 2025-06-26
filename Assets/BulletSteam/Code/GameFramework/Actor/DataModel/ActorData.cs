using System.Collections.Generic;
using BulletSteam.GameFramework.DataModel;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace BulletSteam.GameFramework.Actor.DataModel
{
    [CreateAssetMenu(fileName = "ActorData", menuName = "GameFramework/ActorData/ActorData")]
    public class ActorData : ObjectData<Actor>
    {
        public AssetReference ViewReference;
        [SerializeReference] public List<ObjectData> Behaviours;
    }
}