using System;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.MediaManager
{
    public interface IMediaManagerParameters
    {
        Action<IProgramStreams> ProgramStreamsHandler { get; set; }
    }

    public class MediaManagerParameters : IMediaManagerParameters
    {
        public Action<IProgramStreams> ProgramStreamsHandler { get; set; }
    }
}
