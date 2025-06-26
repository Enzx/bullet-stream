using System;

namespace BulletSteam.GameFramework.Messaging
{
    public interface ISubscriber<TMessage>
    {
        IDisposable Subscribe(IHandler<TMessage> message);
        void Unsubscribe(IDisposable token);
    }
}