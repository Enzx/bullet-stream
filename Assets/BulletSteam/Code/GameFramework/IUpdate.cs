namespace BulletSteam.GameFramework
{
    public interface IUpdate
    {
        void Update(float deltaTime);
    }
    
    public interface IFixedUpdate
    {
        void FixedUpdate(float deltaTime);
    }
}