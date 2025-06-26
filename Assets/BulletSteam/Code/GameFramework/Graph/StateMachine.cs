using System;
using System.Collections.Generic;

namespace BulletSteam.GameFramework.Graph
{
    public class StateMachine : State
    {
        protected readonly Graph Graph;
        private NodeId _currentState;
        private readonly NodeId _initialState;


        public StateMachine(Graph graph)
        {
            Graph = graph;
            _initialState = graph.StartNode.Key;
            _currentState = _initialState;
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _currentState = _initialState;
        }

        protected override void OnUpdate(float deltaTime)
        {
            Node node = Graph[_currentState];
            Result result = node.Execute();
            switch (result)
            {
                case Result.Success:
                {
                    Transition transition = Graph.GetTransition(_currentState);
                    if (transition == Transition.Empty)
                    {
                        Finish(true);
                        return;
                    }
                    _currentState = transition.Destination;
                    break;
                }
                case Result.Failure:
                    Finish(false);
                    break;
                case Result.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }


    public class StateMachine<TAgent> : StateMachine, IAGentSettable<TAgent>
    {
        private TAgent _agent;

        public StateMachine(Graph graph) : base(graph)
        {
        }

        public void SetAgent(TAgent agent)
        {
            _agent = agent;
            foreach (KeyValuePair<NodeId, Node> pair in Graph.Nodes)
            {
                if (pair.Value is IAGentSettable<TAgent> node)
                {
                    node.SetAgent(agent);
                }
            }
        }
    }

    public static class StateMachineExtensions
    {
        public static void Update(this StateMachine stateMachine, float deltaTime)
        {
            stateMachine.DeltaTime = deltaTime;
            stateMachine.Execute();
        }
    }
}