using System;
using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.Pes
{
    public interface IPesHandlerFactory
    {
        PesStreamHandler CreateHandler(PesStreamParameters parameters);
    }

    public sealed class PesHandlerFactory : IPesHandlerFactory
    {
        //    Table 2-34 Stream type assignments
        //    ISO/IEC 13818-1:2007/Amd.3:2009 (E)
        //    Rec. ITU-T H.222.0 (2006)/Amd.3 (03/2009)
        // TODO: TsStreamTypeContentTypes isn't actually used anywhere...
        static readonly IDictionary<byte, ContentType> TsStreamTypeContentTypes =
            new Dictionary<byte, ContentType>
            {
                { TsStreamType.H262StreamType, ContentTypes.H262 },
                { TsStreamType.Mp3Iso11172, ContentTypes.Mp3 },
                { TsStreamType.Mp3Iso13818, ContentTypes.Mp3 },
                { TsStreamType.H264StreamType, ContentTypes.H264 },
                { TsStreamType.AacStreamType, ContentTypes.Aac },
                { TsStreamType.Ac3StreamType, ContentTypes.Ac3 }
            };

        readonly Dictionary<byte, IPesStreamFactoryInstance> _factories;

        public PesHandlerFactory(IEnumerable<IPesStreamFactoryInstance> factoryInstances)
        {
            if (factoryInstances == null)
                throw new ArgumentNullException("factoryInstances");

            _factories = factoryInstances
                .SelectMany(fi => fi.SupportedStreamTypes,
                    (fi, contentType) => new
                    {
                        ContentType = contentType,
                        Instance = fi
                    })
                .ToDictionary(v => v.ContentType, v => v.Instance);
        }

        public PesStreamHandler CreateHandler(PesStreamParameters parameters)
        {
            IPesStreamFactoryInstance factory;
            if (!_factories.TryGetValue(parameters.StreamType.StreamType, out factory))
                return new DefaultPesStreamHandler(parameters);

            return factory.Create(parameters);
        }

    }
}
