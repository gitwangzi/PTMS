using Gsafety.PTMS.Media.Common.Loggers;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public class ChunkedStream : AsyncReaderStream
    {
        readonly IHttpReader _reader;

        long _chunkRead;
        long? _chunkSize;
        bool _eof;

        public ChunkedStream(IHttpReader reader)
        {
            if (null == reader)
                throw new ArgumentNullException("reader");

            _reader = reader;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (null == buffer)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 1 || count + offset > buffer.Length)
                throw new ArgumentOutOfRangeException("count");

            var totalLength = 0;
            var count0 = count;

            for (; ; )
            {
                var length = await ReadOneAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);

                if (length < 1)
                    break;

                totalLength += length;

                count -= length;

                if (count <= 0)
                    break;

                offset += length;

                if (!_reader.HasData)
                    break;
            }

            LoggerInstance.Debug("ChunkedStream.ReadAsync() {0}/{1}", totalLength, count0);

            return totalLength;
        }

        async Task<int> ReadOneAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (_eof)
                return 0;

            if (_chunkSize.HasValue && _chunkRead == _chunkSize.Value)
            {
                // We need to consume the chunk-data's trailing CRLF
                var blankLine = await _reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);

                if (null == blankLine)
                {
                    _eof = true;
                    return 0;
                }

                if (!string.IsNullOrEmpty(blankLine))
                {
                    _eof = true;
                    throw new WebException("Invalid chunked encoding");
                }

                _chunkSize = null;
            }

            if (!_chunkSize.HasValue)
            {
                // We need a line with a chunk size
                var chunkSizeLine = await _reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);

                if (string.IsNullOrEmpty(chunkSizeLine))
                {
                    _eof = true;
                    return 0;
                }

                var semicolon = chunkSizeLine.IndexOf(';');

                var chunkSizeString = semicolon > 0 ? chunkSizeLine.Substring(0, semicolon) : chunkSizeLine;

                long chunkSize;
                if (!long.TryParse(chunkSizeString, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out chunkSize))
                    throw new WebException("invalid chunk size: " + chunkSizeLine);

                if (chunkSize < 0)
                    throw new WebException("invalid chunk size: " + chunkSizeLine);

                if (0 == chunkSize)
                {
                    _eof = true;
                    return 0;
                }

                _chunkSize = chunkSize;
                _chunkRead = 0;
            }

            var remaining = (int)(_chunkSize.Value - _chunkRead);

            if (count > remaining)
                count = remaining;

            if (count < 1)
                return 0;

            var length = await _reader.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);

            LoggerInstance.Debug("ChunkedStream.ReadOneAsync() {0}/{1}", length, count);

            if (length < 1)
            {
                _eof = true;
                return 0;
            }

            Debug.Assert(_chunkRead + length <= _chunkSize);

            _chunkRead += length;

            IncrementPosition(length);

            return length;
        }
    }
}
