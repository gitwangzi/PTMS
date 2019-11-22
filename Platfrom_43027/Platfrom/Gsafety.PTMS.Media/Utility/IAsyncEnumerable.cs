using System;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public interface IAsyncEnumerable<T>
    {
        // Windows Phone 7 gives type load exceptions if we try "IAsyncEnumerable<out T>"

        IAsyncEnumerator<T> GetEnumerator();
    }

    public interface IAsyncEnumerator<T> : IDisposable
    {
        // Windows Phone 7 gives type load exceptions if we try "IAsyncEnumerator<out T>"

        T Current { get; }

        Task<bool> MoveNextAsync();
    }

    public static class AsyncEnumerableExtensions
    {
        public static async Task<T> FirstOrDefaultAsync<T>(this IAsyncEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!await enumerator.MoveNextAsync().ConfigureAwait(false))
                    return default(T);

                return enumerator.Current;
            }
        }
    }
}
