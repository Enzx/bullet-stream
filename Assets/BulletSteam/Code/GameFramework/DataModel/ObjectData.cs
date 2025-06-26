using UnityEngine;

namespace BulletSteam.GameFramework.DataModel
{
    public abstract class ObjectData<TObject, TInterface> : ObjectData where TObject : IObject
    {
        public sealed override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<TObject, TInterface>(this);
        }
    }

    public abstract class ObjectData<TObject> : ObjectData where TObject : IObject
    {
        public sealed override IObject Accept(IDataVisitor dataVisitor)
        {
            return dataVisitor.Visit<TObject>(this);
        }
    }

    public abstract class ObjectData : ScriptableObject, IData
    {
        public abstract IObject Accept(IDataVisitor dataVisitor);
    }
}