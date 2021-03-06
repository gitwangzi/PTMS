﻿#region Copyright
/*
This file came from Managed Media Aggregation, You can always find the latest version @ https://net7mma.codeplex.com/
  
 Julius.Friedman@gmail.com / (SR. Software Engineer ASTI Transportation Inc. http://www.asti-trans.com)

Permission is hereby granted, free of charge, 
 * to any person obtaining a copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, 
 * including without limitation the rights to :
 * use, 
 * copy, 
 * modify, 
 * merge, 
 * publish, 
 * distribute, 
 * sublicense, 
 * and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 * 
 * 
 * JuliusFriedman@gmail.com should be contacted for further details.

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, 
 * ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 * v//
 */
#endregion

#region Using Statements

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gsafety.PTMS.Media.RTSP.Common;
using Gsafety.PTMS.Media.RTSP.Common.Extensions.Array;
using Gsafety.PTMS.Media.RTSP.RTP;

#endregion


namespace Gsafety.PTMS.Media.RTSP.RTCP
{
    #region GoodbyeReport

    /// <summary>
    /// Provides an implementation of the Goodbye Rtcp Packet outlined in http://tools.ietf.org/html/rfc3550#section-6.6
    /// </summary>
    public class GoodbyeReport : RtcpReport
    {

        #region Constants and Statics

        //abstract int AssignedPayloadType
        public new const int PayloadType = 203;

        #endregion

        /// <summary>
        /// For the support of the efficient creation of a GoodbyeReport
        /// </summary>
        /// <param name="version"></param>
        /// <param name="padding"></param>
        /// <param name="ssrc"></param>
        /// <param name="blockCount"></param>
        /// <param name="extensionSize"></param>
        /// <param name="blockSize"></param>
        /// <param name="extensionData"></param>
        internal GoodbyeReport(int version, int padding, int ssrc, ref int blockCount, ref int extensionSize, ref int bytesInSourceList, byte[] extensionData)
            : base(version, PayloadType, padding, ssrc, blockCount, 0, //caulcated via extensionSize 
                2, //lengthInWords because the ssrc is present...
                (ArrayExtensions.IsNullOrEmpty(extensionData, out extensionSize) ? bytesInSourceList : 1 + extensionSize + bytesInSourceList)) //reasonForLeaving
        {
            #region Babble

            /* If I won't have a participant list then I sure as shit won't make thing as they shouldn't be here just for [Wireshark, et al]
             6.6 BYE: Goodbye RTCP Packet

               0                   1                   2                   3
               0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1
              +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
              |V=2|P|    SC   |   PT=BYE=203  |             length            |
              +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
              |                           SSRC/CSRC                           |
              +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
              :                              ...                              : (THIS IS WHERE THE SOURCE LIST GOES FYI)
              +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+
        (opt) |     length    |               reason for leaving            ...
              +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
             
             * When SC = 0 the packet is useless.
             * When SC >= 1 the SourceList appears BEFORE the (opt) Length, The id there may be different from SSRC/CSRC
             * Length is optional and a 0 value should not HAVE to be present as including a 0 value forces you to inlcude 3 more 0's to octet align the payload
             */

            #endregion

            //If there is an extension
            if (extensionSize > 0)
            {
                //Ensure it will fit
                if (extensionSize > byte.MaxValue) throw new InvalidOperationException("Only 255 octets can occupy the ReasonForLeaving in a GoodbyeReport.");

                //The data is placed in the payload, right after the ssrc in the header.
                int offset = (blockCount * Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList.ItemSize) - Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList.ItemSize;

                //The length before the string right after the sourceList
                Payload.Array[Payload.Offset + offset++] = (byte)extensionSize;

                //Copy it to the payload after the length
                extensionData.CopyTo(Payload.Array, Payload.Offset + offset);
            }

            //Finally ensure the LengthInWords is set accordingly.
            SetLengthInWordsMinusOne();
        }

        /// <summary>
        /// For support of the efficient creation of a GoodbyeReport
        /// </summary>
        /// <param name="version"></param>
        /// <param name="padding"></param>
        /// <param name="ssrc"></param>
        /// <param name="blockCount"></param>
        /// <param name="extensionSize"></param>
        /// <param name="extensionData"></param>
        internal GoodbyeReport(int version, int padding, int ssrc, int blockCount, int extensionSize, int bytesInSourceList, byte[] extensionData) // ref
            : this(version, padding, ssrc, ref blockCount, ref extensionSize, ref bytesInSourceList, extensionData)
        {

        }

        /// <summary>
        /// Constructs a new GoodbyeReport from the given values.
        /// </summary>
        /// <param name="version">The version of the report</param>
        /// <param name="padding"></param>
        /// <param name="ssrc">The id of the senders of the report</param>
        /// <param name="sourcesLeaving">The SourceList which describes the sources who are leaving</param>
        /// <param name="reasonForLeaving">An optional reason for leaving(only the first 255 octets will be used)</param>
        internal GoodbyeReport(int version, int padding, int ssrc, Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList sourcesLeaving, byte[] reasonForLeaving)
            : this(version, padding, ssrc,
                sourcesLeaving.Count + 1, //BlockCount, + 1 because the ssrc is present.
                0,// 0 is extensionSize which is assigned by the size of reasonForLeaving in the constructor via ref.
            sourcesLeaving.Count * Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList.ItemSize, //bytesInSourceList
            reasonForLeaving)
        {
            sourcesLeaving.TryCopyTo(m_OwnedOctets, Payload.Offset);
        }

        public GoodbyeReport(int version, int ssrc, Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList sourcesLeaving, byte[] reasonForLeaving)
            : this(version, 0, ssrc, sourcesLeaving == null ? Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList.Empty : sourcesLeaving, reasonForLeaving)
        {

        }

        public GoodbyeReport(int version, int ssrc, byte[] reasonForLeaving) : this(version, ssrc, null, reasonForLeaving) { } //Todo chain?

        /// <summary>
        /// Constructs a new GoodbyeReport from the given values.
        /// </summary>
        /// <param name="version">The version of the report</param>
        /// <param name="ssrc">The id of the senders of the report</param>
        public GoodbyeReport(int version, int ssrc)
            //: base(new RtcpHeader(version, PayloadType, false, 0, ssrc), MemorySegment.Empty) { }
            : base(version, PayloadType, 0, ssrc, 0, 0, RtcpHeader.DefaultLengthInWords, 0) { }

        /// <summary>
        /// Constructs a new GoodbyeReport from the given <see cref="RtcpHeader"/> and payload.
        /// Changes to the header are immediately reflected in this instance.
        /// Changes to the payload are not immediately reflected in this instance.
        /// </summary>
        /// <param name="header">The header</param>
        /// <param name="payload">The payload</param>
        public GoodbyeReport(RtcpHeader header, IEnumerable<byte> payload, bool shouldDipose = true)
            : base(header, payload, shouldDipose)
        {
            if (Header.PayloadType != PayloadType) throw new ArgumentException("Header.PayloadType is not equal to the expected type of 203.", "reference");
            //RtcpReportExtensions.VerifyPayloadType(this);
        }

        /// <summary>
        /// Constructs a new GoodbyeReport from the given <see cref="RtcpHeader"/> and payload.
        /// Changes to the header and payload are immediately reflected in this instance.
        /// </summary>
        /// <param name="header"></param>
        /// <param name="payload"></param>
        public GoodbyeReport(RtcpHeader header, MemorySegment payload, bool shouldDipose = true)
            : base(header, payload, shouldDipose)
        {
            if (Header.PayloadType != PayloadType) throw new ArgumentException("Header.PayloadType is not equal to the expected type of 203.", "reference");
            //RtcpReportExtensions.VerifyPayloadType(this);
        }

        /// <summary>
        /// Constructs a GoodbyeReport instance from an existing RtcpPacket reference.
        /// Throws a ArgumentNullException if reference is null.
        /// Throws an ArgumentException if the <see cref="RtcpHeader.PayloadType"/> is not GoodbyeReport (203)
        /// </summary>
        /// <param name="reference">The packet containing the GoodbyeReport</param>
        public GoodbyeReport(RtcpPacket reference, bool shouldDispose = true)
            : base(reference.Header, reference.Payload, shouldDispose)
        {
            if (Header.PayloadType != PayloadType) throw new ArgumentException("Header.PayloadType is not equal to the expected type of 203.", "reference");
            //RtcpReportExtensions.VerifyPayloadType(this);
        }

        /// <summary>
        /// Gets the amount of octets contained in the Payload which belong to the <see cref="RFC3550.SourceList"/> or <see cref="ReasonForLeavingData"/>
        /// The BlockCount is obtained from the Header.
        /// </summary>
        public override int ReportBlockOctets
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return Binary.Abs(BlockCount - 1) * RFC3550.SourceList.ItemSize;
            }
        }

        /// <summary>
        /// Indicates if the GoodbyeReport contains a ReasonForLeaving based on the length of <see cref="RFC3550.SourceList"/> contained in the GoodbyeReport.
        /// </summary>
        public bool HasReasonForLeaving
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return ReasonForLeavingLength > 0; }
        }

        /// <summary>
        /// Gets the data assoicated with the <see cref="ReasonForLeavingData"/> denoted by the length of field if present.
        /// If no reason for leaving is present then an empty sequence is returned.
        /// </summary>
        public IEnumerable<byte> ReasonForLeavingData
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                if (IsDisposed || false == HasReasonForLeaving) return Enumerable.Empty<byte>();

                return ExtensionData.Skip(1).Take(ReasonForLeavingLength);
            }
        }

        /// <summary>
        /// Returns the length of the <see cref="ReasonForLeavingData"/> field as indicated by the length of the field if present.
        /// If no reason for leaving is present 0 is returned.
        /// </summary>
        public int ReasonForLeavingLength
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get
            {
                return HasExtensionData ? ExtensionData.First() : 0;
            }
        }

        /// <summary>
        /// Calulcates the size of the octets which are given by <see cref="ReasonForLeavingLength"/>, usually not part of the <see cref="ReportData"/>
        /// </summary>
        public override int ExtensionDataOctets
        {
            get
            {
                return ReasonForLeavingLength;
            }
        }

        /// <summary>
        /// Gets the data assoicated with the <see cref="ReasonForLeavingData"/>
        /// </summary>
        public override IEnumerable<byte> ExtensionData
        {
            get
            {
                return Payload.Skip(ReportBlockOctets);
            }
        }

        #region Methods

        /// <summary>
        /// Creates a <see cref="SourceList"/> from the information contained in the GoodbyeReport.
        /// </summary>
        /// <returns>The <see cref="SourceList"/> created.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList GetSourceList() { return new Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList(this); }
        #endregion

        public override void Add(IReportBlock reportBlock)
        {
            //Will throw an InvalidCastException is the given reportBlock is not a RFC3550.SourceList
            if (reportBlock is Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList) Add(reportBlock as Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList);
            else base.Add(reportBlock);
        }

        /// <summary>
        /// Adds the maximum amount of items from the source list as there are availabe blocks
        /// </summary>
        /// <param name="sourceList"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        internal virtual protected void Add(Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList sourceList, int offset, int count)
        {
            if (sourceList == null) return;

            if (IsReadOnly) throw new InvalidOperationException("A RFC3550.SourceList cannot be added when IsReadOnly is true.");

            int reportBlocksRemaining = ReportBlocksRemaining;

            if (reportBlocksRemaining == 0) throw new InvalidOperationException("A RtcpReport can only hold 31 ReportBlocks");

            //Add the bytes to the payload and set the LengthInWordsMinusOne and increase the BlockCount

            //This is not valid when there is a ReasonForLeaving, the data needs to be placed before such reason and padding
            AddBytesToPayload(sourceList.GetBinaryEnumerable(offset, BlockCount += Binary.Min(reportBlocksRemaining, count)));
        }

        public virtual void Add(Gsafety.PTMS.Media.RTSP.RTP.RFC3550.SourceList sourceList)
        {
            Add(sourceList, 0, sourceList.Count);
        }

        public override IEnumerator<IReportBlock> GetEnumerator()
        {
            //CheckDisposed();

            //The first entry is in the header
            using (ReportBlock rb = new ReportBlock(Header.GetSendersSynchronizationSourceIdentifierSegment()))
            {
                yield return rb;
            }//size becomes -.... add shouldDispose = false....

            //If there are no more entries then we can return
            if (Header.BlockCount == 1) yield break;

            //The next entries are in the payload.
            using (var sl = GetSourceList())
            {
                foreach (uint ssrc in sl)
                {
                    //Give a ReportBlock of 4 bytes to represent the source list entry
                    using (ReportBlock rb = new ReportBlock(new MemorySegment(Payload.Array, Payload.Offset + sl.ItemIndex * RFC3550.SourceList.ItemSize, RFC3550.SourceList.ItemSize)))
                    {
                        yield return rb;
                    }
                }
            }
        }

        public override string ToString()
        {

            return "GoodbyeReport : \n" + base.ToString();
        }
    }

    #endregion
}