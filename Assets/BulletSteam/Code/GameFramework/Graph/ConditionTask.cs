using UnityEngine;

namespace BulletSteam.GameFramework.Graph
{
    public abstract class ConditionTask : ScriptableObject
    {
        public abstract bool Check();

        public virtual void Initialize()
        {
            
        }
    }
}