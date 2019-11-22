using Autofac;
using Gsafety.PTMS.Media.AAC;
using Gsafety.PTMS.Media.Audio.Shoutcast;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.H264;
using Gsafety.PTMS.Media.MediaManager;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.TransportStream;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Descriptor;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Utility.RandomGenerators;
using Gsafety.PTMS.Media.Utility.TextEncodings;
using Gsafety.PTMS.Media.Web;

namespace Gsafety.PTMS.Media
{
    public class TsParserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(new ContentTypeDetector(ContentTypes.AllTypes)).As<IContentTypeDetector>();

            builder.RegisterType<TsPesPacketPool>().As<ITsPesPacketPool>().SingleInstance();
            builder.RegisterType<BufferPool>().As<IBufferPool>().SingleInstance();
            builder.RegisterType<DefaultBufferPoolParameters>().As<IBufferPoolParameters>().SingleInstance();

            builder.RegisterType<MediaParserFactoryFinder>().As<IMediaParserFactoryFinder>().SingleInstance();
            builder.RegisterType<MediaParserFactory>().As<IMediaParserFactory>().SingleInstance();
            builder.RegisterType<WebMetadataFactory>().As<IWebMetadataFactory>().SingleInstance();
            builder.RegisterType<MetadataSink>().As<IMetadataSink>();

            builder.RegisterType<Utf8ShoutcastEncodingSelector>().As<IShoutcastEncodingSelector>().SingleInstance();
            builder.RegisterType<ShoutcastMetadataFilterFactory>().As<IShoutcastMetadataFilterFactory>().SingleInstance();

            builder.RegisterType<AacMediaParserFactory>().As<IMediaParserFactoryInstance>().SingleInstance().PreserveExistingDefaults();
            builder.RegisterType<TsMediaParserFactory>().As<IMediaParserFactoryInstance>().SingleInstance().PreserveExistingDefaults();

            builder.RegisterType<AacMediaParser>().AsSelf().ExternallyOwned();
            builder.RegisterType<TsMediaParser>().AsSelf().ExternallyOwned();

            builder.RegisterType<AacStreamHandlerFactory>().As<IPesStreamFactoryInstance>().InstancePerLifetimeScope().PreserveExistingDefaults();
            builder.RegisterType<H264StreamHandlerFactory>().As<IPesStreamFactoryInstance>().InstancePerLifetimeScope().PreserveExistingDefaults();

            builder.RegisterType<PesHandlerFactory>().As<IPesHandlerFactory>().SingleInstance();

            builder.RegisterType<PesStreamParameters>().AsSelf();

            builder.RegisterType<TsDecoder>().As<ITsDecoder>();
            builder.RegisterType<TsTimestamp>().As<ITsTimestamp>();
            builder.RegisterType<PesHandlers>().As<IPesHandlers>();

            builder.RegisterType<WebReaderManagerParameters>().As<IWebReaderManagerParameters>().SingleInstance();
            builder.RegisterType<MediaManagerParameters>().As<IMediaManagerParameters>().SingleInstance();
            builder.RegisterType<DefaultBufferingPolicy>().As<IBufferingPolicy>().InstancePerMatchingLifetimeScope("builder-scope");

            builder.RegisterType<BufferingManager>().As<IBufferingManager>();

            builder.RegisterType<RetryManager>().As<IRetryManager>().SingleInstance();

            builder.RegisterType<SmEncodings>().As<ISmEncodings>().SingleInstance();

            builder.RegisterType<UserAgent>().As<IUserAgent>().SingleInstance();

            builder.RegisterType<XorShift1024Star>().As<IRandomGenerator>();
            builder.RegisterType<XorShift1024Star>().As<IRandomGenerator<ulong>>();

            builder.RegisterType<TsProgramAssociationTableFactory>().As<ITsProgramAssociationTableFactory>().SingleInstance();

            builder.RegisterType<TsProgramMapTableFactory>().As<ITsProgramMapTableFactory>().SingleInstance();

            builder.RegisterType<TsIso639LanguageDescriptorFactory>().As<ITsDescriptorFactoryInstance>().SingleInstance().PreserveExistingDefaults();

            builder.RegisterType<TsDescriptorFactory>().As<ITsDescriptorFactory>().SingleInstance();
        }
    }
}
