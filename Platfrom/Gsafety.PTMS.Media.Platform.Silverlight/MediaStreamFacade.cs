using System.Windows.Media;
using Gsafety.PTMS.Media.Builder;
using Gsafety.PTMS.Media.MediaManager;

namespace Gsafety.PTMS.Media
{
    public interface IMediaStreamFacade : IMediaStreamFacadeBase<MediaStreamSource>
    { }

    public class MediaStreamFacade : MediaStreamFacadeBase<MediaStreamSource>, IMediaStreamFacade
    {
        public MediaStreamFacade(IBuilder<IMediaManager> builder = null)
            : base(builder ?? new TsMediaManagerBuilder(MediaStreamFacadeSettings.Parameters.UseRtspStreamMediaManager, MediaStreamFacadeSettings.Parameters.UseHttpConnection))
        { }
    }
}
