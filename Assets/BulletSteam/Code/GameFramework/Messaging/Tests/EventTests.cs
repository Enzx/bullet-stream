using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace BulletSteam.GameFramework.Messaging.Tests
{
    public class EventTests
    {
        // Test that the Event Class can subscribe and publish
        [UnityTest]
        public IEnumerator Event_SubscribeAndPublish()
        {
            Events subscriber = new();
            subscriber.Subscribe<int>(i => Assert.AreEqual(1, i, "Event_SubscribeAndPublish"));
            subscriber.Publish(1);
            subscriber.Dispose();
            yield return null;
        }

        // Test that event can unsubscribe
        [UnityTest]
        public IEnumerator Event_Unsubscribe()
        {
            Events subscriber = new();
            IDisposable token =
                subscriber.Subscribe<int>(i => Assert.AreEqual(1, i, "Event_SubscribeAndPublishEnumerator"));
            //Publish event and check if it is called
            subscriber.Publish(1);
            //Unsubscribe
            token.Dispose();
            //This should not be called
            subscriber.Publish(2);

            yield return null;
        }

        // Test that callback can get called until token is valid
        [UnityTest]
        public IEnumerator Event_UnsubscribeInvalidToken()
        {
            Events subscriber = new();
            IDisposable token =
                subscriber.Subscribe<int>(i => Assert.AreEqual(1, i, " Event_UnsubscribeInvalidToken"));
            //Publish event and check if it is called
            subscriber.Publish(1);
            subscriber.Publish(1);
            //Unsubscribe
            token.Dispose();
            //This should not be called
            subscriber.Publish(2);

            yield return null;
        }

        // Test that disposing the event will unsubscribe all tokens
        [UnityTest]
        public IEnumerator Event_Dispose()
        {
            Events subscriber = new();
            _ = subscriber.Subscribe<int>(i => Assert.AreEqual(1, i, " Event_Dispose"));
            //Publish event and check if it is called
            subscriber.Publish(1);
            subscriber.Publish(1);
            //Dispose
            subscriber.Dispose();
            //This should not be called
            subscriber.Publish(2);

            yield return null;
        }

        // Test Filter with even numbers
        [UnityTest]
        public IEnumerator Event_FilterEvenNumbers()
        {
            Events subscriber = new();
            subscriber.Subscribe(i => Assert.IsTrue(i % 2 != 0, "Event_FilterEvenNumbers"), new OddNumbersFilter());
            for (int i = 0; i < 10; i++)
            {
                subscriber.Publish(i);
            }

            subscriber.Dispose();
            yield return null;
        }

        // Test Composite Filter with even numbers and greater than 5
        [UnityTest]
        public IEnumerator Event_CompositeFilter()
        {
            Events subscriber = new();
            subscriber.Subscribe(i => Assert.IsTrue(i % 2 != 0 && i > 5, "Event_CompositeFilter"),
                new OddNumbersFilter(),
                new GreaterThanFilter(5));
            
            for (int i = 0; i < 10; i++)
            {
                subscriber.Publish(i);
            }

            subscriber.Dispose();
            yield return null;
        }

        private class OddNumbersFilter : Filter<int>
        {
            public override bool Apply(int message)
            {
                return message % 2 != 0;
            }
        }

        private class GreaterThanFilter : Filter<int>
        {
            private readonly int _value;

            public GreaterThanFilter(int value)
            {
                _value = value;
            }

            public override bool Apply(int message)
            {
                return message > _value;
            }
        }
    }
}