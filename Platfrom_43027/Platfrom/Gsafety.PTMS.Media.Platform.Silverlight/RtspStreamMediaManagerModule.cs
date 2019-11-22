using Autofac;
using Gsafety.PTMS.Media.MediaManager;

namespace Gsafety.PTMS.Media.Platform.Silverlight
{
    public class RtspStreamMediaManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RtspStreamMediaManager>().As<IMediaManager>().InstancePerMatchingLifetimeScope("builder-scope");
        }
    }
}
