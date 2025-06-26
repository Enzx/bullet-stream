using System;
using System.Collections.Generic;

namespace BulletSteam.GameFramework.Graph
{
    public abstract class Condition : Node
    {
        public ExecuteMode ExecuteMode;
        private readonly List<ConditionTask> _conditions;
        private Result _result;
        private bool _isInitialized;

        protected Condition(ConditionData data = default) : base(data)
        {
            if (ReferenceEquals(data, null))
            {
                _conditions = new List<ConditionTask>();
                ExecuteMode = ExecuteMode.Sequence;
            }
            else
            {
                _conditions = data.Conditions;
                ExecuteMode = data.ExecuteMode;
            }
            _result = Result.Success;
            _isInitialized = false;
        }
        
        protected virtual void Initialize()
        {
            foreach (ConditionTask condition in _conditions)
            {
                condition.Initialize();
            }
        }


        public override Result Execute()
        {
            if (_isInitialized == false)
            {
                Initialize();
                _isInitialized = true;
            }
           
                 Reset();
            
            bool success = ExecuteMode switch
            {
                ExecuteMode.Parallel => Parallel(),
                ExecuteMode.Sequence => Sequence(),
                _ => throw new ArgumentOutOfRangeException()
            };

            success = Check() && success;

            Finish(success);
            return _result;
        }


        private void Finish(bool success)
        {
            _result = success ? Result.Success : Result.Failure;
        }

        private bool Parallel()
        {
            bool success = true;
            for (int index = 0; index < _conditions.Count; index++)
            {
                ConditionTask condition = _conditions[index];
                if (!condition.Check())
                {
                    success = false;
                }
            }

            return success;
        }

        private bool Sequence()
        {
            bool success = true;
            for (int index = 0; index < _conditions.Count; index++)
            {
                ConditionTask condition = _conditions[index];
                if (!condition.Check())
                {
                    success = false;
                    break;
                }
            }

            return success;
        }

        protected virtual void Reset()
        {
            _result = Result.None;
        }

        protected virtual bool Check()
        {
            return true;
        }
    }

    public abstract class Condition<TAgent> : Condition, IAGentSettable<TAgent>
    {
        protected TAgent Agent;

        public void SetAgent(TAgent agent)
        {
            Agent = agent;
        }
    }
}