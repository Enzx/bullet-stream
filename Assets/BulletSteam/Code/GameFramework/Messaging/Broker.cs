using System;
using System.Collections.Generic;

namespace BulletSteam.GameFramework.Messaging
{
    /// <summary>
    ///  A message broker that allows for the publishing and subscribing of messages
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class Broker<TMessage> : IPublisher<TMessage>, ISubscriber<TMessage>, IDisposable
    {
        private readonly List<IHandler<TMessage>> _handlers = new(128);


        /// <summary>
        /// Publishes a message to all subscribers
        /// </summary>
        /// <param name="message">Template message type</param>
        public void Publish(TMessage message)
        {
            for (int i = 0; i < _handlers.Count; i++)
            {
                IHandler<TMessage> handler = _handlers[i];
                if (handler.Filter(message))
                {
                    handler.Handle(message);
                }
            }
        }

        /// <summary>
        /// Subscribes to the message type
        /// </summary>
        ///<param name="handler">a delegate to a function</param>
        public IDisposable Subscribe(IHandler<TMessage> handler)
        {
            if (_handlers.Contains(handler))
                return SubscriptionToken<TMessage>.Empty;

            SubscriptionToken<TMessage> token = new(_handlers.Count, this);
            _handlers.Add(handler);
            return token;
        }

        public void Unsubscribe(IDisposable token)
        {
            SubscriptionToken<TMessage> subscriptionToken = (SubscriptionToken<TMessage>)token;
            if (subscriptionToken.Id < 0 || subscriptionToken.Id >= _handlers.Count)
                return;
            IHandler<TMessage> handler = _handlers[subscriptionToken.Id]; 
            _handlers.Remove(handler);
            handler.Dispose();
        }

        /// <summary>
        /// Publishes a message to all subscribers
        /// </summary>
        /// <param name="obj">Generic message</param>
        public void Publish(object obj)
        {
            Publish((TMessage)obj);
        }

        /// <summary>
        /// Disposes of all handlers
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < _handlers.Count; i++)
            {
                _handlers[i].Dispose();
            }

            _handlers.Clear();
        }
    }
}