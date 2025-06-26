using System;
using System.Collections.Generic;
using System.Linq;

namespace BulletSteam.GameFramework.Collections
{
    public class ServiceLocator<TType>
    {
        private readonly Dictionary<Type, TType> _behaviours;

        private FixedTypeKeyHashtable<TType> _behaviorTypeMap;

        public ServiceLocator(int count = 0)
        {
            _behaviours = new Dictionary<Type, TType>(count);
        }


        private TType Register(Type type, TType instance)
        {
            _behaviours.Add(type, instance);
            return instance;
        }

        public TType Register(TType instance)
        {
            Type type = instance.GetType();
            return Register(type, instance);
        }

        public TInterface Register<TInterface, TImplementation>()
            where TInterface : TType
            where TImplementation : TInterface, new()
        {
            Type type = typeof(TInterface);
            return (TInterface)Register(type, new TImplementation());
        }

        public TImplementation Register<TImplementation>()
            where TImplementation : TType, new()
        {
            Type type = typeof(TImplementation);
            return (TImplementation)Register(type, new TImplementation());
        }

        public void Register<TInterface>(TType instance)
        {
            Type type = typeof(TInterface);
            Register(type, instance);
        }

        public void Build()
        {
            _behaviorTypeMap = new FixedTypeKeyHashtable<TType>(_behaviours.ToArray());
            _behaviorTypeMap.ForEach(type =>
            {
                if (type is IService service) service.OnBuild(this);
            });
        }

        public T Get<T>() where T : class, TType
        {
            return _behaviorTypeMap.Get(typeof(T)) as T;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ForEach(Action<TType> action)
        {
            _behaviorTypeMap.ForEach(action);
        }

        public List<T> GetAll<T>()
        {
            List<T> result = new();
             _behaviorTypeMap.ForEach(type =>
             {
                 if (type is T t) result.Add(t);
             });
            return result;
        }
    }
}