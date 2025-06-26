namespace BulletSteam.GameFramework.Messaging.Example
{
    public class MessageBroadcaster
    {
        IPublisher<TextMessage> _publisher;

        public MessageBroadcaster(IPublisher<TextMessage> publisher)
        {
            _publisher = publisher;
        }

        private int count;
        private float _time;

        public void Tick(float dt)
        {
            _time += dt;

            if (_time >= 1)
            {
                _publisher.Publish(new TextMessage {Text = $"{count++}"});
                _publisher.Publish(new TextMessage{Text = $"{count}"});

                _time = 0;
            }
        }
    }
}