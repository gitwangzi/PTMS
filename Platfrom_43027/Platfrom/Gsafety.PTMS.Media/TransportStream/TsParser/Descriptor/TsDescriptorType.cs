using System;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public class TsDescriptorType : IEquatable<TsDescriptorType>
    {
        readonly byte _code;
        readonly string _description;

        public TsDescriptorType(byte code, string description)
        {
            _code = code;
            _description = description;
        }

        public byte Code
        {
            get { return _code; }
        }

        public string Description
        {
            get { return _description; }
        }

        public bool Equals(TsDescriptorType other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;

            return _code == other._code;
        }

        public override int GetHashCode()
        {
            return _code.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TsDescriptorType);
        }

        public override string ToString()
        {
            return _code + ":" + _description;
        }
    }
}
