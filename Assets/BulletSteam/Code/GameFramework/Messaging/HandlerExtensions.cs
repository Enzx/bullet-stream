using System;

namespace BulletSteam.GameFramework.Messaging
{
    public static class HandlerExtensions
    {
        public static IDisposable Subscribe<TMessage>(this ISubscriber<TMessage> subscriber, Action<TMessage> action)
        {
            return subscriber.Subscribe(new Handler<TMessage>(action, null));
        }

        public static IDisposable Subscribe<T>(this ISubscriber<T> subscriber, Action<T> action, Filter<T> filter)
        {
            return subscriber.Subscribe(new Handler<T>(action, filter));
        }

       
    }
}