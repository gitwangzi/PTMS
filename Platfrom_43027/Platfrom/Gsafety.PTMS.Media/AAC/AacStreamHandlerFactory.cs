using System.Collections.Generic;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.AAC
{
    public class AacStreamHandlerFactory : IPesStreamFactoryInstance
    {
        static readonly byte[] Types = { TsStreamType.AacStreamType };


        public ICollection<byte> SupportedStreamTypes
        {
            get { return Types; }
        }

        public PesStreamHandler Create(PesStreamParameters parameters)
        {
            return new AacStreamHandler(parameters);
        }
    }
}
