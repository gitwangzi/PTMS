namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public interface IPesStreamHandler
    {
        void PacketHandler(TsPesPacket packet);
    }
}
