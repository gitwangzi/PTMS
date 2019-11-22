using System;

namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public abstract class TsDescriptorFactoryInstanceBase : ITsDescriptorFactoryInstance
    {
        readonly TsDescriptorType _type;

        protected TsDescriptorFactoryInstanceBase(TsDescriptorType type)
        {
            if (null == type)
                throw new ArgumentNullException("type");

            _type = type;
        }

        public TsDescriptorType Type
        {
            get { return _type; }
        }

        public abstract TsDescriptor Create(byte[] buffer, int offset, int length);
    }
}
