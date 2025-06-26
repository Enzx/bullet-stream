using System;

namespace BulletSteam.GameFramework.Graph
{
    [Serializable]
    public class Transition
    {
        public static readonly Transition Empty = new(NodeId.Empty, NodeId.Empty);
        public NodeId Source;
        public NodeId Destination;
        
        //implement == operator
        public static bool operator ==(Transition a, Transition b)
        {
            return a.Source == b.Source && a.Destination == b.Destination;
        }
        
        public static bool operator !=(Transition a, Transition b)
        {
            return !(a == b);
        }
        
        public Transition(NodeId from, NodeId to)
        {
            Source = from;
            Destination = to;
        }
    }
}