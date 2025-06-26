using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace BulletSteam.GameFramework.Messaging
{
    public interface ISubscriber
    {
        IDisposable Subscribe<TMessage>(in IHandler<TMessage> handler);
        IDisposable Subscribe<TMessage>(Action<TMessage> action);
        IDisposable Subscribe<T>(Action<T> action, Filter<T> filter);
        IDisposable Subscribe<TMessage>(Action<TMessage> action, params Filter<TMessage>[] filters);
    }

    public sealed class Events : IDisposable, ISubscriber
    {
        private readonly ConcurrentDictionary<Type, IPublisher> _brokers = new(4, 10);

        public bool Publish<TMessage>(TMessage message)
        {
            if (!_brokers.TryGetValue(message.GetType(), out IPublisher publisher)) return false;
            if (publisher is IPublisher<TMessage> templatePublisher)
            {
                templatePublisher.Publish(message);
                return true;
            }

            publisher.Publish(message);
            return true;
        }

        public IDisposable Subscribe<TMessage>(in IHandler<TMessage> handler)
        {
            IPublisher broker = _brokers.GetOrAdd(typeof(TMessage), _ => new Broker<TMessage>());
            if (broker is Broker<TMessage> templateBroker)
            {
                return templateBroker.Subscribe(handler);
            }

            throw new InvalidOperationException(
                "Cannot subscribe to a message type that is not of the same type as the broker");
        }
        
        // Extension method for Events class
        public IDisposable Subscribe<TMessage>(Action<TMessage> action)
        {
            return Subscribe(new Handler<TMessage>(action, null));
        }

        public IDisposable Subscribe<T>(Action<T> action, Filter<T> filter)
        {
            return Subscribe(new Handler<T>(action, filter));
        }
        
        public IDisposable Subscribe<TMessage>(Action<TMessage> action, params Filter<TMessage>[] filters)
        {
            return Subscribe(new Handler<TMessage>(action, new CompositeFilter<TMessage,Filter<TMessage>>(filters)));
        }


        public void Dispose()
        {
            foreach (KeyValuePair<Type, IPublisher> broker in _brokers)
            {
                IDisposable disposable = broker.Value as IDisposable;
                disposable?.Dispose();
            }
        }
        
    }
}