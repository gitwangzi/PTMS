namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public class TsDescriptor
    {
        readonly TsDescriptorType _type;

        public TsDescriptor(TsDescriptorType type)
        {
            _type = type;
        }

        public TsDescriptorType Type
        {
            get { return _type; }
        }
    }
}
