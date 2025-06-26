namespace BulletSteam.GameFramework.Actor.Behaviours
{
    public interface IBehaviour : IObject
    {
    }
    
    public interface IBehaviour<out TData> : IBehaviour
        where TData : class
    {
        TData Data { get; }
    }
}