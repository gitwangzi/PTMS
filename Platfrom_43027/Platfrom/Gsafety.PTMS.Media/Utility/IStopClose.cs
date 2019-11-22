using System;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public interface IStopClose : IDisposable
    {
        Task StopAsync();
        Task CloseAsync();
    }
}
