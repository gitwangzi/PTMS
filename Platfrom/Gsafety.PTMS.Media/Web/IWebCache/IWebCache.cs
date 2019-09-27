using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Web
{
    public interface IWebCache
    {
        IWebReader WebReader { get; }

        Task<TCached> ReadAsync<TCached>(Func<Uri, byte[], TCached> factory, CancellationToken cancellationToken, WebResponse webResponse = null)
            where TCached : class;
    }
}
