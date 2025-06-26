using System;
using UnityEngine;

namespace BulletSteam.Prototype
{
    public class AnimationCallback : StateMachineBehaviour
    {
        public event Action<int> ExitState; 
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            ExitState?.Invoke(stateInfo.shortNameHash);
        }
    }
}