using System;
using BulletSteam.GameFramework.Messaging;
using UnityEngine;

namespace BulletSteam.GameFramework.Actor.DataModel
{
    [Serializable]
    public class Attrib<TType> : IAttrib 
    {
        public readonly Events Events;

        private TType _previousValue;
        [SerializeField] private TType _value;
        [SerializeField] private TType _initialValue;
        [SerializeField] private TType _minValue;
        [SerializeField] private TType _maxValue;

        public Attrib(TType initialValue, TType minValue, TType maxValue)
        {
            _initialValue = initialValue;
            _minValue = minValue;
            _maxValue = maxValue;
            _value = initialValue;
            _previousValue = initialValue;

            Events = new Events();
        }

        public TType Value
        {
            get => _value;
            set
            {
                if (value.Equals(_value))
                    return;
                //Note: Add derivatives for value types to avoid boxing and apply clamp 
                // if (_minValue.CompareTo(value) < 0)
                //     _value = _minValue;
                // if (_maxValue.CompareTo(value) > 0)
                //     _value = _maxValue;

                _previousValue = _value;
                _value = value;
                Events.Publish(new ValueChangedMessage<TType>()
                {
                    PreviousValue = _previousValue,
                    CurrentValue = _value
                });
            }
        }

        public TType PreviousValue => _previousValue;

        public TType MinValue
        {
            get => _maxValue;
#if UNITY_EDITOR
            set => _minValue = value;
#endif
        }

        public TType MaxValue
        {
            get => _maxValue;
#if UNITY_EDITOR
            set => _maxValue = value;
#endif
        }

        public TType InitialValue
        {
            get => _initialValue;
#if UNITY_EDITOR
            set => _initialValue = value;
#endif
        }

        public void Reset()
        {
            Value = _initialValue;
        }
    }
}