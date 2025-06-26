namespace BulletSteam.GameFramework.Graph
{
    public interface IAGentSettable<in TAgent>
    {
        void SetAgent(TAgent agent);
    }
}