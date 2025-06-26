using System;

namespace BulletSteam.GameFramework.Messaging
{
    /// <summary>
    /// A token representing a subscription.
    /// </summary>
    /// <summary>
    /// A token representing a subscription.
    /// </summary>
    public class SubscriptionToken<TMessage> : IDisposable
    {
        public static readonly SubscriptionToken<TMessage> Empty = new(-1, null);
        private readonly Broker<TMessage> _broker;
        public int Id { get; }


        internal SubscriptionToken(int tokenId, Broker<TMessage> broker)
        {
            Id = tokenId;
            _broker = broker;
        }

        public void Dispose()
        {
            _broker?.Unsubscribe(this);
        }

    }
}