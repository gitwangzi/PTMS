using System.Collections.Generic;

namespace Gsafety.PTMS.Media.H264
{
    public interface IH264ConfiguratorSink
    {
        IEnumerable<byte> SpsBytes { get; }
        void ParseSpsBytes(ICollection<byte> value);
        IEnumerable<byte> PpsBytes { get; }
        void ParsePpsBytes(ICollection<byte> value);

        bool IsConfigured { get; }
    }
}
