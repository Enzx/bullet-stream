using UnityEngine;

namespace BulletSteam.GameFramework.DataModel
{
    public abstract class MonoData<TObject, TInterface> : MonoData where TObject : IObject
    {
        public sealed override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<TObject, TInterface>(this);
        }
    }

    public abstract class MonoData<TObject> : MonoData where TObject : IObject
    {
        public sealed override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<TObject>(this);
        }
    }

    public abstract class MonoData : MonoBehaviour, IData
    {
        public abstract IObject Accept(IDataVisitor dataVisitor);
    }
}