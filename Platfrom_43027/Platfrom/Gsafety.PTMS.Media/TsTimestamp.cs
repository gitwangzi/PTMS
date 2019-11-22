using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public sealed class TsTimestamp : ITsTimestamp
    {
        public static bool EnableDiscontinutityFilter = true;
        static readonly TimeSpan MaximumError = TimeSpan.FromMilliseconds(5);

        readonly List<PacketsState> _packetsStates = new List<PacketsState>();
        TimeSpan? _timestampOffset;


        public TimeSpan StartPosition { get; set; }

        public TimeSpan? Offset
        {
            get { return _timestampOffset; }
        }

        public void Flush()
        {
            _timestampOffset = null;
        }

        public bool ProcessPackets()
        {
            if (_packetsStates.Count <= 0)
                return false;

            //if (_packetsStates.Any(ps => ps.Packets.Count <= 0))
            //    return false;

            var enableDiscontinutityFilter = EnableDiscontinutityFilter;

            if (!_timestampOffset.HasValue)
            {
                var minPts = TimeSpan.MaxValue;
                var minDts = TimeSpan.MaxValue;

                foreach (var state in _packetsStates)
                {
                    var validData = false;

                    if (!state.IsMedia)
                        continue;

                    foreach (var packet in state.Packets)
                    {
                        if (null == packet)
                            continue;

                        validData = true;

                        if (packet.PresentationTimestamp < minPts)
                            minPts = packet.PresentationTimestamp;
                        if (packet.DecodeTimestamp < minDts && packet.DecodeTimestamp.HasValue)
                            minDts = packet.DecodeTimestamp.Value;
                    }

                    // We need packets from all the streams before we can
                    // determine the proper offset.
                    if (!validData)
                        return false;
                }

                var minTimestamp = minPts;

                if (minDts < minTimestamp)
                    minTimestamp = minDts;

                _timestampOffset = minTimestamp - StartPosition;

                //LoggerInstance.Debug("TsTimestamp.ProcessPackets() syncing pts {0} dts {1} target {2} => offset {3}",
                //    minPts, minDts, StartPosition, _timestampOffset);
            }
            else if (enableDiscontinutityFilter)
            {
                foreach (var state in _packetsStates)
                {
                    if (state.Packets.Count <= 0)
                        continue;

                    if (!state.Duration.HasValue || !state.PresentationTimestamp.HasValue)
                        continue;

                    var packet = state.Packets.First();

                    if (null == packet)
                        continue;

                    var actualPts = packet.PresentationTimestamp - _timestampOffset.Value;

                    var expectedPts = state.PresentationTimestamp.Value + state.Duration.Value;

                    var error = actualPts - expectedPts;

                    if (error < MaximumError && error > -MaximumError)
                        continue;

                    var timestampOffset = packet.PresentationTimestamp - expectedPts;

                    //LoggerInstance.Debug("TsTimestamp.ProcessPackets() resyncing expected pts {0} actual pts {1} target {2} => offset {3} (was {4})",
                    //    expectedPts, actualPts, StartPosition, timestampOffset, _timestampOffset);

                    _timestampOffset = timestampOffset;
                }
            }

            if (_timestampOffset != TimeSpan.Zero)
                AdjustTimestamps(_timestampOffset.Value);

            if (enableDiscontinutityFilter)
            {
                foreach (var state in _packetsStates)
                {
                    if (state.Packets.Count <= 0)
                        continue;

                    var packet = state.Packets.Last();

                    if (null == packet)
                        continue;

                    state.PresentationTimestamp = packet.PresentationTimestamp;
                    state.DecodeTimestamp = packet.DecodeTimestamp;

                    if (packet.Duration.HasValue)
                        state.Duration = packet.Duration;
                    else if (null != state.GetDuration)
                        state.Duration = state.GetDuration(packet);
                    else
                        state.Duration = null;
                }
            }

            return true;
        }

        public void RegisterMediaStream(MediaStream mediaStream, Func<TsPesPacket, TimeSpan?> getDuration)
        {
            _packetsStates.Add(new PacketsState
            {
                Packets = mediaStream.Packets,
                GetDuration = getDuration,
                IsMedia = null != mediaStream.ConfigurationSource
            });
        }

        void AdjustTimestamps(TimeSpan offset)
        {
            foreach (var state in _packetsStates)
            {
                foreach (var packet in state.Packets)
                {
                    if (null == packet)
                        continue;

                    packet.PresentationTimestamp -= offset;

                    if (packet.DecodeTimestamp.HasValue)
                        packet.DecodeTimestamp -= offset;
                }
            }
        }

        class PacketsState
        {
            public TimeSpan? DecodeTimestamp;
            public TimeSpan? Duration;
            public Func<TsPesPacket, TimeSpan?> GetDuration;
            public bool IsMedia;
            public ICollection<TsPesPacket> Packets;
            public TimeSpan? PresentationTimestamp;
        }
    }
}
