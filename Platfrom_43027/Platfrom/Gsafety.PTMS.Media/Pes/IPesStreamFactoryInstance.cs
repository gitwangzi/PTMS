using System.Collections.Generic;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.Pes
{
    public interface IPesStreamFactoryInstance
    {
        ICollection<byte> SupportedStreamTypes { get; }
        PesStreamHandler Create(PesStreamParameters parameters);
    }
}
