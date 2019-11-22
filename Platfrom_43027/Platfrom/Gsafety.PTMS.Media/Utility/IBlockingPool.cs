using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public interface IBlockingPool<TItem> : IDisposable
        where TItem : new()
    {
        Task<TItem> AllocateAsync(CancellationToken cancellationToken);
        void Free(TItem item);
    }
}
