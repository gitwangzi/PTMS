using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Web.HttpConnection
{
    public abstract class AsyncReaderStream : Stream
    {
        long _position;

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { return _position; }
            set { throw new NotSupportedException(); }
        }

        public override void Flush()
        { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return ReadAsync(buffer, offset, count, CancellationToken.None).Result;
        }

        //TODO:自己添加的
        public virtual Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var task = Task.Factory.FromAsync<byte[], int, int, int>(BeginRead, EndRead, buffer, offset, count, new object());
            return task;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        protected void IncrementPosition(long length)
        {
            if (length < 0)
                throw new ArgumentException("length cannot be negative", "length");

            _position += length;
        }
    }
}
