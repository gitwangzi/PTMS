using System;

namespace Gsafety.PTMS.Media.Metadata
{
    public class ActionMetadataSink : MetadataSink
    {
        readonly Action _updateAction;

        public ActionMetadataSink(Action updateAction)
        {
            if (null == updateAction)
                throw new ArgumentNullException("updateAction");

            _updateAction = updateAction;
        }

        public override void Reset()
        {
            base.Reset();

            _updateAction();
        }

        public override void ReportStreamMetadata(TimeSpan timestamp, IStreamMetadata streamMetadata)
        {
            base.ReportStreamMetadata(timestamp, streamMetadata);

            _updateAction();
        }

        public override void ReportSegmentMetadata(TimeSpan timestamp, ISegmentMetadata segmentMetadata)
        {
            base.ReportSegmentMetadata(timestamp, segmentMetadata);

            _updateAction();
        }

        public override void ReportTrackMetadata(ITrackMetadata trackMetadata)
        {
            base.ReportTrackMetadata(trackMetadata);

            _updateAction();
        }

        public override void ReportConfigurationMetadata(IConfigurationMetadata configurationMetadata)
        {
            base.ReportConfigurationMetadata(configurationMetadata);

            _updateAction();
        }
    }
}
