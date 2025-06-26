using System.Collections.Generic;
using UnityEngine.Scripting;

namespace BulletSteam.GameFramework.Graph.Editor
{
    [Preserve]
    public class StateNode : BaseNode
    {
        private readonly StateData _data;

        public StateNode(StateData data) : base(data)
        {
            _data = data;
            _data.Actions = new List<ActionTask>();
        }
    }
}