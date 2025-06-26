using UnityEngine;

namespace BulletSteam.GameFramework.Graph
{
    public abstract class ActionTask : ScriptableObject
    {
        public string ErrorMessage { get; private set; }

        public virtual void ReportError(string message, object context = null)
        {
            ErrorMessage = context != null ? $"{context}: {message}" : $"{message}";
        }
        
        public void Reset()
        {
            ErrorMessage = null;
        }
        
        public virtual void Tick (float deltaTime)
        {
        }

        public abstract Result Execute();
    }

    public abstract class ActionTask<TAgent> : ActionTask, IAGentSettable<TAgent>
    {
        protected TAgent Agent;


        public void SetAgent(TAgent agent)
        {
            Agent = agent;
        }

        public override void ReportError(string message, object context = null)
        {
            base.ReportError(message, Agent);
        }
    }
}