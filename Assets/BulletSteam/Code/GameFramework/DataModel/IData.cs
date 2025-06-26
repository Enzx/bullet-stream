namespace BulletSteam.GameFramework.DataModel
{
    public interface IData
    {
        IObject Accept(IDataVisitor dataVisitor);
    }
}