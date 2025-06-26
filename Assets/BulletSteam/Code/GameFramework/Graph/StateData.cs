using System.Collections.Generic;
using BulletSteam.GameFramework.DataModel;
using UnityEngine;

namespace BulletSteam.GameFramework.Graph
{
    public class StateData : NodeData
    {
        [SerializeReference] public List<ActionTask> Actions;

        public override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<State>(this);
        }
    }
}