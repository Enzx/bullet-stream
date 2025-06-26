using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletSteam.GameFramework.Actor.DataModel
{
    [Serializable]
    public class AttributeCollection : ISerializationCallbackReceiver
    {
        private Dictionary<Type, IAttrib> _attribs = new();
      
        private List<Type> _keys = new();
        private List<IAttrib> _values = new();
        
        public void Add<TAttrib>(TAttrib attrib) where TAttrib : IAttrib
        {
            _attribs.Add(typeof(TAttrib), attrib);
        }
        
        public TAttrib Get<TAttrib>() where TAttrib : IAttrib
        {
            return (TAttrib)_attribs[typeof(TAttrib)];
        }
        
        public void Reset()
        {
            foreach (KeyValuePair<Type, IAttrib> attrib in _attribs)
            {
                attrib.Value.Reset();
            }
        }

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            foreach (KeyValuePair<Type, IAttrib> attrib in _attribs)
            {
                _keys.Add(attrib.Key);
                _values.Add(attrib.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            _attribs = new Dictionary<Type, IAttrib>();
            for (int i = 0; i < _keys.Count; i++)
            {
                IAttrib attrib = _values[i];
                _attribs.Add(_keys[i], attrib);
            }
        }
    }
}