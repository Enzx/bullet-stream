using System;
using BulletSteam.GameFramework.Actor.Behaviours;
using BulletSteam.GameFramework.Collections;
using BulletSteam.GameFramework.DataModel;

namespace BulletSteam.GameFramework.Actor
{
    public class Builder : IDataVisitor
    {
        private readonly IBehaviour _agent;
        private readonly ServiceLocator<IBehaviour> _serviceLocator;
        public Builder(IBehaviour agent, ServiceLocator<IBehaviour> serviceLocator)
        {
            _agent = agent;
            _serviceLocator = serviceLocator;
        }
    

        public IObject Visit<TBehavior>(IData data)
        {
            Type dataType = typeof(TBehavior);
            IBehaviour behaviour = (IBehaviour)Activator.CreateInstance(dataType, _agent, data);
            _serviceLocator.Register(behaviour);
            return behaviour;
        }

        public IObject Visit<TBehavior, TInterface>(IData data)
        {
            IBehaviour behaviour = (IBehaviour)Visit<TBehavior>(data);
            _serviceLocator.Register<TInterface>(behaviour);
            return behaviour;
        }
    }
}