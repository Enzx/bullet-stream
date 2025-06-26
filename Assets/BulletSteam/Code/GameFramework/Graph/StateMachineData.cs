using BulletSteam.GameFramework.DataModel;

namespace BulletSteam.GameFramework.Graph
{
    public class StateMachineData : GraphData
    {
        public override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<StateMachine>(this);
        }
    }
}