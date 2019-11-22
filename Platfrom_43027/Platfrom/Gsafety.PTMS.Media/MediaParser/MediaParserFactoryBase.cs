using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;

namespace Gsafety.PTMS.Media.MediaParser
{
    public abstract class MediaParserFactoryBase<TMediaParser> : IMediaParserFactoryInstance
        where TMediaParser : IMediaParser
    {
        readonly Func<TMediaParser> _parserFactory;

        protected MediaParserFactoryBase(Func<TMediaParser> parserFactory)
        {
            if (null == parserFactory)
                throw new ArgumentNullException("parserFactory");

            _parserFactory = parserFactory;
        }

        public abstract ICollection<ContentType> KnownContentTypes { get; }

        public Task<IMediaParser> CreateAsync(IMediaParserParameters parameter, ContentType contentType, CancellationToken cancellationToken)
        {
            var mediaParser = _parserFactory();
            
            return Task.Factory.StartNew<IMediaParser>(() => { return mediaParser; });
        }
    }
}
