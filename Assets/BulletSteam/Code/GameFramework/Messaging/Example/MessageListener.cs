using UnityEngine;

namespace BulletSteam.GameFramework.Messaging.Example
{
    public class MessageListener
    {
        ISubscriber<TextMessage> _subscriber;

        public MessageListener(ISubscriber<TextMessage> subscriber)
        {
            _subscriber = subscriber;
            FilterDuplication<TextMessage> filter = new();
            FilterEvenNumbers evenFilter = new();
            CompositeFilter<TextMessage, Filter<TextMessage>> compositeFilter = new(filter, evenFilter);
            _subscriber.Subscribe(PrintMessage, compositeFilter);
          //  _subscriber.Subscribe(HandleMessage);
        }

        private void PrintMessage(TextMessage msg)
        {
            Debug.Log(msg.Text);
        }
    }
}