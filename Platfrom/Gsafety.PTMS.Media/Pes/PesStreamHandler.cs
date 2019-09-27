using System;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Pes
{
    public abstract class PesStreamHandler : IPesStreamHandler
    {
        protected readonly uint Pid;
        protected readonly TsStreamType StreamType;

        protected PesStreamHandler(PesStreamParameters parameters)
        {
            if (null == parameters)
                throw new ArgumentNullException("parameters");

            StreamType = parameters.StreamType;
            Pid = parameters.Pid;
        }

        public abstract IConfigurationSource Configurator { get; }


        public virtual void PacketHandler(TsPesPacket packet)
        {
            return;

#pragma warning disable 162
            if (null == packet)
                LoggerInstance.Debug("PES {0}/{1} End-of-stream", StreamType.Contents, Pid);
            else
            {
#if DEBUG
                LoggerInstance.Debug("PES({0}) {1}/{2} PTS {3} Length {4}",
                    packet.PacketId,
                    StreamType.Contents, Pid,
                    packet.PresentationTimestamp, packet.Length);
#endif
            }
#pragma warning restore 162
        }


        public virtual TimeSpan? GetDuration(TsPesPacket packet)
        {
            return packet.Duration;
        }
    }
}
