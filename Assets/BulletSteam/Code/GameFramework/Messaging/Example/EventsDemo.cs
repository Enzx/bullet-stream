using UnityEngine;

namespace BulletSteam.GameFramework.Messaging.Example
{
    public class EventsDemo : MonoBehaviour
    {
        private readonly Events _subscriber = new Events();

        private void Awake()
        {
            _subscriber.Subscribe<TextMessage>(TextMessageHandler);
            _subscriber.Subscribe<ActivationMessage>(ActivationHandler);
            _subscriber.Subscribe<int>(i => { print(i); });

            Message message = new TextMessage() { Text = "Text Message" };
            if (_subscriber.Publish(message) == false)
            {
                print($"There is no broker for {message.GetType()}");
            }

            message = new ActivationMessage();
            if (_subscriber.Publish(message) == false)
            {
                print($"There is no broker for {message.GetType()}");
            }
            
            _subscriber.Publish(0);
        }
        
        private void TextMessageHandler(TextMessage msg)
        {
            print(msg);
        }

        private void ActivationHandler(ActivationMessage msg)
        {
            print(msg);
        }
    }
}