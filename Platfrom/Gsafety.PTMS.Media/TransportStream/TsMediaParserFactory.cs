using System;
using System.Collections.Generic;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.MediaParser;

namespace Gsafety.PTMS.Media.TransportStream
{
    public class TsMediaParserFactory : MediaParserFactoryBase<TsMediaParser>
    {
        static readonly ContentType[] Types = { ContentTypes.TransportStream };

        public TsMediaParserFactory(Func<TsMediaParser> factory)
            : base(factory)
        { }

        public override ICollection<ContentType> KnownContentTypes
        {
            get { return Types; }
        }
    }
}
