using System;
using System.Collections.Generic;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.MediaParser;

namespace Gsafety.PTMS.Media.AAC
{
    public class AacMediaParserFactory : MediaParserFactoryBase<AacMediaParser>
    {
        static readonly ContentType[] Types = { ContentTypes.Aac };

        public AacMediaParserFactory(Func<AacMediaParser> factory)
            : base(factory)
        { }

        public override ICollection<ContentType> KnownContentTypes
        {
            get { return Types; }
        }
    }
}
