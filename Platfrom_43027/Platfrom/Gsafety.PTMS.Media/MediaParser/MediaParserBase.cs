using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.MediaParser
{
    public abstract class MediaParserBase<TConfigurator> : IMediaParser
        where TConfigurator : IConfigurationSource
    {
        readonly TConfigurator _configurator;
        readonly TsStreamType _streamType;
        readonly ITsPesPacketPool _tsPesPacketPool;
        IBufferingManager _bufferingManager;
        int _isDisposed;
        MediaStream _mediaStream;
        ICollection<IMediaParserMediaStream> _mediaStreams;
        IStreamBuffer _streamBuffer;

        protected MediaParserBase(TsStreamType streamType, TConfigurator configurator, ITsPesPacketPool tsPesPacketPool)
        {
            if (null == streamType)
                throw new ArgumentNullException("streamType");
            if (ReferenceEquals(default(TConfigurator), configurator))
                throw new ArgumentNullException("configurator");
            if (null == tsPesPacketPool)
                throw new ArgumentNullException("tsPesPacketPool");

            _streamType = streamType;
            _configurator = configurator;
            _tsPesPacketPool = tsPesPacketPool;

            _configurator.ConfigurationComplete += OnConfigurationComplete;
        }

        protected TConfigurator Configurator
        {
            get { return _configurator; }
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 1))
                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public ICollection<IMediaParserMediaStream> MediaStreams
        {
            get { return _mediaStreams; }
        }

        public bool EnableProcessing { get; set; }
        public virtual TimeSpan StartPosition { get; set; }
        public event EventHandler ConfigurationComplete;

        public virtual void ProcessEndOfData()
        {
            FlushBuffers();

            SubmitPacket(null);

            _mediaStream.PushPackets();

            _bufferingManager.ReportEndOfData();
        }

        public abstract void ProcessData(byte[] buffer, int offset, int length);

        public virtual void FlushBuffers()
        {
            _mediaStream.Flush();
        }

        public void Initialize(IBufferingManager bufferingManager, Action<IProgramStreams> programStreamsHandler = null)
        {
            if (null == bufferingManager)
                throw new ArgumentNullException("bufferingManager");

            _bufferingManager = bufferingManager;

            _streamBuffer = bufferingManager.CreateStreamBuffer(_streamType);

            _mediaStream = new MediaStream(_configurator, _streamBuffer, _tsPesPacketPool.FreePesPacket);

            _mediaStreams = new[] { _mediaStream };
        }

        public abstract void InitializeStream(IStreamMetadata streamMetadata);
        public abstract void StartSegment(ISegmentMetadata segmentMetadata);
        public abstract void SetTrackMetadata(ITrackMetadata trackMetadata);

        protected virtual bool PushStreams()
        {
            if (!_mediaStream.PushPackets())
                return false;

            _bufferingManager.Refresh();

            return true;
        }

        void OnConfigurationComplete(object sender, EventArgs eventArgs)
        {
            _configurator.ConfigurationComplete -= OnConfigurationComplete;

            var occ = ConfigurationComplete;

            if (null == occ)
                return;

            occ(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (!Equals(default(TConfigurator), _configurator))
                _configurator.ConfigurationComplete -= OnConfigurationComplete;

            if (null != ConfigurationComplete)
            {
                LoggerInstance.Debug("MediaParserBase<>.Dispose(bool) ConfigurationComplete event is still subscribed");
                ConfigurationComplete = null;
            }

            using (_streamBuffer)
            { }

            using (_mediaStream)
            { }

            _mediaStreams = null;
            _mediaStream = null;
            _bufferingManager = null;
            _streamBuffer = null;
        }

        protected void SubmitPacket(TsPesPacket packet)
        {
            _mediaStream.EnqueuePacket(packet);
        }
    }
}
