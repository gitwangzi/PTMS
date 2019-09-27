using Autofac.Core;
using Gsafety.PTMS.Media.Builder;
using Gsafety.PTMS.Media.MediaManager;
using Gsafety.PTMS.Media.Platform.Silverlight;

namespace Gsafety.PTMS.Media
{
    sealed class TsMediaManagerBuilder : BuilderBase<IMediaManager>
    {
        static readonly IModule[] Modules =
        {
            new TsParserModule(),
            new TsMediaModule()
        };

        public TsMediaManagerBuilder(bool useRtspStreamMediaManager, bool useHttpConnection = false)
            : base(Modules)
        {
            if (useRtspStreamMediaManager)
            {
                this.RegisterModule<RtspStreamMediaManagerModule>();
            }
            else
            {
                this.RegisterModule<SingleStreamMediaManagerModule>();

                if (useHttpConnection)
                    this.RegisterModule<HttpConnectionModule>();
                else
                    this.RegisterModule<SilverlightWebRequestModule>();
            }
        }
    }
}
