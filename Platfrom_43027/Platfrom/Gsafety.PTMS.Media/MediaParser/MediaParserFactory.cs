using Gsafety.PTMS.Media.Content;

namespace Gsafety.PTMS.Media.MediaParser
{
    public interface IMediaParserFactory : IContentServiceFactory<IMediaParser, IMediaParserParameters>
    { }

    public class MediaParserFactory : ContentServiceFactory<IMediaParser, IMediaParserParameters>, IMediaParserFactory
    {
        public MediaParserFactory(IMediaParserFactoryFinder factoryFinder)
            : base(factoryFinder)
        { }
    }
}
