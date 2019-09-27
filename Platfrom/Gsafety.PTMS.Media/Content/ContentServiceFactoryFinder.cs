using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Gsafety.PTMS.Media.Content
{
    public interface IContentServiceFactoryFinder<TService, TParameter>
    {
        IContentServiceFactoryInstance<TService, TParameter> GetFactory(ContentType contentType);
        void Register(ContentType contentType, IContentServiceFactoryInstance<TService, TParameter> factory);
        void Deregister(ContentType contentType);
    }

    public class ContentServiceFactoryFinder<TService, TParameter> : IContentServiceFactoryFinder<TService, TParameter>
    {
        volatile Dictionary<ContentType, IContentServiceFactoryInstance<TService, TParameter>> _factories;

        public ContentServiceFactoryFinder(IEnumerable<IContentServiceFactoryInstance<TService, TParameter>> factoryInstances)
        {
            _factories = factoryInstances
                .SelectMany(fi => fi.KnownContentTypes,
                    (fi, contentType) => new
                                         {
                                             ContentType = contentType,
                                             Instance = fi
                                         })
                .ToDictionary(v => v.ContentType, v => v.Instance);
        }


        public IContentServiceFactoryInstance<TService, TParameter> GetFactory(ContentType contentType)
        {
            IContentServiceFactoryInstance<TService, TParameter> factory;

            if (_factories.TryGetValue(contentType, out factory))
                return factory;

            return null;
        }

        public void Register(ContentType contentType, IContentServiceFactoryInstance<TService, TParameter> factory)
        {
            SafeChangeFactories(factories => factories[contentType] = factory);
        }

        public void Deregister(ContentType contentType)
        {
            SafeChangeFactories(factories => factories.Remove(contentType));
        }

        void SafeChangeFactories(Action<Dictionary<ContentType, IContentServiceFactoryInstance<TService, TParameter>>> changeAction)
        {
            var oldFactories = _factories;

            for (; ; )
            {
                var newFactories = new Dictionary<ContentType, IContentServiceFactoryInstance<TService, TParameter>>(oldFactories);

                changeAction(newFactories);

#pragma warning disable 420
                var factories = Interlocked.CompareExchange(ref _factories, newFactories, oldFactories);
#pragma warning restore 420

                if (oldFactories == factories)
                    return;

                oldFactories = factories;
            }
        }
    }
}
