using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class WorkBufferBlockingPool : IBlockingPool<WorkBuffer>
    {
        BlockingPool<WorkBuffer> _pool;

        public WorkBufferBlockingPool(int poolSize)
        {
            _pool = new BlockingPool<WorkBuffer>(poolSize);
        }

        #region IBlockingPool<WorkBuffer> Members

        public void Dispose()
        {
            var pool = _pool;

            _pool = null;

            pool.Dispose();
        }

#if DEBUG
        public async Task<WorkBuffer> AllocateAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            var item = await _pool.AllocateAsync(cancellationToken).ConfigureAwait(false);

            Debug.Assert(null == item.Metadata, "Pending metadata");

            return item;
        }
#else
        public Task<WorkBuffer> AllocateAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            return _pool.AllocateAsync(cancellationToken);
        }
#endif

        public void Free(WorkBuffer item)
        {
            ThrowIfDisposed();

            item.Metadata = null;

            _pool.Free(item);
        }

        #endregion

        void ThrowIfDisposed()
        {
            if (null == _pool)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
