using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public interface IStreamBuffer : IStreamSource, IDisposable
    {
        bool TryEnqueue(ICollection<TsPesPacket> packet);
    }

    public sealed class StreamBuffer : IBufferingQueue, IStreamBuffer
    {
        readonly Queue<TsPesPacket> _packets = new Queue<TsPesPacket>();
        readonly object _packetsLock = new object();
        readonly TsStreamType _streamType;
        Action<TsPesPacket> _freePesPacket;
        IBufferingManager _bufferingManager;
        int _isDisposed;
        bool _isDone;
        TimeSpan? _oldest;
        TimeSpan? _newest;
        int _size;
        readonly bool _isMedia;
        bool _eofPacketRead;

#if DEBUG
        static int _streamBufferCounter;
        readonly int _streamBufferId = Interlocked.Increment(ref _streamBufferCounter);
#endif

        public TsStreamType StreamType
        {
            get { return _streamType; }
        }

        public StreamBuffer(TsStreamType streamType, Action<TsPesPacket> freePesPacket, IBufferingManager bufferingManager)
        {
            if (null == streamType)
                throw new ArgumentNullException("streamType");
            if (null == freePesPacket)
                throw new ArgumentNullException("freePesPacket");
            if (null == bufferingManager)
                throw new ArgumentNullException("bufferingManager");

            _streamType = streamType;
            _freePesPacket = freePesPacket;
            _bufferingManager = bufferingManager;
            _isMedia = TsStreamType.StreamContents.Audio == _streamType.Contents || TsStreamType.StreamContents.Video == _streamType.Contents;
        }


        public void Dispose()
        {
            var wasDisposed = Interlocked.Exchange(ref _isDisposed, 1);

            if (0 != wasDisposed)
                return;

            lock (_packetsLock)
            {
                while (_packets.Count > 0)
                {
                    var packet = _packets.Dequeue();

                    if (null == packet)
                        continue;

                    _freePesPacket(packet);
                }
            }

            _freePesPacket = null;
            _bufferingManager = null;
        }

        void ThrowIfDisposed()
        {
            if (0 != _isDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }


        public bool IsEof
        {
            get { return _isDone; }
        }

        public TimeSpan? PresentationTimestamp
        {
            get
            {
                lock (_packetsLock)
                {
                    if (!_isDone && _bufferingManager.IsBuffering)
                        return null;

                    if (_packets.Count < 1)
                        return null;

                    var packet = _packets.Peek();

                    if (null == packet)
                        return null;

                    return packet.PresentationTimestamp;
                }
            }
        }

        public bool HasSample
        {
            get { lock (_packetsLock) return _packets.Count > 0; }
        }

        public float? BufferingProgress
        {
            get { return _bufferingManager.BufferingProgress; }
        }

        public TsPesPacket GetNextSample()
        {
            //LoggerInstance.Debug("StreamBuffer.GetNextSample() " + _streamType.Contents);

            ThrowIfDisposed();

            TsPesPacket packet = null;

            try
            {
                var isEmpty = false;

                lock (_packetsLock)
                {
                    if (!_isDone && _bufferingManager.IsBuffering)
                        return null;

                    if (_packets.Count > 0)
                    {
                        packet = _packets.Dequeue();

                        if (null != packet)
                        {
                            _oldest = packet.PresentationTimestamp;

                            if (!_newest.HasValue)
                                _newest = _oldest;

                            _size -= packet.Length;
                        }
                        else
                            _eofPacketRead = true;
                    }
                    else
                        isEmpty = true;
                }

                if (isEmpty)
                    _bufferingManager.ReportExhaustion();
                else
                    _bufferingManager.Refresh();

                if (null == packet)
                    return null;

#if DEBUG
                //LoggerInstance.Debug("StreamBuffer {0}/{1} forwarding sample {2}", _streamBufferId, _streamType.Contents, PresentationTimestamp);
#endif
                var localPacket = packet;

                packet = null;

                return localPacket;
            }
            catch (Exception ex)
            {
                LoggerInstance.Exception("GetNextSample: " + ex.ToString());
            }
            finally
            {
                if (null != packet)
                    _freePesPacket(packet);

#if DEBUG
                ThrowIfDisposed();
#endif
            }

            return null;
        }

        public void FreeSample(TsPesPacket packet)
        {
            _freePesPacket(packet);
        }

        public bool DiscardPacketsBefore(TimeSpan value)
        {
            bool isEmpty;
            var discarded = false;

            lock (_packetsLock)
            {
                while (_packets.Count > 0)
                {
                    var packet = _packets.Peek();

                    if (packet.PresentationTimestamp >= value)
                        break;

                    _packets.Dequeue();

                    FreeSample(packet);

                    discarded = true;
                }

                isEmpty = !_bufferingManager.IsBuffering;
            }

            if (isEmpty)
                _bufferingManager.ReportExhaustion();
            else
                _bufferingManager.Refresh();

            return discarded;
        }

        void IBufferingQueue.Flush()
        {
            TsPesPacket[] packets = null;

            lock (_packetsLock)
            {
                if (_packets.Count > 0)
                {
                    packets = _packets.ToArray();

                    _packets.Clear();
                }

                _size = 0;
                _newest = null;
                _oldest = null;

                if (!_eofPacketRead)
                    _isDone = false;
            }

            if (null == packets)
                return;

            foreach (var packet in packets)
            {
                if (null == packet)
                    continue;

                _freePesPacket(packet);
            }
        }

        public void UpdateBufferStatus(BufferStatus bufferStatus)
        {
            lock (_packetsLock)
            {
                bufferStatus.PacketCount = _packets.Count;
                bufferStatus.Size = _size;
                bufferStatus.Newest = _newest;
                bufferStatus.Oldest = _oldest;
                bufferStatus.IsValid = _packets.Count > 0 && _newest.HasValue && _oldest.HasValue;
                bufferStatus.IsDone = _isDone;
                bufferStatus.IsMedia = _isMedia;
            }
        }

        public bool TryEnqueue(ICollection<TsPesPacket> packets)
        {
            ThrowIfDisposed();

            lock (_packetsLock)
            {
                foreach (var packet in packets)
                {
                    if (null != packet && packet.Length < 1)
                    {
                        //LoggerInstance.Debug("StreamBuffer.TryEnqueue() discarding short buffer: size " + packet.Length);

                        _freePesPacket(packet);

                        continue;
                    }

                    if (_isDone)
                    {
                        if (null != packet)
                            _freePesPacket(packet);

                        continue;
                    }

                    _packets.Enqueue(packet);

                    if (null != packet)
                    {
                        _newest = packet.PresentationTimestamp;

                        if (!_oldest.HasValue)
                            _oldest = _newest;

                        _size += packet.Length;
                    }
                    else
                        _isDone = true;
                }
            }

#if DEBUG
            ThrowIfDisposed();
#endif

            return true;
        }

        public override string ToString()
        {
            var bufferStatus = new BufferStatus();

            UpdateBufferStatus(bufferStatus);

            return string.Format("{0} count {1} size {2} newest {3} oldest {4}",
                StreamType.Contents, bufferStatus.PacketCount, bufferStatus.Size, bufferStatus.Newest, bufferStatus.Oldest);
        }
    }
}
