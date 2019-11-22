using System;

namespace Gsafety.PTMS.Media.Builder
{
    public interface IBuilder : IDisposable
    {
        void Register<TService, TImplementation>()
            where TImplementation : TService;

        void RegisterSingleton<TService, TImplementation>()
            where TImplementation : TService;

        void RegisterSingleton<TService>(TService instance)
            where TService : class;

        void RegisterSingletonFactory<TService>(Func<TService> factory);

        void RegisterTransientFactory<TService>(Func<TService> factory);
    }

    public interface IBuilder<TBuild> : IBuilder
    {
        TBuild Create();
        void Destroy(TBuild instance);
    }
}
