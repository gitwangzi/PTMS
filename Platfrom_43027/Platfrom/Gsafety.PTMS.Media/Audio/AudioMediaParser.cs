using System;
using System.Diagnostics;
using Gsafety.PTMS.Media.Audio.Shoutcast;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.Audio
{
    public abstract class AudioMediaParser<TParser, TConfigurator> : MediaParserBase<TConfigurator>
        where TParser : class, IAudioParser
        where TConfigurator : IConfigurationSource
    {
        readonly IMetadataSink _metadataSink;
        readonly IShoutcastMetadataFilterFactory _shoutcastMetadataFilterFactory;
        protected TParser Parser;
        IAudioParser _audioParser;

        protected AudioMediaParser(TsStreamType streamType, TConfigurator configurator, ITsPesPacketPool pesPacketPool,
            IShoutcastMetadataFilterFactory shoutcastMetadataFilterFactory, IMetadataSink metadataSink)
            : base(streamType, configurator, pesPacketPool)
        {
            if (null == shoutcastMetadataFilterFactory)
                throw new ArgumentNullException("shoutcastMetadataFilterFactory");
            if (null == metadataSink)
                throw new ArgumentNullException("metadataSink");

            _shoutcastMetadataFilterFactory = shoutcastMetadataFilterFactory;
            _metadataSink = metadataSink;
        }

        public override TimeSpan StartPosition
        {
            get { return Parser.StartPosition; }
            set { Parser.StartPosition = value; }
        }

        public override void InitializeStream(IStreamMetadata streamMetadata)
        {
            _metadataSink.ReportStreamMetadata(Parser.Position ?? TimeSpan.Zero, streamMetadata);
        }

        public override void StartSegment(ISegmentMetadata segmentMetadata)
        {
            _audioParser = Parser;

            var shoutcastMetadata = segmentMetadata as IShoutcastSegmentMetadata;

            if (null != shoutcastMetadata)
            {
                var icyMetaInt = shoutcastMetadata.IcyMetaInt;

                if (icyMetaInt.HasValue && icyMetaInt > 0)
                    _audioParser = _shoutcastMetadataFilterFactory.Create(segmentMetadata, Parser, SetTrackMetadata, icyMetaInt.Value);
            }

            _metadataSink.ReportSegmentMetadata(Parser.Position ?? TimeSpan.Zero, segmentMetadata);
        }

        public override void SetTrackMetadata(ITrackMetadata trackMetadata)
        {
            _metadataSink.ReportTrackMetadata(trackMetadata);
        }

        public override void ProcessData(byte[] buffer, int offset, int length)
        {
            Debug.Assert(length > 0);
            Debug.Assert(offset + length <= buffer.Length);

            if (null == _audioParser)
                throw new InvalidOperationException("StartSegment has not been called");

            _audioParser.ProcessData(buffer, offset, length);

            PushStreams();
        }

        public override void FlushBuffers()
        {
            if (null != _audioParser)
                _audioParser.FlushBuffers();

            base.FlushBuffers();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (_audioParser)
                { }
            }

            base.Dispose(disposing);
        }
    }
}
