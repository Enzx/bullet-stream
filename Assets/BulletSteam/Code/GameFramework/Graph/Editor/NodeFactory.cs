using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace BulletSteam.GameFramework.Graph.Editor
{
    public class NodeFactory
    {
        private readonly Dictionary<Type, ConstructorInfo> _nodeTypeMap = new();

        public BaseNode Create(NodeData nodeData)
        {
            if (_nodeTypeMap.TryGetValue(nodeData.GetType(), out ConstructorInfo constructorInfo))
            {
                return Construct(constructorInfo, nodeData);

            }

            CacheConstructorInfo(out constructorInfo, nodeData);
           return Construct(constructorInfo, nodeData);
        }

        private void CacheConstructorInfo(out ConstructorInfo constructorInfo, NodeData nodeData)
        {
            constructorInfo = TypeCache.GetTypesDerivedFrom<BaseNode>()
                .Select(type => type.GetConstructor(new[] { nodeData.GetType() }))
                .FirstOrDefault();

            if (constructorInfo == null)
            {
                throw new Exception($"No constructor found for {nodeData.GetType()}");
            }

            _nodeTypeMap.Add(nodeData.GetType(), constructorInfo);
        }

        private static BaseNode Construct(ConstructorInfo constructorInfo, NodeData nodeData)
        {
            BaseNode node = (BaseNode)constructorInfo.Invoke(new object[] { nodeData });
            node.title = string.IsNullOrEmpty(nodeData.name) ? node.GetType().Name : nodeData.name;
            node.viewDataKey = nodeData.Key.Id.ToString();

            return node;
        }
    }
}