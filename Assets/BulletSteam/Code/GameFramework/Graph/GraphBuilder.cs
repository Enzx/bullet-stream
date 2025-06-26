using System;
using BulletSteam.GameFramework.DataModel;

namespace BulletSteam.GameFramework.Graph
{
    public class GraphBuilder : IDataVisitor
    {
        private readonly GraphData _data;

        public GraphBuilder(GraphData data)
        {
            _data = data;
        }

        public Graph Build() 
        {
            Node startNode = Create<Node>(_data.StartNodeData);
            Graph graph = new(startNode);
            for (int i = 0; i < _data.NodesData.Count; i++)
            {
                Node node = Create<Node>(_data.NodesData[i]);
                graph.AddNode(node);
            }
            graph.AddTransitions(_data.Transitions);


            return graph;
        }

        private TObject Create<TObject>(IData data)
        {
            TObject obj = (TObject)Visit<TObject>(data);
            return obj;
        }

        public IObject Visit<TObject>(IData data)
        {
            Type dataType = typeof(TObject);
            IObject behaviour = (IObject)Activator.CreateInstance(dataType, data);
            return behaviour;
        }

        public IObject Visit<TObject, TInterface>(IData data)
        {
            IObject behaviour = Visit<TObject>(data);
            return behaviour;
        }
    }
}