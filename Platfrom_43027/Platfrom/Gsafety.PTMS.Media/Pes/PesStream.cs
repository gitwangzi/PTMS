using System;
using System.IO;
using Gsafety.PTMS.Media.TransportStream.TsParser;

namespace Gsafety.PTMS.Media.Pes
{
    public sealed class PesStream : Stream
    {
        int _location;
        TsPesPacket _packet;

        public TsPesPacket Packet
        {
            get { return _packet; }
            set
            {
                _packet = value;
                _location = 0;
            }
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return Packet.Length; }
        }

        public override long Position
        {
            get { return _location; }
            set { Seek(value, SeekOrigin.Begin); }
        }

        public override void Flush()
        { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var p = Packet;

            count = Math.Min(count, p.Length - _location);

            if (count < 1)
                return 0;

            Array.Copy(p.Buffer, p.Index + _location, buffer, offset, count);

            _location += count;

            return count;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    if (offset > Packet.Length || offset < 0)
                        throw new ArgumentOutOfRangeException("offset");

                    _location = (int)offset;

                    break;
                case SeekOrigin.End:
                    if (offset > Packet.Length || offset < 0)
                        throw new ArgumentOutOfRangeException("offset");

                    _location = Packet.Length - (int)offset;

                    break;

                case SeekOrigin.Current:
                    var newOffset = _location + offset;

                    if (newOffset < 0 || newOffset > Packet.Length)
                        throw new ArgumentOutOfRangeException("offset");

                    _location = (int)newOffset;

                    break;
                default:
                    throw new ArgumentException("origin");
            }

            return _location;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
