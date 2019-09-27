using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public class ContentLengthStream : AsyncReaderStream
    {
        readonly long? _contentLength;
        readonly IHttpReader _reader;

        public ContentLengthStream(IHttpReader reader, long? contentLength)
        {
            if (null == reader)
                throw new ArgumentNullException("reader");

            _reader = reader;
            _contentLength = contentLength;
        }

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (null == buffer)
                throw new ArgumentNullException("buffer");
            if (offset < 0 || offset > buffer.Length)
                throw new ArgumentOutOfRangeException("offset");
            if (count < 1 || count + offset > buffer.Length)
                throw new ArgumentOutOfRangeException("count");

            if (_contentLength.HasValue)
            {
                var remaining = _contentLength.Value - Position;

                if (count > remaining)
                    count = (int)remaining;
            }

            if (count < 1)
                return 0;

            var length = await _reader.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);

            //LoggerInstance.Debug("ContentLengthStream.ReadAsync() {0}/{1}", length, count);

            if (length > 0)
                IncrementPosition(length);

            return length;
        }
    }
}
