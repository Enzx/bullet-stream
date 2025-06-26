using System;
using UnityEngine;

namespace BulletSteam.GameFramework.Graph
{
    [Serializable]
    public struct NodeId
    {
        public static NodeId Empty => new() { Id = SerializableGuid.Empty };
        public SerializableGuid Id;
        

        public bool Equals(NodeId other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return obj is NodeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        
        public static bool operator ==(NodeId a, NodeId b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(NodeId a, NodeId b)
        {
            return !(a == b);
        }
        

    }

    //Serializable GUID struct
    [Serializable]
    public struct SerializableGuid
    {
        [SerializeField] private byte[] _guidBytes;
        
        public static SerializableGuid Empty => new(Guid.Empty);

        public SerializableGuid(Guid guid)
        {
            _guidBytes = guid.ToByteArray();
        }
        
        public static SerializableGuid NewGuid()
        {
            return new SerializableGuid(Guid.NewGuid());
        }

        public Guid ToGuid()
        {
            return new Guid(_guidBytes);
        }

        public static implicit operator Guid(SerializableGuid serializableGuid)
        {
            return serializableGuid.ToGuid();
        }

        public static implicit operator SerializableGuid(Guid guid)
        {
            return new SerializableGuid(guid);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < _guidBytes.Length; i++)
                {
                    hash = (hash ^ _guidBytes[i]) * p;
                }

                return hash;
            }
        }

        public override string ToString()
        {
            return ToGuid().ToString();
        }
    }
}