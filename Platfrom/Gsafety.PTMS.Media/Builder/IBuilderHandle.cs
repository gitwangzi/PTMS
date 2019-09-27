using System;

namespace Gsafety.PTMS.Media.Builder
{
    public interface IBuilderHandle<TBuild> : IDisposable
    {
        TBuild Instance { get; }
    }
}
