using System.Collections.Generic;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public interface IProgramStream
    {
        uint Pid { get; }
        TsStreamType StreamType { get; }
        bool BlockStream { get; set; }
        string Language { get; }
    }

    public interface IProgramStreams
    {
        int ProgramNumber { get; }
        string Language { get; }
        ICollection<IProgramStream> Streams { get; }
    }

    class ProgramStreams : IProgramStreams
    {

        public string Language { get; set; }

        public int ProgramNumber { get; internal set; }

        public ICollection<IProgramStream> Streams { get; internal set; }

        public class ProgramStream : IProgramStream
        {
            public string Language { get; set; }

            public uint Pid { get; internal set; }

            public TsStreamType StreamType { get; internal set; }

            public bool BlockStream { get; set; }
        }
    }
}
