using System;
using System.Collections.Generic;

namespace BulletSteam.GameFramework.Graph
{
    public abstract class State<TAgent> : State, IAGentSettable<TAgent>
    {
        protected TAgent Agent;

        public override void AddAction(ActionTask action)
        {
            if (action is IAGentSettable<TAgent> agentSettable)
            {
                agentSettable.SetAgent(Agent);
            }

            base.AddAction(action);
        }

        public void SetAgent(TAgent agent)
        {
            Agent = agent;
        }
    }

    public class State : Node
    {
        private Status _status;
        private Result _result;
        public float DeltaTime;
        private List<ActionTask> _actions;

        public State(StateData data = default) : base(data)
        {
            _actions = ReferenceEquals(data, null) ? new List<ActionTask>() : data.Actions;
        }

        public virtual void AddAction(ActionTask action)
        {
            _actions.Add(action);
        }

        public override Result Execute()
        {
            switch (_status)
            {
                case Status.Enter:
                    Enter();
                    break;
                case Status.Update:
                    Update();
                    break;
                case Status.Exit:
                    Exit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return _result;
        }


        // ReSharper disable Unity.PerformanceAnalysis
        private void Enter()
        {
            _status = Status.Enter;
            _result = Result.None;
            _actions.ForEach(action => action.Execute());
            OnEnter();
            _status = Status.Update;
        }

        private void Update()
        {
            _actions.ForEach(action => action.Tick(DeltaTime));
            OnUpdate(DeltaTime);
        }

        private void Exit()
        {
            OnExit();
        }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnUpdate(float deltaTime)
        {
        }

        protected virtual void OnExit()
        {
        }

        protected void Finish(bool success)
        {
            _result = success ? Result.Success : Result.Failure;
            _status = Status.Exit;
        }
    }
}