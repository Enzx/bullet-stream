namespace BulletSteam.GameFramework.Graph
{
    public abstract class Node : IObject
    {
        public NodeId Key => _data.Key;

        private readonly NodeData _data;

        protected Node(NodeData data = default)
        {
            _data = ReferenceEquals(data, null) ?  NodeData.Create<NodeData>() : data;
        }

        public abstract Result Execute();
    }
}