using System;

namespace BulletSteam.GameFramework.Messaging
{
    public class Handler<TMessage> : IHandler<TMessage>, IDisposable
    {
        private Action<TMessage> _callback;
        private readonly Filter<TMessage> _filter;
        
        public Handler(Action<TMessage> callback, Filter<TMessage> filter)
        {
            _callback = callback;
            _filter = filter;
        }

        public void Dispose()
        {
            _callback = null;
        }

        public bool Filter(TMessage message)
        {
            return _filter == default || _filter.Apply(message);
        }

        // ReSharper disable Unity.PerformanceAnalysis

        public void Handle(TMessage message)
        {
            _callback(message);
        }
    }
}