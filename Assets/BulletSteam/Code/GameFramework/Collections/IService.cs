namespace BulletSteam.GameFramework.Collections
{
    public interface IService
    {
        void OnBuild(ServiceLocator<IService> locator);
        void OnBuild<TType>(ServiceLocator<TType> locator);
    }
    
    public abstract class ServiceBase : IService
    {
        public virtual void OnBuild(ServiceLocator<IService> locator)
        {
        }

        public void OnBuild<TType>(ServiceLocator<TType> locator)
        {
            ServiceLocator<IService> serviceLocator = locator as ServiceLocator<IService>;
            OnBuild(serviceLocator);
        }
    }
}