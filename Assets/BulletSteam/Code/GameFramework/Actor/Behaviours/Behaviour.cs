namespace BulletSteam.GameFramework.Actor.Behaviours
{
    public abstract class Behaviour : IBehaviour, IUpdate, IFixedUpdate
    {
        public virtual void Update(float deltaTime)
        {
            
        }

        public virtual void FixedUpdate(float deltaTime)
        {
        }
    }
    
    public abstract class Behaviour<TData> : Behaviour, IBehaviour<TData> where TData : class
    {
        public TData Data { get; }

        protected Behaviour(TData data)
        {
            Data = data;
        }
   
    }
}