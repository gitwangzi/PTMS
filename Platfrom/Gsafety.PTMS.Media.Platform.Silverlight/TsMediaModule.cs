using Autofac;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media
{
    public class TsMediaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlatformServices>()
                .As<IPlatformServices>()
                .SingleInstance();

            builder.RegisterType<MediaStreamConfigurator>().
                As<IMediaStreamConfigurator>()
                .As<IMediaStreamControl>()
                .InstancePerMatchingLifetimeScope("builder-scope");

            builder.RegisterType<TsMediaStreamSource>()
           .AsSelf()
           .ExternallyOwned();
        }
    }
}
