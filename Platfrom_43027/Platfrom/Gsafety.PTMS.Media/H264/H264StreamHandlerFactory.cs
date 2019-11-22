using System.Collections.Generic;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.H264
{
    public class H264StreamHandlerFactory : IPesStreamFactoryInstance
    {
        static readonly byte[] Types = { TsStreamType.H264StreamType };

        public ICollection<byte> SupportedStreamTypes
        {
            get { return Types; }
        }

        public PesStreamHandler Create(PesStreamParameters parameters)
        {
            return new H264StreamHandler(parameters);
        }
    }
}
