namespace Gsafety.PTMS.Media.Configuration
{
    public interface IFrameParser
    {
        int FrameLength { get; }
        bool Parse(byte[] buffer, int index, int length);
    }
}
