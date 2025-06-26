namespace BulletSteam.GameFramework.Messaging
{
    public interface IPublisher
    {
        void Publish(object obj);
    }
    public interface IPublisher<in TMessage> : IPublisher
    {
        void Publish(TMessage message);
    }
}