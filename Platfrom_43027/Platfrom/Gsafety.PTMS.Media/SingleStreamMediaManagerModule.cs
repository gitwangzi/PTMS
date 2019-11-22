using Autofac;
using Gsafety.PTMS.Media.MediaManager;

namespace Gsafety.PTMS.Media
{
    public class SingleStreamMediaManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SingleStreamMediaManager>().As<IMediaManager>().InstancePerMatchingLifetimeScope("builder-scope");
        }
    }
}
