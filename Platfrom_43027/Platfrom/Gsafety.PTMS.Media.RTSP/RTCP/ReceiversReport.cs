using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.Media.RTSP.Common;

namespace Gsafety.PTMS.Media.RTSP.RTCP
{
    /// <summary>
    /// Provides a managed implemenation of the ReceiversReport defined in http://tools.ietf.org/html/rfc3550#section-6.4.2
    /// </summary>
    public class ReceiversReport : RtcpReport
    {
        #region Constants and Statics

        new public const int PayloadType = 201;

        #endregion

        #region Constructor

        public ReceiversReport(int version, int padding, int reportBlocks, int ssrc)
            : base(version, PayloadType, padding, ssrc, reportBlocks, ReportBlock.ReportBlockSize) { }

        public ReceiversReport(int version, int reportBlocks, int ssrc)
            : base(version, PayloadType, 0, ssrc, reportBlocks, ReportBlock.ReportBlockSize)
        {
        }

        public ReceiversReport(RtcpPacket reference, bool shouldDispose = true)
            : base(reference.Header, reference.Payload, shouldDispose)
        {
            if (Header.PayloadType != PayloadType) throw new ArgumentException("Header.PayloadType is not equal to the expected type of 201.", "reference");
        }

        /// <summary>
        /// Constructs a new SendersReport from the given <see cref="RtcpHeader"/> and payload.
        /// Changes to the header are immediately reflected in this instance.
        /// Changes to the payload are not immediately reflected in this instance.
        /// </summary>
        /// <param name="header">The header</param>
        /// <param name="payload">The payload</param>
        public ReceiversReport(RtcpHeader header, System.Collections.Generic.IEnumerable<byte> payload, bool shouldDispose = true)
            : base(header, payload, shouldDispose)
        {
            if (Header.PayloadType != PayloadType) throw new ArgumentException("Header.PayloadType is not equal to the expected type of 201.", "reference");
        }

        /// <summary>
        /// Constructs a new SendersReport from the given <see cref="RtcpHeader"/> and payload.
        /// Changes to the header and payload are immediately reflected in this instance.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="payload"></param>
        public ReceiversReport(RtcpHeader header, Common.MemorySegment payload, bool shouldDipose = true)
            : base(header, payload, shouldDipose)
        {
            if (Header.PayloadType != PayloadType) throw new ArgumentException("Header.PayloadType is not equal to the expected type of 201.", "reference");
        }
        #endregion

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("ReceiversReport : \n");
            builder.AppendLine(base.ToString());

            foreach (var item in this)
            {
                builder.AppendLine(item.ToString());
            }
            return builder.ToString();
        }
    }
}