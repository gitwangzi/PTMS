using System;
using System.Collections.Generic;

namespace Gsafety.PTMS.Media.H264
{
    class H264Bitstream : IDisposable
    {
        int _bitsLeft;

        IEnumerator<byte> _bytes;
        byte _currentByte;
        byte _nextByte;

        public H264Bitstream(IEnumerable<byte> buffer)
        {
            _bytes = buffer.GetEnumerator();

            if (!_bytes.MoveNext())
            {
                _bytes.Dispose();
                _bytes = null;

                return;
            }

            _nextByte = _bytes.Current;

            _bitsLeft = 0;
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            using (_bytes)
            { }
        }

        public bool HasData
        {
            get
            {
                if (_bitsLeft > 0)
                    return true;

                return GetMoreBits();
            }
        }

        bool GetMoreBits()
        {
            if (_bitsLeft > 0)
                return true;

            if (null == _bytes)
                return false;

            _currentByte = _nextByte;

            if (!_bytes.MoveNext())
            {
                _bytes.Dispose();
                _bytes = null;

                _bitsLeft = 0;

                var zeros = 0;
                while (0 == (1 & _currentByte))
                {
                    ++zeros;
                    _currentByte >>= 1;
                }

                if (zeros > 6)
                    return false;

                _currentByte >>= 1;
                _bitsLeft = 7 - zeros;

                return true;
            }

            _bitsLeft = 8;
            _nextByte = _bytes.Current;

            return true;
        }

        public uint ReadBits(int count)
        {
            var ret = 0u;

            while (count > 0)
            {
                if (_bitsLeft < 1)
                {
                    if (!GetMoreBits())
                        throw new FormatException("Read past the end of the RBSP stream");
                }

                var v = _currentByte;

                if (8 == _bitsLeft && count >= 8)
                {
                    ret = (ret << 8) | v;

                    count -= 8;
                    _bitsLeft = 0;

                    continue;
                }

                var takeBits = Math.Min(_bitsLeft, count);

                ret <<= takeBits;

                var mask = (1u << takeBits) - 1u;

                v >>= _bitsLeft - takeBits;

                ret |= v & mask;

                _bitsLeft -= takeBits;
                count -= takeBits;
            }

            return ret;
        }
    }
}
