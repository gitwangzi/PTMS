using System;
using System.Diagnostics;
using Gsafety.PTMS.Media.Audio;
using Gsafety.PTMS.Media.TransportStream.TsParser;
using Gsafety.PTMS.Media.TransportStream.TsParser.Utility;

namespace Gsafety.PTMS.Media.AAC
{
    public sealed class AacParser : AudioParserBase
    {
        public AacParser(ITsPesPacketPool pesPacketPool, Action<IAudioFrameHeader> configurationHandler, Action<TsPesPacket> submitPacket)
            : base(new AacFrameHeader(), pesPacketPool, configurationHandler, submitPacket)
        { }

        public override void ProcessData(byte[] buffer, int offset, int length)
        {
            Debug.Assert(length > 0);
            Debug.Assert(offset + length <= buffer.Length);

            var endOffset = offset + length;
            // Make sure there is enough room for the frame header.  We really only need 9 bytes
            // for the header.
            EnsureBufferSpace(128);

            for (var i = offset; i < endOffset; )
            {
                var storedLength = _index - _startIndex;

                if (storedLength <= 9)
                    ProcessHeader(storedLength, buffer[i++]);
                else
                {
                    // "_frameHeader" has a valid header and we have enough buffer space
                    // for the frame.

                    i += ProcessBody(buffer, i, storedLength, endOffset);
                }
            }
        }

        int ProcessBody(byte[] buffer, int index, int storedLength, int endOffset)
        {
            var remainingFrameLength = _frameHeader.FrameLength - storedLength;

            Debug.Assert(remainingFrameLength >= 0);

            var remainingBuffer = endOffset - index;

            Debug.Assert(remainingBuffer >= 0);

            var copyLength = Math.Min(remainingBuffer, remainingFrameLength);

            Debug.Assert(copyLength >= 0);

            if (copyLength > 0)
                Array.Copy(buffer, index, _packet.Buffer, _index, copyLength);

            _index += copyLength;
            storedLength += copyLength;

            Debug.Assert(storedLength >= 0 && storedLength == _index - _startIndex);
            Debug.Assert(storedLength <= _frameHeader.FrameLength);

            if (storedLength == _frameHeader.FrameLength)
            {
                // We have a completed AAC frame.

                if (AacDecoderSettings.Parameters.UseRawAac)
                {
                    var header = _frameHeader.HeaderLength;

                    _startIndex += header;
                }

                SubmitFrame();
            }

            return copyLength;
        }

        void ProcessHeader(int storedLength, byte data)
        {
        retry:
            if (0 == storedLength)
            {
                if (0xff == data)
                    _packet.Buffer[_index++] = 0xff;
                else
                    ++_badBytes;
            }
            else if (1 == storedLength)
            {
                if (0xf0 == (0xf0 & data))
                    _packet.Buffer[_index++] = data;
                else
                {
                    _index = _startIndex;

                    storedLength = 0;

                    ++_badBytes;

                    goto retry;
                }
            }
            else if (storedLength < 9)
                _packet.Buffer[_index++] = data;
            else
            {
                _packet.Buffer[_index++] = data;

                // We now have an AAC header.

                var shouldConfigure = !_isConfigured && _hasSeenValidFrames && 0 == _badBytes;

                if (!_frameHeader.Parse(_packet.Buffer, _startIndex, _index - _startIndex, shouldConfigure))
                {
                    SkipInvalidFrameHeader();

                    return;
                }

                Debug.Assert(_frameHeader.FrameLength > 7);

                if (shouldConfigure)
                {
                    _configurationHandler(_frameHeader);
                    _isConfigured = true;
                }

                // Even better: the frame header is valid.  Now we need some data...

                EnsureBufferSpace(_frameHeader.FrameLength);
            }
        }

        void SkipInvalidFrameHeader()
        {
            if (0xff == _packet.Buffer[_startIndex + 1] &&
                0xf0 == (0xf0 & _packet.Buffer[_startIndex + 2]))
            {
                // _bufferEntry.Buffer[_startIndex] is already 0xff
                _packet.Buffer[_startIndex + 1] = _packet.Buffer[_startIndex + 2];
                _packet.Buffer[_startIndex + 2] = _packet.Buffer[_startIndex + 3];

                _index = _startIndex + 3;
                ++_badBytes;
            }
            else if (0xff == _packet.Buffer[_startIndex + 2] &&
                     0xf0 == (0xf0 & _packet.Buffer[_startIndex + 3]))
            {
                // _bufferEntry.Buffer[_startIndex] is already 0xff
                _packet.Buffer[_startIndex + 1] = _packet.Buffer[_startIndex + 3];

                _index = _startIndex + 2;
                _badBytes += 2;
            }
            else if (0xff == _packet.Buffer[_startIndex + 3])
            {
                // _bufferEntry.Buffer[_startIndex] is already 0xff

                _index = _startIndex + 1;
                _badBytes += 3;
            }
            else
            {
                _index = _startIndex;
                _badBytes += 4;
            }
        }
    }
}
