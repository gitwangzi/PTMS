
namespace Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor
{
    public interface ITsDescriptorFactoryInstance
    {
        TsDescriptorType Type { get; }

        TsDescriptor Create(byte[] buffer, int offset, int length);
    }
}
