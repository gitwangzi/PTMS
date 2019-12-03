﻿using Gsafety.PTMS.Media.RTSP.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gsafety.PTMS.Media.RTSP.RTP
{
    /// <summary>
    /// A collection of RtpPackets
    /// </summary>
    public class RtpFrame : Gsafety.PTMS.Media.RTSP.Common.SuppressedFinalizerDisposable, IEnumerable<RtpPacket>// IDictionary, IList, etc? IClonable
    {
        //Todo, should be Lifetime Disposable        (Where Lifetime is given by expected duration + connection time by default or 1 Minute)

        #region Static

        //Todo, could just as well be an extension to RtpFrame.
        //This also will appear in derived types for no reason.
        /// <summary>
        /// Assembles a single packet by skipping any ContributingSourceListOctets and optionally Extensions and a certain profile header. 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="useExtensions"></param>
        /// <param name="profileHeaderSize"></param>
        /// <returns></returns>
        public static Common.MemorySegment AssemblePacket(RtpPacket packet, bool useExtensions = false, int profileHeaderSize = 0)
        {
            //Set to profileHeaderSize
            int localSize = profileHeaderSize;

            //Should be handled by derived implementation because it is not known if the flags are relevent to the data or how.
            if (packet.Extension)
            {
                //Use the extension
                using (RtpExtension extension = packet.GetExtension())
                {
                    //If present and complete
                    if (extension != null && extension.IsComplete)
                    {
                        //If the data should be included then include it
                        if (useExtensions)
                        {
                            localSize += packet.ContributingSourceListOctets + RtpExtension.MinimumSize;

                            return new Common.MemorySegment(packet.Payload.Array,
                                    packet.Payload.Offset + localSize,
                                    (packet.Payload.Count - localSize) - packet.PaddingOctets);
                        }

                        //Add the size to the localSize
                        localSize += extension.Size;
                    }
                }
            }

            //Include any csrc's prsent
            localSize += packet.ContributingSourceListOctets;

            return new Common.MemorySegment(packet.Payload.Array,
                packet.Payload.Offset + localSize,
                (packet.Payload.Count - localSize) - packet.PaddingOctets);
        }

        //Todo, possibly add Packetize static overload for byte[] given a payloadType and timestamp etc.

        #endregion

        #region Readonly

        //Used in GetHashCode and Equals
        //readonly ValueType
        /// <summary>
        /// The DateTime in which the instance was created.
        /// </summary>
        public readonly DateTime Created;

        //All of these properties - SHOULD - be readonly but would require parameter in constructor, would also not perform as well.

        /// <summary>
        /// The maximum amount of packets which can be contained in a frame
        /// </summary>
        internal protected int MaxPackets = 1024;

        /// <summary>
        /// Indicates if Add operations will ensure that all packets added have the same <see cref="RtpPacket.PayloadType"/>
        /// </summary>
        internal protected bool AllowsMultiplePayloadTypes { get; set; }

        ///// <summary>
        ///// Indicates if duplicate packets will be allowed to be stored.
        ///// </summary>
        //public bool AllowDuplicatePackets { get; internal protected set; }

        /// <summary>
        /// Updating the SequenceNumber of a contained packet can still cause unintended results.
        /// </summary>
        internal readonly protected List<RtpPacket> Packets;

        //Could use List if Add is replaced with Insert and index given by something like => Abs(Clamp(n, 0, Min(n - count) + Max(n - count))) or IndexOf(n)
        //Also needs a Dictionary to be able to maintain state of remove operations...
        //Could itself be a Dictionary to ensure that a packet is not already present but if packets are added out of order then the buffer would need to be created again...

        //How to keep track of Decodering Order ?

        //See notes below in region Todo

        /// <summary>
        /// After a single RtpPacket is <see cref="Depacketize">depacketized</see> it will be placed into this list with the appropriate index.
        /// </summary>
        internal readonly Dictionary<int, MemorySegment> Depacketized;

        #region Todo

        //Todo, could potentially have a single array for copying individual packets too (it seems more of a job for the application or RtpClient implementation)

        //Could keep a HashSet<int> with the SeqeuenceNumbers until GetHashCode is determined.

        //In this case we need Depacketized index's to also match the index in packets so remove works correctly.
        //The problem with that is that children classes may add more than one depacketization per packet, (that data may or may not be from the payload of the packet)
        //To remove all of them there would have to be a way to store packets and their data to all segments.

        //Maybe something like RtpPacket, IEnumerable<Common.MemorySegment>
        // e.g. ConcurrentThesaurus<RtpPacket, Common.MemorySegment> , but it would be impossible to preserve the order of packets in the dictionary without using SortedDictionary and custom comparer for every derived type

        //A structure with the Packet and all corresponding Segments could work, this makes removing easy but it required the DecodingOrder to be seperately stored for each packet.
        //This is because there may be multiple access units within a single packet which have multiple decoding order vectors.

        //For example

        //Rtp 0
        //Au 1, 3, 5
        //Rtp 1
        //Au 0, 2, 4

        //Packetization and Depacketization structure for internal use.
        internal class PaD : Common.BaseDisposable
        {
            #region Fields

            RtpPacket m_Packet;

            readonly List<Gsafety.PTMS.Media.RTSP.Common.MemorySegment> Parts;

            internal int DecodingOrder;

            #endregion

            #region Constructor

            PaD(PaD pad, bool shouldDispose = true)
                : base(shouldDispose)
            {
                if (Common.IDisposedExtensions.IsNullOrDisposed(pad)) throw new InvalidOperationException("pad is NullOrDisposed");

                m_Packet = pad.m_Packet;

                Parts = pad.Parts;
            }

            PaD(RtpPacket packet)
                : base(packet.ShouldDispose)
            {
                if (Common.IDisposedExtensions.IsNullOrDisposed(packet)) throw new InvalidOperationException("packet is NullOrDisposed");

                m_Packet = packet;

                Parts = new List<Common.MemorySegment>();
            }

            #endregion

            //Override
            protected internal override void Dispose(bool disposing)
            {
                if (IsDisposed) return;

                base.Dispose(disposing);

                if (IsDisposed && ShouldDispose)
                {
                    for (int i = 0, e = Parts.Count; i < e; ++i)
                    {
                        using (Common.MemorySegment ms = Parts[0])
                        {
                            Parts.RemoveAt(0);
                        }
                    }
                }
            }

            //Methods -> ToSegmentStream()
        }

        //When (re)packetizing or depacketizing:
        //1) A PaD is created with the packet.
        //2) The Parts member is created empty.
        //3) The DecodingOrder number is increased or set from the packet sequenceNumber
        //4) The packet data is depacketized to the Parts member.
        //5) If more data remains in the packet go to Step 1
        // Repeat as necessary for the data in the packet.

        //Each packet would be allowed to have many buffers and they could be removed according to the packet which owns them (or the MemorySegment even better in some cases)

        //Overlapping DecodingOrder's are easily handled by using Distinct / Sort in PrepareBuffer

        //This makes removing slower though since the list must be traversed for all PaD in the list which contain Packet as a member.

        //This can be optimized further by using a ConcurrentThesaurus<RtpPacket, PaD> which can be cleared in O(1) upon remove.

        //internal Gsafety.PTMS.Media.RTSP.Common.Collections.Generic.ConcurrentThesaurus<RtpPacket, PaD> References = new Common.Collections.Generic.ConcurrentThesaurus<RtpPacket, PaD>();

        //The only thing left to resolve would be HashCode conflicts due to how GetHashCode is implemented
        //Could also maybe just use sequenceNumber since it wouldn't change the semantic and would be lighter on memory.

        internal Gsafety.PTMS.Media.RTSP.Common.Collections.Generic.ConcurrentThesaurus<int, PaD> References = new Common.Collections.Generic.ConcurrentThesaurus<int, PaD>();

        #endregion

        #endregion

        #region Fields

        /// <summary>
        /// The amount of packets contained which had the Marker bit set.
        /// </summary>
        internal int m_MarkerCount = 0;

        /// <summary>
        /// Timestamp, SynchronizationSourceIdentifier of all contained packets.
        /// </summary>
        internal int m_Timestamp = -1, m_Ssrc = -1;

        /// <summary>
        /// The PayloadType of all contained packets, if the value has not been determined than it's default value is -1.
        /// </summary>
        internal int m_PayloadType = -1;

        #region If need offsets of marker packets specificially...
        //Marker index, Offset into Packets
        //0, 1
        //1, 3
        //2, 7
        //3, 9
        //Dictionary<int, int> MarkerPackets
        //public int MarkerCount => MarkerPackets.Count

        #endregion

        /// <summary>
        /// The Lowest and Highest SequenceNumber in the contained RtpPackets or -1 if no RtpPackets are contained
        /// </summary>
        internal int m_LowestSequenceNumber = -1, m_HighestSequenceNumber = -1;

        /// <summary>
        /// Useful for depacketization.. might not need a field as buffer can be created on demand from SegmentStream, just need to determine if every call to Buffer should maintain position or not.
        /// </summary>
        internal protected Common.SegmentStream m_Buffer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the expected PayloadType of all contained packets or -1 if has not <see cref="SpecifiedPayloadType"/>
        /// </summary>
        public int PayloadType
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_PayloadType; }
            #region Unused set
            //internal protected set
            //{
            //    //When the value is less than 0 this means clear...
            //    if (value < 0)
            //    {
            //        m_PayloadType = -1;

            //        Clear();

            //        return;
            //    }

            //                                                                      //value &= Common.Binary.SevenBitMaxValue;

            //    //Set all packets to the value (validates the value) should also check that m_PayloadType is equal to packet.PayloadType when multiple payload types are stored.
            //    foreach (RtpPacket packet in Packets) packet.PayloadType = value; //packet.Header.First16Bits.Last8Bits = (byte)value;

            //    //Set the byte depending on if the marker was previous set.
            //    m_PayloadType = (byte)(HasMarker ? value | RFC3550.CommonHeaderBits.RtpMarkerMask : value);
            //}
            #endregion
        }

        /// <summary>
        /// Indicates the amount of packets stored that have the Marker bit set.
        /// </summary>
        public int MarkerCount
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_MarkerCount; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            protected set { m_MarkerCount = value; }
        }

        //Could also indicate if any have extensions... possibly provide methods for doing things with them

        //SourceList which should be added to each packet int the frame?      

        //Public means this can be disposed. virtual is not necessary
        public Common.SegmentStream Buffer
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return HasBuffer ? m_Buffer : m_Buffer = new Common.SegmentStream(Depacketized.Values); }
        }

        /// <summary>
        /// Gets or sets the SynchronizationSourceIdentifier of All Packets Contained or -1 if not assigned.
        /// </summary>
        public int SynchronizationSourceIdentifier
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_Ssrc; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal protected set
            {
                m_Ssrc = value;

                foreach (RtpPacket p in Packets) p.SynchronizationSourceIdentifier = m_Ssrc;
            }
        }

        /// <summary>
        /// Gets or sets the Timestamp of All Packets Contained or -1 if unassigned.
        /// </summary>
        public int Timestamp
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_Timestamp; }
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            internal protected set { m_Timestamp = value; }
        }

        /// <summary>
        /// Gets the packet at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal protected RtpPacket this[int index]
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return Packets[index]; }
            #region Unused set
            /*private*/
            //set { Packets[index] = value; }
            #endregion
        }

        #region Readonly Properties

        /// <summary>
        /// Gets a value indicating if the <see cref="PayloadType"/> was specified.
        /// </summary>
        public bool SpecifiedPayloadType
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_PayloadType >= 0; }
        }

        /// <summary>
        /// Indicates if there are any packets have been <see cref="Depacketize">depacketized</see>
        /// </summary>
        public bool HasDepacketized
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return Depacketized.Count > 0; }
        }

        /// <summary>
        /// Indicates if the <see cref="Buffer"/> is not null and CanRead.
        /// </summary>
        public bool HasBuffer
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return false == (m_Buffer == null) && m_Buffer.CanRead; }
        }

        /// <summary>
        /// Indicates if all contained RtpPacket instances have a Transferred Value otherwise false.
        /// </summary>
        public bool Transferred
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return IsEmpty ? false : Packets.All(p => p.Transferred.HasValue); }
        }

        //Possible make an Action<bool> / Delegate 'IsCompleteCheck' which represents the functionality to use here and remove virtual.
        /// <summary>
        /// Indicates if the RtpFrame <see cref="Disposed"/> is False AND <see cref="IsMissingPackets"/> is False AND <see cref="HasMarker"/> is True.
        /// </summary>
        public virtual bool IsComplete
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return false == IsDisposed && false == IsMissingPackets && HasMarker; }
        }

        //Todo, for Rtcp feedback one would need the sequence numbers of the missing packets...
        //Would be given any arbitrary RtpFrame and would implement that logic there.

        /// <summary>
        /// If at least 2 packets are contained, Indicates if all contained packets are sequential up the Highest Sequence Number contained in the RtpFrame.
        /// False if less than 2 packets are contained or there is a not a gap in the contained packets sequence numbers.
        /// </summary>
        /// <remarks>This function does not check <see cref="HasMarker"/></remarks>
        public bool IsMissingPackets
        {
            get
            {
                int count = Count;

                switch (count)
                {
                    //No packets
                    case 0: return true;
                    //Single packet only
                    case 1: return false;
                    //Skip the range check for 2 packets
                    case 2: return ((short)(m_LowestSequenceNumber - m_HighestSequenceNumber) != -1); //(should be same as 1 + (short)((m_LowestSequenceNumber - m_HighestSequenceNumber)) == 0 but saves an additional addition)
                    //2 or more packets, cache the m_LowestSequenceNumber and check all packets to be sequential starting at offset 1
                    default: RtpPacket p; for (int nextSeq = m_LowestSequenceNumber == ushort.MaxValue ? ushort.MinValue : m_LowestSequenceNumber + 1, i = 1; i < count; ++i)
                        {
                            //Scope the packet
                            p = Packets[i];

                            //obtain the sequence number to check if the packet is missing
                            if (p.SequenceNumber != nextSeq) return true;

                            //If the differece is not 0 then the packet is missing.
                            //if ((short)(p.SequenceNumber - nextSeq) != 0) return true;

                            //Determine the next sequence number
                            nextSeq = nextSeq == ushort.MaxValue ? ushort.MinValue : nextSeq + 1; //++nextSeq;
                        }

                        //Not missing any packets with respect to sequence number.
                        return false;
                }
            }
        }

        //Possible change name to LastPacketIsMarker
        /// <summary>
        /// Indicates if a contained packet has the marker bit set. (Usually the last packet in a frame)
        /// </summary>
        public bool HasMarker
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_MarkerCount > 0; }
        }

        /// <summary>
        /// The amount of Packets in the RtpFrame
        /// </summary>
        public int Count
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return Packets.Count; }
        }

        /// <summary>
        /// Indicates if there are packets in the RtpFrame
        /// </summary>
        public bool IsEmpty
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return Packets.Count == 0; }
        }

        /// <summary>
        /// Gets the 16 bit unsigned value which is associated with the highest sequence number contained or -1 if no RtpPackets are contained.
        /// Usually the packet at the highest offset
        /// </summary>
        public int HighestSequenceNumber
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_HighestSequenceNumber; }
        }

        /// <summary>
        /// Gets the 16 bit unsigned value which is associated with the lowest sequence number contained or -1 if no RtpPackets are contained.
        /// Usually the packet at the lowest offset
        /// </summary>
        public int LowestSequenceNumber
        {
            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            get { return m_LowestSequenceNumber; }
        }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance which has no packets and an undetermined <see cref="PayloadType"/>
        /// </summary>
        /// <param name="shouldDispose">Indicates if the instance will <see cref="Clear"/> when <see cref="Dispose"/> is called.</param>
        public RtpFrame(bool shouldDispose)
            : base(shouldDispose)
        {
            //Indicate when this instance was created
            Created = DateTime.UtcNow;

            //Create the list
            Packets = new List<RtpPacket>();

            //Create the list
            Depacketized = new Dictionary<int, Common.MemorySegment>();
        }

        /// <summary>
        /// Creates an instance which has no packets and an undetermined <see cref="PayloadType"/> and will dispose when <see cref="Dispose"/> is called.
        /// </summary>
        public RtpFrame() : this(true) { }

        /// <summary>
        /// Creates an instance which has no packets and and the given <see cref="PayloadType"/> and will dispose when <see cref="Dispose"/> is called.
        /// </summary>
        /// <param name="payloadType"></param>
        public RtpFrame(int payloadType)
            : this()
        {
            //Should be bound from 0 - 127 inclusive...
            if (payloadType > byte.MaxValue) throw Common.Binary.CreateOverflowException("payloadType", payloadType, byte.MinValue.ToString(), byte.MaxValue.ToString());

            //Assign the type of RtpFrame
            m_PayloadType = (byte)payloadType;
        }

        /// <summary>
        /// Creates an instance which has no packets and and the given <see cref="PayloadType"/>, <see cref="Timestamp"/> and <see cref="SynchronizationSourceIdentifier"/> and will dispose when <see cref="Dispose"/> is called.
        /// </summary>
        /// <param name="payloadType"></param>
        /// <param name="timeStamp"></param>
        /// <param name="ssrc"></param>
        public RtpFrame(int payloadType, int timeStamp, int ssrc)
            : this(payloadType)
        {
            //Assign the Synconrization Source Identifier
            m_Ssrc = ssrc;

            //Assign the Timestamp
            m_Timestamp = timeStamp;
        }

        //Todo, additional byte[] overloads to packetize data with

        //Could also provide delegated actions for use with logic to prevent having to subclass but type safetype is sacraficed somewhat as 
        //The semantics of each instance cannot easily be traced without knowing what to expect from each delegation
        //This requires the use of interfaces to properly 'do'.

        /// <summary>
        /// Creates an instance and if the packet is not null assigns properties from the given packet and optionally adds the packet to the list of stored packets.
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="addPacket"></param>
        /// <param name="shouldDispose"></param>
        public RtpFrame(RtpPacket packet, bool addPacket = true, bool shouldDispose = true)
            : this(shouldDispose)
        {
            if (Common.IDisposedExtensions.IsNullOrDisposed(packet)) return;

            m_PayloadType = packet.PayloadType;

            m_Timestamp = packet.Timestamp;

            m_Ssrc = packet.SynchronizationSourceIdentifier;

            if (addPacket) Add(packet);
        }

        /// <summary>
        /// Clone and existing RtpFrame
        /// </summary>
        /// <param name="f">The frame to clonse</param>
        /// <param name="referencePackets">Indicate if contained packets should be referenced</param>
        public RtpFrame(RtpFrame f, bool referencePackets = false, bool referenceBuffer = false, bool shouldDispose = true)
            : base(shouldDispose) //If shouldDispose is true when referencePackets is true then Dispose will clear both lists.
        {
            if (Common.IDisposedExtensions.IsNullOrDisposed(f)) return;

            m_PayloadType = f.m_PayloadType;

            m_Ssrc = f.m_Ssrc;

            m_Timestamp = f.m_Timestamp;

            m_HighestSequenceNumber = f.m_HighestSequenceNumber;

            m_LowestSequenceNumber = f.m_LowestSequenceNumber;

            m_MarkerCount = f.m_MarkerCount;

            //If this is a shallow clone then just use the reference
            if (referencePackets) Packets = f.Packets; //Assign the list from the packets in te frame (changes to list reflected in both instances)
            else Packets = new List<RtpPacket>(f); //Create the list from the packets in the frame (changes to list not reflected in both instances)

            //It should be that...
            //If you reference the packets you also reference the buffer...

            //If the buffer is referenced
            if (referenceBuffer)
            {
                //Assign it
                m_Buffer = f.m_Buffer;

                //The depacketized must also be then..
                Depacketized = f.Depacketized;
            }
            else
            {
                //Create the list
                Depacketized = new Dictionary<int, Common.MemorySegment>();

                //Can't create a new one because of the implications
                m_Buffer = f.m_Buffer;
            }

            /// See notes and determine if this is appropraite behavior
            //ShouldDispose = f.ShouldDispose;
        }

        ///// <summary>
        ///// Destructor.
        ///// </summary>
        //~RtpFrame() { Dispose(); } 

        #endregion

        #region Methods

        //Should provide a virtual Packetize method ...

        //Should provide own logic and not throw for new or removed packets? (if packets are added or removed this logic is interrupted)
        /// <summary>
        /// Gets an enumerator of All Contained Packets at the time of the call
        /// </summary>
        /// <returns>The enumerator of the contained packets</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public IEnumerator<RtpPacket> GetEnumerator() { return Packets.GetEnumerator(); }

        //Could also depacketize in place and only use memory available.

        /// <summary>
        /// If HasDepacketized is true then returns all data already depacketized otherwise all packets are iterated and depacketized and memory is reclaimed afterwards.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Common.MemorySegment> GetDepacketizion(bool inplace = true)
        {
            //If there is already a Depacketizion use the memory in place.
            if (HasDepacketized)
            {
                foreach (Common.MemorySegment ms in Depacketized.Values)
                {
                    yield return ms;
                }

                yield break;
            }

            //Iterate the packets
            foreach (RtpPacket packet in Packets)
            {

                //An overload which returned the data could then optionally put it into the Depacketized list which would allow the Free portion to not have to occur.

                //Depacketize a single packet
                Depacketize(packet);

                //If there was anything depacketized
                if (HasDepacketized)
                {
                    //Yeild what was depacketized for this packet.
                    foreach (Common.MemorySegment ms in Depacketized.Values)
                    {
                        yield return ms;
                    }

                    //clear memory
                    FreeDepacketizedMemory(inplace);
                }
            }
        }

        //The amount of memory used by Depacketized. (When Persisted... otherwise it should be shared...)

        /// <summary>
        /// Adds a RtpPacket to the RtpFrame. The first packet added sets the SynchronizationSourceIdentifier and Timestamp if not already set.
        /// </summary>
        /// <param name="packet">The RtpPacket to Add</param>
        /// <param name="allowPacketsAfterMarker">Indicates if the packet shouldbe allowed even if the packet's sequence number is greater than or equal to <see cref="HighestSequenceNumber"/> and <see cref="IsComplete"/> is true.</param>
        public void Add(RtpPacket packet, bool allowPacketsAfterMarker = true, bool allowDuplicates = false)
        {
            //If the packet is disposed of this frame is then do not add.
            if (Common.IDisposedExtensions.IsNullOrDisposed(packet) || IsDisposed) return;

            int count = Count, ssrc = packet.SynchronizationSourceIdentifier, seq = packet.SequenceNumber, ts = packet.Timestamp, pt = packet.PayloadType;

            //No packets contained yet
            if (count == 0)
            {
                if (m_Ssrc == -1) m_Ssrc = ssrc;
                else if (ssrc != m_Ssrc) throw new ArgumentException("packet.SynchronizationSourceIdentifier must match frame SynchronizationSourceIdentifier", "packet");

                if (m_Timestamp == -1) m_Timestamp = ts;
                else if (ts != m_Timestamp) throw new ArgumentException("packet.Timestamp must match frame Timestamp", "packet");

                if (m_PayloadType == -1) m_PayloadType = pt;
                else if (AllowsMultiplePayloadTypes == false && pt != PayloadType) throw new ArgumentException("packet.PayloadType must match frame PayloadType", "packet");

                m_LowestSequenceNumber = m_HighestSequenceNumber = seq;

                Packets.Add(packet);

                //Check for the marker
                if (packet.Marker)
                {
                    m_MarkerCount = 1;

                    //m_PayloadType |= RFC3550.CommonHeaderBits.RtpMarkerMask;
                }

                //Should mark as dirty if not dispose.
                //DisposeBuffer();

                return;
            }

            //At least 1 packet is contained
            //Check payload type if indicated
            if (AllowsMultiplePayloadTypes == false && pt != PayloadType) throw new ArgumentException("packet.PayloadType must match frame PayloadType", "packet");

            if (ssrc != m_Ssrc) throw new ArgumentException("packet.SynchronizationSourceIdentifier must match frame SynchronizationSourceIdentifier", "packet");

            if (ts != m_Timestamp) throw new ArgumentException("packet.Timestamp must match frame Timestamp", "packet");

            if (count >= MaxPackets) throw new InvalidOperationException(string.Format("The amount of packets contained in a RtpFrame cannot exceed: {0}", MaxPackets));

            //Ensure not a duplicate (Must also check PayloadType if multiple types are allowed)
            if (false == allowDuplicates && (m_LowestSequenceNumber == seq || m_HighestSequenceNumber == seq)) throw new InvalidOperationException("Cannot have duplicate packets in the same frame.");

            //Could possibly check for < m_LowestSequenceNumber or > m_HighestSequenceNumber here.

            //Determine if the packet has a marker
            bool packetMarker = packet.Marker;

            //If not a duplicate and the marker is already contained
            if (HasMarker)
            {
                //Check if the packet is allowed
                if (false == allowPacketsAfterMarker) throw new InvalidOperationException("Cannot add packets after the marker packet.");
            }

            //Determine where to insert and what seq will be inserted
            int insert = 0, tempSeq = 0;

            //If AllowMultiplePayloadTypes is true then the packets should maintain their order by PayloadType and then SequenceNumber.

            //Search for insert point while the index < count and while roll over would not occur
            while (insert < count && (short)(seq - (tempSeq = Packets[insert].SequenceNumber)) >= 0)
            {
                //move the index
                ++insert;
            }

            //Ensure not a duplicate
            if (false == allowDuplicates && tempSeq == seq) throw new InvalidOperationException("Cannot have duplicate packets in the same frame.");

            //Handle prepend
            if (insert == 0)
            {
                Packets.Insert(0, packet);

                m_LowestSequenceNumber = seq;
            }
            else if (insert >= count) //Handle add
            {
                Packets.Add(packet);

                m_HighestSequenceNumber = seq;
            }
            else Packets.Insert(insert, packet); //Insert

            //Increase the marker count if the marker bit was set.
            if (packetMarker) ++m_MarkerCount;
        }

        /// <summary>
        /// Calls <see cref="Add"/> and indicates if the operations was a success
        /// </summary>
        public bool TryAdd(RtpPacket packet, bool allowPacketsAfterMarker = true, bool allowDuplicates = false)
        {
            if (IsDisposed) return false;

            try { Add(packet, allowPacketsAfterMarker, allowDuplicates); return true; }
            catch { return false; }
        }

        //TryAddOrUpdate

        //Update

        //GetHashCode on Packet instance...
        /// <summary>
        /// Indicates if the RtpFrame contains a RtpPacket
        /// </summary>
        /// <param name="packet">The RtpPacket to check</param>
        /// <returns>True if the packet is contained, otherwise false.</returns>
        //public bool Contains(RtpPacket packet) { return Packets.Contains(packet); }

        /// <summary>
        /// Indicates if the RtpFrame contains a RtpPacket
        /// </summary>
        /// <param name="sequenceNumber">The RtpPacket to check</param>
        /// <returns>True if the packet is contained, otherwise false.</returns>
        public bool Contains(int sequenceNumber) { return IndexOf(ref sequenceNumber) >= 0; }

        //[CLSCompliant(false)]
        internal bool Contains(ref int sequenceNumber) { return IndexOf(ref sequenceNumber) >= 0; }

        [CLSCompliant(false)]
        internal protected int IndexOf(int sequenceNumber) { return IndexOf(ref sequenceNumber); }

        /// <summary>
        /// Indicates if the RtpFrame contains a RtpPacket based on the given sequence number.
        /// </summary>
        /// <param name="sequenceNumber">The sequence number to check</param>
        /// <returns>The index of the packet is contained, otherwise -1.</returns>
        internal protected int IndexOf(ref int sequenceNumber)
        {
            int count = Count;

            switch (count)
            {
                case 0: return -1;
                case 1:
                    {
                        return m_HighestSequenceNumber == sequenceNumber ? 0 : -1;
                    }
                case 2:
                    {
                        if (m_LowestSequenceNumber == sequenceNumber) return 0;

                        return m_HighestSequenceNumber == sequenceNumber ? 1 : -1;
                    }
                //Only optimal if sequenceNumber is @ 1 otherwise default cases may be faster, this saves 2 additional range changes
                case 3:
                    {
                        if (Packets[1].SequenceNumber == sequenceNumber) return 1;

                        goto case 2;
                    }
                //Still really only just saves 2 checks, still not optimals
                //case 4:
                //    {
                //        if (Packets[2].SequenceNumber == sequenceNumber) return 2;

                //        goto case 3;
                //    }
                //case 5:
                //    {
                //        if (Packets[3].SequenceNumber == sequenceNumber) return 3;

                //        goto case 4;
                //    }
                default:
                    {
                        //Fast path when no roll over occur, e.g. m_Packets[0].SequenceNumber > m_Packets.Last().SequenceNumber
                        //if (m_HighestSequenceNumber > m_LowestSequenceNumber && (sequenceNumber <= m_HighestSequenceNumber && sequenceNumber >= m_LowestSequenceNumber)) return true;

                        //Check case 0
                        if (sequenceNumber == m_LowestSequenceNumber) return 0;

                        //Check case count - 1
                        if (sequenceNumber == m_HighestSequenceNumber) return count - 1;

                        RtpPacket p;

                        //Not really necessary to Max, could just start at 1, but this potentially saves some array access
                        //Loop from 1 to count - 1 since they were checked above
                        for (int i = Common.Binary.Max(1, sequenceNumber - m_LowestSequenceNumber), e = count - 1; i < e; ++i)
                        {
                            //Obtain packet at i
                            p = Packets[i];

                            //compare seqeuence number
                            if (p.SequenceNumber == sequenceNumber) return i; // i

                            //Obtain packet at 1 - e
                            p = Packets[--e];

                            //compare seqeuence number
                            if (p.SequenceNumber == sequenceNumber) return e; // e
                        }

                        return -1;
                    }
            }
        }

        //bool Remove(int seq, out RtpPacket packet)
        //bool Remove(int seq, out RtpPacket packet, out int index)

        /// <summary>
        /// Removes a RtpPacket from the RtpFrame by the given Sequence Number.
        /// </summary>
        /// <param name="sequenceNumber">The sequence number of the RtpPacket to remove</param>
        /// <returns>A RtpPacket with the sequence number if removed, otherwise null</returns>
        public RtpPacket Remove(int sequenceNumber) { return Remove(ref sequenceNumber); }

        [CLSCompliant(false)]
        public RtpPacket Remove(ref int sequenceNumber) //ref
        {
            int count = Count;

            if (count == 0) return null;

            int i = IndexOf(ref sequenceNumber);

            //> Count
            if (i < 0) return null;

            //Get the packet
            RtpPacket p = Packets[i];

            //if (p.SequenceNumber != sequenceNumber) throw new Exception();

            //Race

            //Remove it
            Packets.RemoveAt(i);

            //Determine if the sequence number effects the lowest or highest fields
            switch (--count)
            {
                case 0:
                    m_LowestSequenceNumber = m_HighestSequenceNumber = -1;

                    //m_PayloadType &= Common.Binary.SevenBitMaxValue;

                    m_MarkerCount = 0;

                    goto Done;
                //Only 1 packet remains, saves a count - 1 and an array access.
                case 1:
                    {
                        //m_LowestSequenceNumber = m_HighestSequenceNumber = Packets[0].SequenceNumber; 

                        //If this was at 0 then remap 0 to 1
                        if (sequenceNumber == m_LowestSequenceNumber) m_LowestSequenceNumber = m_HighestSequenceNumber;

                        //Remap 1 to 0
                        else m_HighestSequenceNumber = m_LowestSequenceNumber;

                        //only because multiple markers
                        goto CheckMarker;

                    }
                //only 2 packets is really default also but this saves a count - 1 instruction
                //It also saves one access to the array when possible.
                case 2:
                    {
                        switch (i)
                        {
                            case 0://(sequenceNumber == m_LowestSequenceNumber)
                                {
                                    m_LowestSequenceNumber = Packets[0].SequenceNumber;
                                    break;
                                }
                            case 1: break; //Index 1 when there was 3 packets cannot effect the lowest or highest but may have a marker if multiple marker packets are stored.
                            case 2://(sequenceNumber == m_HighestSequenceNumber)
                                {
                                    m_HighestSequenceNumber = Packets[1].SequenceNumber;
                                    break;
                                }
                        }

                        goto CheckMarker;
                    }
                default:
                    {
                        //Skip the access of the array for all cases but when the sequence was == to the m_LowestSequenceNumber (i == 0)
                        if (i == 0) //(sequenceNumber == m_LowestSequenceNumber)
                        {
                            m_LowestSequenceNumber = Packets[0].SequenceNumber; //First
                        }
                        else if (sequenceNumber == m_HighestSequenceNumber)// (i >= count)
                        {
                            m_HighestSequenceNumber = Packets[count - 1].SequenceNumber; //Last
                        }

                        goto CheckMarker;
                    }
            }

        CheckMarker:
            //Check for marker when i >= count and unset marker bit if present. (Todo, if AllowMultipleMarkers this needs to be counted)
            if (m_MarkerCount > 0 && p.Marker) --m_MarkerCount;

        Done:
            //Remove any memory assoicated with the packet by getting the key of the packet.
            //Force if the packet should be disposed... (the packet is not really being disposed just yet..)

            //The packets may not be stored with sequenceNumber as a key.
            FreeDepacketizedMemory((short)sequenceNumber, p.ShouldDispose); //(sequenceNumber);

            return p;            //Notes, i contains the offset where p was stored.
        }

        /// <summary>
        /// Empties the RtpFrame by clearing the underlying List of contained RtpPackets
        /// </summary>
        internal protected void RemoveAllPackets() //bool disposeBuffer
        {
            //Packets.Clear();

            ////Clear but don't dispose memory..??Todo
            //Depacketized.Clear();

            Clear();

            //m_HighestSequenceNumber = m_LowestSequenceNumber = -1;

            //m_MarkerCount = 0;
        }

        /// <summary>
        /// Disposes all contained packets.
        /// Disposes the buffer
        /// Clears the contained packets.
        /// </summary>
        public void Clear()
        {
            //////Multiple threads adding packets would not effect count but removing definitely would...
            ////Packets.Clear();

            ////m_LowestSequenceNumber = m_HighestSequenceNumber = -1;

            //Iterate in reverse to make the remove as efficient as possible.
            for (int e = Packets.Count; --e >= 0; --e)
            {
                //Flag / Mark removing

                ////Use the packet to get it's SequenceNumber and determine if it should be forcefully removed from the collection
                //using (RtpPacket p = Packets[e])
                //{
                //    //Must either choose to persist or free at this point.
                //    //if (p.ShouldDispose)

                //    //To free it's memory that was possibly referenced
                //    FreeDepacketizedMemory(GetPacketKey(p.SequenceNumber), p.ShouldDispose);
                //}

                ////Remove that packet.
                //Packets.RemoveAt(e);


                //Remove the packet at index 0 as well as free depacketized memory related to it.
                Remove(ref m_LowestSequenceNumber);
            }

            ////////Different than most collections
            //////DisposeAllPackets();

            //////Disposes the buffer also (but not really neede since Depacketize is handled with FreeDepacketizedMemory
            //////DisposeBuffer();

            //////Dispose all segments in Depacketized.
            //////FreeDepacketizedMemory();

            //////Finally clears the collection and resets sequence numbers
            //////RemoveAllPackets(); 
        }

        //(if packets are added or removed this logic is interrupted)
        /// <summary>
        /// Disposes all contained packets. 
        /// </summary>
        internal protected void DisposeAllPackets()
        {
            //System.Linq.ParallelEnumerable.ForAll(Packets.AsParallel(), (t) => t.Dispose());

            //Dispose all packets...
            foreach (RtpPacket p in Packets) p.Dispose();
        }

        //Notes, Assemble terminology is backwards, should be Disassemble
        //This also has no place in the API unless forcefully made up, e.g. ProcessPacket could be Assemble

        //The differences currently are related to the types of return which are hard to maintain and understand
        //Assemble a packet means to take a rtp packet and get the data which is needed for the decoder
        //sometimes the extensions are needed, most of the time there is only the need to skip the csrc list if present

        /// <summary>
        /// Calls <see cref="RtpFrame.AssemblePacket"/>
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="useExtensions"></param>
        /// <param name="profileHeaderSize"></param>
        /// <returns></returns>
        public virtual Common.MemorySegment Assemble(RtpPacket packet, bool useExtensions = false, int profileHeaderSize = 0)
        {
            return RtpFrame.AssemblePacket(packet, useExtensions, profileHeaderSize);
        }

        /// <summary>
        /// Assembles the RtpFrame into a IEnumerable by use of concatenation, the ExtensionBytes and Payload of all contained RtpPackets into a single sequence (excluding the RtpHeader)
        /// <see cref="RtpFrame.AssemblePacket"/>
        /// </summary>
        /// <returns>The byte array containing the assembled frame</returns>
        public IEnumerable<byte> Assemble(bool useExtensions = false, int profileHeaderSize = 0)
        {
            //The result
            IEnumerable<byte> sequence = Common.MemorySegment.Empty;

            //Iterate the packets (if packets are added or removed this logic is interrupted)
            //Use the static functionality by default RtpFrame.AssemblePacket(packet, useExtensions, profileHeaderSize)
            foreach (RtpPacket packet in Packets) sequence = sequence.Concat(Assemble(packet, useExtensions, profileHeaderSize));


            //Return the result
            return sequence;
        }

        //Todo, virtual here increases complexity and overhead.

        //Should return true if new data has been depacketized.

        /// <summary>
        /// Depacketizes all contained packets ignoring <see cref="IsComplete"/>.
        /// </summary>
        public void Depacketize() { Depacketize(true); }

        //Same here but allows dervived types to specify

        /// <summary>
        /// Depacketizes all contained packets if possible.
        /// </summary>
        /// <param name="allowIncomplete">Determines if <see cref="IsComplete"/> must be true</param>
        public virtual void Depacketize(bool allowIncomplete)
        {
            //May allow incomplete packets.
            if (false == allowIncomplete && false == IsComplete) return;

            //This should proably provide the index to Depacketize otherwise the order cannot be preserved and when removing the order is unknown.

            //for (int i = 0, e = Count; i < e; ++i)
            //{
            //    RtpPacket p = Packets[i];

            //    Depacketize(p, i);
            //}

            //If so then Depacketized should be a list just like Packets so their index numbers match.

            //The other way would be to make Packets a list of class PacketKey { RtpPacket packet; List<MemorySegment> Depacketized;  }

            //When depacketizing the list KeyItem would implicitly be in the same order as the packets.

            //Iterate all packets contained and depacketize
            foreach (RtpPacket packet in Packets) Depacketize(packet);

            //PrepareBuffer must be called to access the buffer.
        }

        //Virtual so dervied types can call their Depacketize method with any options they may require

        /// <summary>
        /// Depacketizes a single packet
        /// </summary>
        /// <param name="packet"></param>
        public virtual void Depacketize(RtpPacket packet)
        {
            if (Common.IDisposedExtensions.IsNullOrDisposed(packet)) return;

            int index = (short)packet.SequenceNumber;

            if (Depacketized.ContainsKey(index)) return;

            Depacketized.Add(index, packet.PayloadDataSegment);
        }

        /// <summary>
        /// Takes all depacketized segments and writes them to the buffer.
        /// Disposes any existing buffer. Creates a new buffer.
        /// </summary>
        internal protected void PrepareBuffer() //bool, persist, action pre pre write, post write
        {
            //Ensure there is something to write to the buffer
            if (false == HasDepacketized) return;

            //If already exists then dispose
            DisposeBuffer();

            //Create a new buffer
            m_Buffer = new Common.SegmentStream(Depacketized.Values);

            ////Iterate ordered segments
            //foreach (KeyValuePair<int, Common.MemorySegment> pair in Depacketized)
            //{
            //    //Get the segment
            //    Common.MemorySegment value = pair.Value;

            //    //if null, disposed or empty skip
            //    if (Common.IDisposedExtensions.IsNullOrDisposed(value) || value.Count == 0) continue;

            //    //Write it to the Buffer
            //    m_Buffer.Write(value.Array, value.Offset, value.Count);
            //}

            ////Ensure at the begining
            //m_Buffer.Seek(0, System.IO.SeekOrigin.Begin);
        }

        /// <summary>
        /// If <see cref="HasDepacketized"/>, Copies the memory already depacketized to an array at the given offset.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <returns>The amount of bytes copied</returns>
        public int CopyTo(byte[] buffer, int offset)
        {
            int total = 0;

            //Iterate decorder ordered segments (possibly not in rtp order)
            foreach (KeyValuePair<int, Common.MemorySegment> pair in Depacketized)
            {
                //Get the segment
                Common.MemorySegment value = pair.Value;

                //if null, disposed or empty skip
                if (Common.IDisposedExtensions.IsNullOrDisposed(value) || value.Count == 0) continue;

                //Write it to the Buffer
                System.Array.Copy(value.Array, value.Offset, buffer, offset, value.Count);

                //Accumulate the total.
                total += value.Count;

                //Move the offset
                offset += value.Count;
            }

            return total;
        }

        /// <summary>
        /// virtual so it's easy to keep the same API, not really needed though since Dispose is also overridable.
        /// </summary>
        internal virtual protected void DisposeBuffer()
        {
            if (m_Buffer != null)
            {
                m_Buffer.Dispose();

                m_Buffer = null;
            }
        }

        //Todo, would be handled with other collection via remove...
        internal protected void FreeDepacketizedMemory(bool force = false)
        {
            //iterate each key in Depacketized
            foreach (KeyValuePair<int, Common.MemorySegment> pair in Depacketized)
            {
                //Set ShouldDispose = true and call Dispose.
                if (force || pair.Value.ShouldDispose)
                    Common.BaseDisposable.SetShouldDispose(pair.Value, true, false);
            }

            //Ensure cleared.
            Depacketized.Clear();
        }

        //PersistPacketizedMemory()...

        //IsPersisted bool

        //Coudld ensure the orderNumber prefixes the SequenceNumber serially.
        //const uint OrderMask = 0xffff0000;

        //const int KeyMask = 0x0000ffff;
        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal protected int GetOrderNumber(ref int key) { return (int)((short)key & OrderMask); }

        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //[CLSCompliant(false)]
        //internal protected /*virtual*/ int GetPacketKey(ref int key)
        //{
        //    unchecked { return (short)key; }
        //}

        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        //internal protected /*virtual*/ int GetPacketKey(int key)
        //{
        //    //Todo, could allow unsafe calls here to improve performance Int32ToInt16Bits
        //    return GetPacketKey(ref key);
        //}

        /// <summary>
        /// Removes memory refereces related to the given key.
        /// By default if the memory was persisted it is left in the list.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="force"></param>
        internal protected void FreeDepacketizedMemory(int key, bool force = false)
        {
            //Needs a method to virtually determine the key of the packet.
            if (Depacketized.ContainsKey(key))
            {
                //Obtain the segment
                Common.MemorySegment segment = Depacketized[key];

                //If forced or the memory was not persisted
                if (force || segment.ShouldDispose)
                {
                    //Dispose the memory
                    segment.Dispose();

                    //Remove it from the list.
                    Depacketized.Remove(key);
                }
            }
        }

        #endregion

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected internal override void Dispose(bool disposing)
        {
            if (false == disposing || false == ShouldDispose) return;

            base.Dispose(ShouldDispose);

            //Remove packets and any memory
            Clear();

            //Dispose the buffer.
            DisposeBuffer();

            //Free the depacketized memory incase packets were removed and we own the memory.
            FreeDepacketizedMemory(true);
        }
    }
}