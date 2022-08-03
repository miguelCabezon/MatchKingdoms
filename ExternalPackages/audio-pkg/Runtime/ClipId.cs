using System;
using UnityEngine;

namespace JackSParrot.Audio
{
    [System.Serializable]
    public class ClipId : IEquatable<ClipId>
    {
        public string Id;

        public ClipId(string id)
        {
            Id = id;
            Debug.Assert(!string.IsNullOrEmpty(id));
        }

        public bool IsValid() => !string.IsNullOrEmpty(Id);

        public bool Equals(ClipId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Id.Equals(((ClipId) obj).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(ClipId a, ClipId b)
        {
            return a.Id == b.Id;
        }

        public static bool operator ==(ClipId a, string b)
        {
            return a.Id == b;
        }

        public static bool operator !=(ClipId a, string b)
        {
            return !(a.Id == b);
        }

        public static bool operator !=(string a, ClipId b)
        {
            return !(a == b.Id);
        }

        public static bool operator ==(string a, ClipId b)
        {
            return a == b.Id;
        }

        public static bool operator !=(ClipId a, ClipId b)
        {
            return !(a == b);
        }
    }
}