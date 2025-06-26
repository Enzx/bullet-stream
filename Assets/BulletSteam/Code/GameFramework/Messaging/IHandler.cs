namespace BulletSteam.GameFramework.Messaging
{
    public interface IHandler<in TMessage>
    {
        void Handle(TMessage message);
        void Dispose();
        bool Filter(TMessage message);
    }
}