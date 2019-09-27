using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.TransportStream
{
    public sealed class TsMediaParser : IMediaParser
    {
        static readonly MediaStream[] NoMediaStreams = new MediaStream[0];
        readonly IBufferPool _bufferPool;
        readonly IPesHandlers _pesHandlers;
        readonly ITsDecoder _tsDecoder;
        readonly ITsPesPacketPool _tsPesPacketPool;
        readonly ITsTimestamp _tsTimemestamp;
        IBufferingManager _bufferingManager;
        MediaStream[] _mediaStreams = NoMediaStreams;
        int? _streamCount;

        public TsMediaParser(ITsDecoder tsDecoder, ITsPesPacketPool tsPesPacketPool, IBufferPool bufferPool, ITsTimestamp tsTimemestamp, IPesHandlers pesHandlers)
        {
            if (null == tsDecoder)
                throw new ArgumentNullException("tsDecoder");
            if (null == tsPesPacketPool)
                throw new ArgumentNullException("tsPesPacketPool");
            if (null == bufferPool)
                throw new ArgumentNullException("bufferPool");
            if (null == tsTimemestamp)
                throw new ArgumentNullException("tsTimemestamp");
            if (null == pesHandlers)
                throw new ArgumentNullException("pesHandlers");

            _tsPesPacketPool = tsPesPacketPool;
            _bufferPool = bufferPool;
            _tsDecoder = tsDecoder;
            _tsTimemestamp = tsTimemestamp;
            _pesHandlers = pesHandlers;
        }

        public ICollection<IMediaParserMediaStream> MediaStreams
        {
            get { return _mediaStreams.ToArray(); }
        }

        public TimeSpan StartPosition
        {
            get { return _tsTimemestamp.StartPosition; }
            set { _tsTimemestamp.StartPosition = value; }
        }

        public event EventHandler ConfigurationComplete;

        public bool EnableProcessing
        {
            get { return _tsDecoder.EnableProcessing; }
            set { _tsDecoder.EnableProcessing = value; }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            DisposeStreams();
        }

        public void Initialize(IBufferingManager bufferingManager, Action<IProgramStreams> programStreamsHandler = null)
        {
            if (null == bufferingManager)
                throw new ArgumentNullException("bufferingManager");

            _bufferingManager = bufferingManager;

            var handler = programStreamsHandler ?? DefaultProgramStreamsHandler;

            programStreamsHandler = pss =>
            {
                handler(pss);

                _streamCount = pss.Streams.Count(s => !s.BlockStream);
            };

            _tsDecoder.Initialize(CreatePacketizedElementaryStream, programStreamsHandler);
        }

        public void InitializeStream(IStreamMetadata streamMetadata)
        { }

        public void StartSegment(ISegmentMetadata segmentMetadata)
        { }

        public void SetTrackMetadata(ITrackMetadata trackMetadata)
        { }

        public void FlushBuffers()
        {
            _tsDecoder.FlushBuffers();

            foreach (var ms in _mediaStreams)
                ms.Flush();

            _tsTimemestamp.Flush();
        }

        public void ProcessEndOfData()
        {
            _tsDecoder.ParseEnd();

            PushStreams(true);

            _bufferingManager.ReportEndOfData();
        }

        public void ProcessData(byte[] buffer, int offset, int length)
        {
            _tsDecoder.Parse(buffer, offset, length);

            if (PushStreams(false))
                _bufferingManager.Refresh();
        }

        bool PushStreams(bool force)
        {
            if (!_tsTimemestamp.ProcessPackets() && !force)
                return false;

            var newPackets = false;

            foreach (var mediaStream in _mediaStreams)
            {
                if (mediaStream.PushPackets())
                    newPackets = true;
            }

            return newPackets;
        }

        static void DefaultProgramStreamsHandler(IProgramStreams pss)
        {
            var hasAudio = false;
            var hasVideo = false;

            foreach (var stream in pss.Streams)
            {
                switch (stream.StreamType.Contents)
                {
                    case TsStreamType.StreamContents.Audio:
                        if (hasAudio)
                            stream.BlockStream = true;
                        else
                            hasAudio = true;
                        break;
                    case TsStreamType.StreamContents.Video:
                        if (hasVideo)
                            stream.BlockStream = true;
                        else
                            hasVideo = true;
                        break;
                    default:
                        stream.BlockStream = true;
                        break;
                }
            }
        }

        void DisposeStreams()
        {
            var mediaStreams = Interlocked.Exchange(ref _mediaStreams, NoMediaStreams);

            if (mediaStreams.Length <= 0)
                return;

            foreach (var ms in mediaStreams)
                ms.Dispose();
        }

        void AddMediaStream(MediaStream mediaStream)
        {
            var mediaStreams = _mediaStreams;
            var newMediaStreams = new MediaStream[_mediaStreams.Length + 1];

            for (; ; )
            {
                if (newMediaStreams.Length != mediaStreams.Length)
                    newMediaStreams = new MediaStream[_mediaStreams.Length + 1];

                Array.Copy(mediaStreams, newMediaStreams, mediaStreams.Length);
                newMediaStreams[newMediaStreams.Length - 1] = mediaStream;

                var oldMediaStream = Interlocked.CompareExchange(ref _mediaStreams, newMediaStreams, mediaStreams);

                if (ReferenceEquals(oldMediaStream, mediaStreams))
                    break;

                mediaStreams = oldMediaStream;
            }

            CheckConfigurationComplete();
        }

        void CheckConfigurationComplete()
        {
            var streams = MediaStreams;

            if (!_streamCount.HasValue || _streamCount.Value != streams.Count)
                return;

            if (streams.Any(stream => null != stream.ConfigurationSource && !stream.ConfigurationSource.IsConfigured))
                return;

            FireConfigurationComplete();
        }

        void FireConfigurationComplete()
        {
            var cc = ConfigurationComplete;

            if (null == cc)
                return;

            ConfigurationComplete = null;

            var task = TaskExt.Run(() => cc(this, EventArgs.Empty));

            TaskCollector.Default.Add(task, "TsMediaParser.FireConfigurationComplete()");
        }

        TsPacketizedElementaryStream CreatePacketizedElementaryStream(TsStreamType streamType, uint pid, IMediaStreamMetadata mediaStreamMetadata)
        {
            var streamBuffer = _bufferingManager.CreateStreamBuffer(streamType);

            MediaStream mediaStream = null;

            var pesStreamHandler = _pesHandlers.GetPesHandler(streamType, pid, mediaStreamMetadata, packet =>
            {
                if (null != mediaStream)
                    mediaStream.EnqueuePacket(packet);
                else if (null != packet)
                    _tsPesPacketPool.FreePesPacket(packet);
            });

            var pes = new TsPacketizedElementaryStream(_bufferPool, _tsPesPacketPool, pesStreamHandler.PacketHandler, streamType, pid);

            var configurator = pesStreamHandler.Configurator;

            if (null != configurator)
            {
                EventHandler configuratorOnConfigurationComplete = null;

                configuratorOnConfigurationComplete = (o, e) =>
                {
                    configurator.ConfigurationComplete -= configuratorOnConfigurationComplete;

                    CheckConfigurationComplete();
                };

                configurator.ConfigurationComplete += configuratorOnConfigurationComplete;
            }

            mediaStream = new MediaStream(configurator, streamBuffer, _tsPesPacketPool.FreePesPacket);

            AddMediaStream(mediaStream);

            _tsTimemestamp.RegisterMediaStream(mediaStream, pesStreamHandler.GetDuration);

            if (null == configurator)
                CheckConfigurationComplete();

            return pes;
        }
    }
}
