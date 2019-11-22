using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Gsafety.PTMS.Media.Metadata
{
    /// <summary>
    ///     Provide access to the track, segment, and stream metadata.  Do not block in
    ///     the implementation for any of these methods.
    /// </summary>
    public interface IMetadataSink
    {
        void ReportStreamMetadata(TimeSpan timestamp, IStreamMetadata streamMetadata);
        void ReportSegmentMetadata(TimeSpan timestamp, ISegmentMetadata segmentMetadata);
        void ReportTrackMetadata(ITrackMetadata trackMetadata);
        void ReportConfigurationMetadata(IConfigurationMetadata configurationMetadata);
    }

    public class MetadataState
    {
        public IConfigurationMetadata ConfigurationMetadata { get; set; }
        public ISegmentMetadata SegmentMetadata { get; set; }
        public TimeSpan SegmentTimestamp { get; set; }

        public IStreamMetadata StreamMetadata { get; set; }
        public TimeSpan StreamTimestamp { get; set; }

        public ITrackMetadata TrackMetadata { get; set; }
    }

    public class MetadataSink : IMetadataSink
    {
        readonly object _lock = new object();
        readonly MetadataState _metadataState = new MetadataState();
        readonly Queue<ITrackMetadata> _pendingTracks = new Queue<ITrackMetadata>();
        TimeSpan _position;

        public virtual void ReportStreamMetadata(TimeSpan timestamp, IStreamMetadata streamMetadata)
        {
            LoggerInstance.Debug("MetadataSink.ReportStreamMetadata() " + timestamp + " " + streamMetadata);

            lock (_lock)
            {
                _metadataState.StreamTimestamp = timestamp;
                _metadataState.StreamMetadata = streamMetadata;
                _metadataState.SegmentMetadata = null;
                _metadataState.TrackMetadata = null;
            }
        }

        public virtual void ReportSegmentMetadata(TimeSpan timestamp, ISegmentMetadata segmentMetadata)
        {
            LoggerInstance.Debug("MetadataSink.ReportSegmentMetadata() " + timestamp + " " + segmentMetadata);

            lock (_lock)
            {
                _metadataState.SegmentTimestamp = timestamp;
                _metadataState.SegmentMetadata = segmentMetadata;
            }
        }

        public virtual void ReportTrackMetadata(ITrackMetadata trackMetadata)
        {
            if (trackMetadata == null)
                throw new ArgumentNullException("trackMetadata");

            LoggerInstance.Debug("MetadataSink.ReportTrackMetadata() " + trackMetadata);

            if (!trackMetadata.TimeStamp.HasValue)
                throw new ArgumentException("A timestamp is required", "trackMetadata");
            if (trackMetadata.TimeStamp.Value < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("trackMetadata", "The timestamp cannot be negative");

            lock (_lock)
            {
                _pendingTracks.Enqueue(trackMetadata);
            }
        }

        public virtual void ReportConfigurationMetadata(IConfigurationMetadata configurationMetadata)
        {
            LoggerInstance.Debug("MetadataSink.ReportMediaStreamMetadata() " + configurationMetadata);

            lock (_lock)
            {
                _metadataState.ConfigurationMetadata = configurationMetadata;
            }
        }

        TimeSpan? ProcessPendingTracks()
        {
            while (_pendingTracks.Count > 0)
            {
                if (null == _metadataState.TrackMetadata)
                {
                    var track = _pendingTracks.Dequeue();

                    _metadataState.TrackMetadata = track;

                    Debug.Assert(track.TimeStamp.HasValue, "Invalid track metadata (no timestamp)");

                    continue;
                }

                var timeStamp = _metadataState.TrackMetadata.TimeStamp;

                if (timeStamp > _position)
                    return timeStamp;

                var pendingTrack = _pendingTracks.Peek();

                Debug.Assert(pendingTrack.TimeStamp.HasValue, "Invalid track metadata (no timestamp)");

                if (pendingTrack.TimeStamp > _position)
                    return pendingTrack.TimeStamp;

                var dequeueTrack = _pendingTracks.Dequeue();

                Debug.Assert(ReferenceEquals(pendingTrack, dequeueTrack), "Dequeue track mismatch");

                _metadataState.TrackMetadata = pendingTrack;
            }

            if (null != _metadataState.TrackMetadata)
            {
                var timeStamp = _metadataState.TrackMetadata.TimeStamp;

                Debug.Assert(timeStamp.HasValue, "Invalid track metadata (no timestamp)");

                if (timeStamp > _position)
                    return timeStamp;
            }

            return null;
        }

        public virtual void Reset()
        {
            lock (_lock)
            {
                _position = TimeSpan.Zero;

                _metadataState.StreamMetadata = null;
                _metadataState.StreamTimestamp = TimeSpan.Zero;
                _metadataState.SegmentMetadata = null;
                _metadataState.SegmentTimestamp = TimeSpan.Zero;
                _metadataState.TrackMetadata = null;

                _pendingTracks.Clear();
            }
        }

        public virtual TimeSpan? Update(MetadataState state, TimeSpan position)
        {
            lock (_lock)
            {
                _position = position;

                var nextEvent = ProcessPendingTracks();

                var streamMetadata = _metadataState.StreamMetadata;

                var segmentMetadata = _metadataState.SegmentMetadata;

                state.SegmentMetadata = segmentMetadata;
                state.SegmentTimestamp = _metadataState.SegmentTimestamp;
                state.StreamMetadata = streamMetadata;
                state.StreamTimestamp = _metadataState.StreamTimestamp;
                state.TrackMetadata = _metadataState.TrackMetadata;
                state.ConfigurationMetadata = _metadataState.ConfigurationMetadata;

                if (_metadataState.StreamTimestamp > _position && _metadataState.StreamTimestamp < nextEvent)
                    nextEvent = _metadataState.StreamTimestamp;

                if (_metadataState.SegmentTimestamp > _position && _metadataState.SegmentTimestamp < nextEvent)
                    nextEvent = _metadataState.SegmentTimestamp;

                return nextEvent;
            }
        }
    }

    public static class MetadataSinkExtensions
    {
        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IMetadataSink metadataSink)
        {
            mediaStreamFacade.Builder.RegisterSingleton(metadataSink);
        }
    }
}
