﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.Media.RTSP.Common;

namespace Gsafety.PTMS.Media.RTSP.RTCP
{
    /// <summary>
    /// Represents the binary data contained in the <see cref="Rtcp.SendersReport"/> and <see cref="Rtcp.ReceiversReport"/>.
    /// </summary>
    /// <remarks>
    /// ReportBlock is a fixed sized structure which must always contain 24 octets.
    /// </remarks>
    public class ReportBlock : SuppressedFinalizerDisposable, IReportBlock, IEnumerable<byte>
    {
        #region Constants and Statics

        // private const int MIN_PACKETS_LOST = -8388607; // 0xFF800001
        // private const int MAX_PACKETS_LOST = 8388607; // 0x007FFFFF 

        public const int ReportBlockSize = 24;

        #endregion

        #region Fields

        readonly MemorySegment Memory = MemorySegment.Empty;

        readonly byte[] m_OwnedOctets;

        #endregion

        #region Properties

        public virtual bool IsComplete
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return false == IsDisposed && Memory.Count >= ReportBlockSize; }
        }

        /// <summary>
        /// The size in octets of this ReportBlock instance
        /// </summary>
        public virtual int Size
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return IsDisposed ? 0 : Binary.Clamp(Memory.Count, 0, ReportBlockSize); }
        }//ReportBlockSize

        /// <summary>
        /// The identifier or identity to which this ReportBlock corresponds to.
        /// </summary>
        public int SendersSynchronizationSourceIdentifier
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return (int)Binary.ReadU32(Memory.Array, Memory.Offset, Binary.IsLittleEndian); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.Write32(Memory.Array, Memory.Offset, Binary.IsLittleEndian, (uint)value); }
        }

        /// <summary>
        /// The fractions of <see cref="Rtp.RtpPacket"/>'s from the <see cref="ReportBlock.SendersSynchronizationSourceIdentifier"/> indicated that have been lost since the begining of reception.
        /// <see cref="http://tools.ietf.org/html/rfc3550#appendix-A.3"> for more information </see>
        /// </summary>
        public byte FractionsLost
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return Binary.ReadU8(Memory.Array, Memory.Offset + 4, false); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.WriteU8(Memory.Array, Memory.Offset + 4, false, value); }
        }

        /// <summary>
        /// The total number of <see cref="Rtp.RtpPacket"/>'s from the <see cref="ReportBlock.SendersSynchronizationSourceIdentifier"/> indicated that have been lost since the begining of reception.
        /// <see cref="http://tools.ietf.org/html/rfc3550#appendix-A.3"> for more information </see>
        /// </summary>
        public int CumulativePacketsLost
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            //Read 32 at offset 4 and shift left 4
            get { return (int)Binary.ReadU24(Memory.Array, Memory.Offset + 5, Binary.IsLittleEndian); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.Write24(Memory.Array, Memory.Offset + 5, Binary.IsLittleEndian, (uint)value); }
        }

        /// <summary>
        /// The highest <see cref="Rtp.RtpPacket.SeqeuenceNumber"> SequenceNumber</see> received from the <see cref="ReportBlock.SendersSynchronizationSourceIdentifier"/> indicated.
        /// <see cref="http://tools.ietf.org/html/rfc3550#appendix-A.3"> for more information </see>
        /// </summary>
        public int ExtendedHighestSequenceNumberReceived
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return (int)Binary.ReadU32(Memory.Array, Memory.Offset + 8, Binary.IsLittleEndian); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.Write32(Memory.Array, Memory.Offset + 8, Binary.IsLittleEndian, (uint)value); }
        }

        /// <summary>
        /// An estimate of the statistical variance of the <see cref="Rtp.RtpPacket.Created">RtpPacket</see> interarrival time.
        /// </summary>
        public int InterarrivalJitterEstimate
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return (int)Binary.ReadU32(Memory.Array, Memory.Offset + 12, Binary.IsLittleEndian); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.Write32(Memory.Array, Memory.Offset + 12, Binary.IsLittleEndian, (uint)value); }
        }

        /// <summary>
        /// The Middle 32 bits out of 64 in the NtpTimestamp (as explained in <see cref="http://tools.ietf.org/html/rfc3550#section-4">Section 4</see>)
        /// </summary>
        public int LastSendersReportTimestamp
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return (int)Binary.ReadU32(Memory.Array, Memory.Offset + 16, Binary.IsLittleEndian); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.Write32(Memory.Array, Memory.Offset + 16, Binary.IsLittleEndian, (uint)value); }
        }

        /// <summary>
        /// The delay, [expressed in units of 1/65536 seconds,] between receiving the last <see cref="Rtcp.SendersReport"/> packet from source indicated.
        /// </summary>
        public int DelaySinceLastSendersReport
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return (int)Binary.ReadU32(Memory.Array, Memory.Offset + 20, Binary.IsLittleEndian); }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            protected set { Binary.Write32(Memory.Array, Memory.Offset + 20, Binary.IsLittleEndian, (uint)value); }
        }

        int IReportBlock.BlockIdentifier
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return SendersSynchronizationSourceIdentifier; }
        }

        IEnumerable<byte> IReportBlock.BlockData
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]

            get { return Memory; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Allocates 24 octets to represent this ReportBlock instance.
        /// </summary>
        ReportBlock(bool shouldDispose = true)
            : base(shouldDispose)
        {
            m_OwnedOctets = new byte[ReportBlockSize];

            Memory = new MemorySegment(m_OwnedOctets, 0, ReportBlockSize);
        }

        /// <summary>
        /// Allocates 24 octets of memory and sets the <see cref="ReportBlock.SendersSynchronizationSourceIdentifier"/> property to the value of <paramref name="ssrc"/>.
        /// </summary>
        /// <param name="ssrc">The id of the participant who sent this ReportBlock instance</param>
        public ReportBlock(int ssrc, bool shouldDispose = true) : this(shouldDispose) { SendersSynchronizationSourceIdentifier = ssrc; }

        public ReportBlock(int ssrc, byte fractionsLost) : this(ssrc) { FractionsLost = fractionsLost; }

        public ReportBlock(int ssrc, byte fractionsLost, int cumulativePacketsLost) : this(ssrc, fractionsLost) { CumulativePacketsLost = cumulativePacketsLost; }

        public ReportBlock(int ssrc, byte fractionsLost, int cumulativePacketsLost, int extendedHighestSequenceNumberReceived)
            : this(ssrc, fractionsLost, cumulativePacketsLost) { ExtendedHighestSequenceNumberReceived = extendedHighestSequenceNumberReceived; }

        public ReportBlock(int ssrc, byte fractionsLost, int cumulativePacketsLost, int extendedHighestSequenceNumberReceived, int interarrivalJitterEstimate)
            : this(ssrc, fractionsLost, cumulativePacketsLost, extendedHighestSequenceNumberReceived) { InterarrivalJitterEstimate = interarrivalJitterEstimate; }

        public ReportBlock(int ssrc, byte fractionsLost, int cumulativePacketsLost, int extendedHighestSequenceNumberReceived, int interarrivalJitterEstimate, int lastSendersReportTimestamp)
            : this(ssrc, fractionsLost, cumulativePacketsLost, extendedHighestSequenceNumberReceived, interarrivalJitterEstimate) { LastSendersReportTimestamp = lastSendersReportTimestamp; }

        public ReportBlock(int ssrc, byte fractionsLost, int cumulativePacketsLost, int extendedHighestSequenceNumberReceived, int interarrivalJitterEstimate, int lastSendersReportTimestamp, int delaySinceLastSendersReport) : this(ssrc, fractionsLost, cumulativePacketsLost, extendedHighestSequenceNumberReceived, interarrivalJitterEstimate, lastSendersReportTimestamp) { DelaySinceLastSendersReport = delaySinceLastSendersReport; }

        /// <summary>
        /// Creates a new ReportBlock instance from the given existing reference.
        /// Throws a ArgumentNullException if <paramref name="reference"/> is null.
        /// </summary>
        /// <param name="reference">A reference to a ReportBlock instance.</param>
        public ReportBlock(ReportBlock reference, bool shouldDispose = true)
            : base(shouldDispose)
        {
            if (reference == null) throw new ArgumentNullException();

            Memory = reference.Memory;
        }

        public ReportBlock(MemorySegment data, bool shouldDispose = true)
            : base(shouldDispose)
        {
            Memory = data;
        }

        #endregion

        #region Instance Methods

        public ReportBlock Clone(bool reference)
        {
            if (reference) return new ReportBlock(this);

            return (ReportBlock)this.MemberwiseClone(); //Calls the default constructor and sets each property.
        }

        /// <summary>
        /// Prepares a sequence containig the octets which represent this instance.
        /// </summary>
        /// <returns>The sequence created</returns>
        public virtual IEnumerable<byte> Prepare()
        {
            return Memory.Take(Size);
        }

        #endregion

        #region Implementation Methods

        public IEnumerator<byte> GetEnumeratorImplementation()
        {
            return Prepare().GetEnumerator();
        }

        IEnumerator<byte> IEnumerable<byte>.GetEnumerator()
        {
            return GetEnumeratorImplementation();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumeratorImplementation();
        }

        object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion

        protected internal override void Dispose(bool disposing)
        {
            if (false == disposing || false == ShouldDispose) return;

            base.Dispose(ShouldDispose);

            IDisposable memory = (IDisposable)Memory;

            if (memory != null)
            {
                memory.Dispose();

                memory = null;
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("SSRC : " + this.SendersSynchronizationSourceIdentifier);
            builder.AppendLine("FractionsLost : " + this.FractionsLost);
            builder.AppendLine("CumulativePacketsLost : " + this.CumulativePacketsLost);
            builder.AppendLine("ExtendedHighestSequenceNumberReceived : " + this.ExtendedHighestSequenceNumberReceived);
            builder.AppendLine("InterarrivalJitterEstimate : " + this.InterarrivalJitterEstimate);
            builder.AppendLine("LastSendersReportTimestamp : " + this.LastSendersReportTimestamp);
            builder.AppendLine("DelaySinceLastSendersReport : " + this.DelaySinceLastSendersReport);

            return builder.ToString();
        }
    }
}