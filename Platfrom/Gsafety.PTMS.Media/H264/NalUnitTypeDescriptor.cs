using System;

namespace Gsafety.PTMS.Media.H264
{
    public sealed class NalUnitTypeDescriptor : IEquatable<NalUnitTypeDescriptor>
    {
        readonly string _description;
        readonly string _name;
        readonly NalUnitType _type;

        public NalUnitTypeDescriptor(NalUnitType type, string name, string description)
        {
            _type = type;
            _name = name;
            _description = description;
        }

        public NalUnitType Type
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }
        public bool Equals(NalUnitTypeDescriptor other)
        {
            if (ReferenceEquals(this, other))
                return true;

            if (ReferenceEquals(null, other))
                return false;

            return _type == other._type;
        }

        public override int GetHashCode()
        {
            return (int)_type;
        }

        public override bool Equals(object obj)
        {
            var other = obj as NalUnitTypeDescriptor;

            if (ReferenceEquals(null, other))
                return false;

            return Equals(other);
        }

        public static bool operator ==(NalUnitTypeDescriptor a, NalUnitTypeDescriptor b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(NalUnitTypeDescriptor a, NalUnitTypeDescriptor b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return string.Format("{0}/{1}", _type, _name);
        }
    }
}
