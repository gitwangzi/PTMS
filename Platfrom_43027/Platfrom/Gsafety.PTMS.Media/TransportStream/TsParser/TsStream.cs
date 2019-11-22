using System;

namespace Gsafety.PTMS.Media.TransportStream.TsParser
{
    public class TsStream
    {
        readonly byte[] _buffer = new byte[32 * 1024];
        readonly TsDecoder _decoder;
        readonly Action<TsStream> _handler;
        readonly uint _pid;
        int _count;
        int _index;

        public TsStream(TsDecoder decoder, uint pid, Action<TsStream> handler)
        {
            _decoder = decoder;
            _pid = pid;
            _handler = handler;
        }

        public uint PID
        {
            get { return _pid; }
        }

        public int Length
        {
            get { return _index; }
        }

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        public void Add(TsPacket packet)
        {
            if (packet.IsStart)
                _index = 0;
            else
            {
                // Ignore duplicates and other nonsense
                var nextCount = (_count + 1) & 0x0f;

                if (packet.ContinuityCount != nextCount)
                    return;
            }

            _count = packet.ContinuityCount;

            var length = packet.PayloadLength;

            if (_index + length <= _buffer.Length)
            {
                packet.CopyTo(_buffer, _index);
                _index += length;

                if (null != _handler)
                    _handler(this);
            }
        }
    }
}
