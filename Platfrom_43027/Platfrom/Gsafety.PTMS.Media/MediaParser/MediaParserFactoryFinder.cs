using System.Collections.Generic;
using System.Linq;
using Gsafety.PTMS.Media.Content;

namespace Gsafety.PTMS.Media.MediaParser
{
    public interface IMediaParserFactoryFinder : IContentServiceFactoryFinder<IMediaParser, IMediaParserParameters>
    {
        void Register(ContentType contentType, IMediaParserFactoryInstance factory);
    }

    public class MediaParserFactoryFinder : ContentServiceFactoryFinder<IMediaParser, IMediaParserParameters>, IMediaParserFactoryFinder
    {
        public MediaParserFactoryFinder(IEnumerable<IMediaParserFactoryInstance> factoryInstances)
            : base(factoryInstances.OfType<IContentServiceFactoryInstance<IMediaParser, IMediaParserParameters>>())
        {
        }

        public void Register(ContentType contentType, IMediaParserFactoryInstance factory)
        {
            Register(contentType, (IContentServiceFactoryInstance<IMediaParser, IMediaParserParameters>)factory);
        }
    }
}
