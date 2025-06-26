using System;
using BulletSteam.GameFramework.DataModel;
using UnityEngine;

namespace BulletSteam.GameFramework.Graph
{
    public class NodeData : ObjectData
    {
        [HideInInspector] public NodeId Key;
#if UNITY_EDITOR
        [HideInInspector] [SerializeReference] public object EditorData;
#endif

        private void Awake()
        {
            Key = new NodeId { Id = SerializableGuid.NewGuid() };
        }


        public override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<Node>(this);
        }

        public static NodeData Create<TData>() where TData : NodeData
        {
            return Create(typeof(TData));
        }

        public static NodeData Create(Type type)
        {
            NodeData data = (NodeData)CreateInstance(type);
            data.Key = new NodeId { Id = SerializableGuid.NewGuid() };
            return data;
        }
    }
}