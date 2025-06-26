using UnityEngine;

namespace BulletSteam.GameFramework.Messaging.Example
{
    //Pub-Sub

    public class MessagingSystem : MonoBehaviour
    {
        MessageBroadcaster broadcaster;

        private void Awake()
        {
            Broker<TextMessage> messageBroker = new();
            broadcaster = new MessageBroadcaster(messageBroker);
            MessageListener listener = new(messageBroker);
            //   MessageListenerWithFormatting listener2 = new MessageListenerWithFormatting(messageBroker);
        }

        private void Update()
        {
            broadcaster.Tick(Time.deltaTime);
        }
    }
}