using System;
using System.Threading;
using Autofac;
using Autofac.Core;
using Gsafety.PTMS.Media.Utility;

namespace Gsafety.PTMS.Media.Builder
{
    public abstract class BuilderBase : IBuilder
    {
        readonly ContainerBuilder _containerBuilder = new ContainerBuilder();
        readonly object _lock = new object();
        IContainer _container;
        bool _isDirty;
        int _isDisposed;

        public ContainerBuilder ContainerBuilder
        {
            get
            {
                if (null != _container)
                    throw new InvalidOperationException("The builder is in use");

                return _containerBuilder;
            }
        }

        protected IContainer Container
        {
            get
            {
                var newContainer = default(IContainer);

                for (; ; )
                {
                    lock (_lock)
                    {
                        var container = _container;

                        if (_isDirty)
                        {
                            LockedCleanupContainer();
                            container = null;
                        }

                        if (null != container)
                        {
                            if (null != newContainer)
                                newContainer.DisposeSafe();

                            return container;
                        }

                        if (null != newContainer)
                        {
                            _container = newContainer;
                            return newContainer;
                        }
                    }

                    newContainer = ContainerBuilder.Build();
                }
            }
        }

        public void Register<TService, TImplementation>()
            where TImplementation : TService
        {
            ChangeBuilder();

            ContainerBuilder.RegisterType<TImplementation>().As<TService>();
        }

        public void RegisterSingleton<TService, TImplementation>()
            where TImplementation : TService
        {
            ChangeBuilder();

            ContainerBuilder.RegisterType<TImplementation>().As<TService>().SingleInstance();
        }

        public void RegisterSingleton<TService>(TService instance)
            where TService : class
        {
            ChangeBuilder();

            ContainerBuilder.RegisterInstance(instance).ExternallyOwned();
        }

        public void RegisterSingletonFactory<TService>(Func<TService> factory)
        {
            ChangeBuilder();

            ContainerBuilder.Register(ctx => factory()).SingleInstance();
        }

        public void RegisterTransientFactory<TService>(Func<TService> factory)
        {
            ChangeBuilder();

            ContainerBuilder.Register(ctx => factory()).ExternallyOwned();
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 0))
                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        void LockedCleanupContainer()
        {
            _isDirty = false;

            var container = _container;

            if (null == container)
                return;

            _container = null;

            container.DisposeSafe();
        }

        void ChangeBuilder()
        {
            if (0 != _isDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            lock (_lock)
            {
                if (null != _container)
                    throw new InvalidOperationException("The builder is in use");

                _isDirty = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            IContainer container;

            lock (_lock)
            {
                container = _container;
                _container = null;
            }

            using (container)
            { }
        }
    }

    public static class BuilderBaseExtensions
    {
        public static void RegisterModule(this BuilderBase builder, IModule module)
        {
            builder.ContainerBuilder.RegisterModule(module);
        }

        public static void RegisterModule<TModule>(this BuilderBase builder)
            where TModule : IModule, new()
        {
            builder.ContainerBuilder.RegisterModule<TModule>();
        }
    }

    public class BuilderBase<TBuild> : BuilderBase, IBuilder<TBuild>
    {
        IBuilderHandle<TBuild> _handle;

        protected BuilderBase(params IModule[] modules)
        {
            if (null != modules && modules.Length > 0)
            {
                foreach (var module in modules)
                    ContainerBuilder.RegisterModule(module);
            }
        }

        public TBuild Create()
        {
            if (null != _handle)
                throw new InvalidOperationException("The builder is in use");

            var handle = new BuilderHandle<TBuild>(Container.BeginLifetimeScope("builder-scope"));

            if (null != Interlocked.CompareExchange(ref _handle, handle, null))
            {
                handle.DisposeSafe();

                throw new InvalidOperationException("The builder is in use");
            }

            return handle.Instance;
        }

        public void Destroy(TBuild instance)
        {
            var handle = Interlocked.Exchange(ref _handle, null);

            if (null == handle)
                throw new InvalidOperationException("No handle");

            if (!ReferenceEquals(instance, handle.Instance))
                throw new InvalidOperationException("Wrong instance");

            handle.DisposeSafe();
        }
    }
}
