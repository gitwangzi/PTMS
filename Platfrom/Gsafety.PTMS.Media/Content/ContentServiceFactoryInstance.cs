using System;

namespace Gsafety.PTMS.Media.Content
{
    public abstract class ContentServiceFactoryInstance<TServiceImplementation, TService, TParameter>
        : ContentServiceFactoryBase<TServiceImplementation, TService, TParameter>
        where TServiceImplementation : TService
    {
        readonly Func<TServiceImplementation> _factory;

        protected ContentServiceFactoryInstance(Func<TServiceImplementation> factory)
        {
            if (null == factory)
                throw new ArgumentNullException("factory");

            _factory = factory;
        }

        protected virtual TServiceImplementation Create()
        {
            return _factory();
        }

        protected virtual void Initialize(TServiceImplementation instance, TParameter parameter)
        { }

        protected override TServiceImplementation Create(TParameter parameter, ContentType contentType)
        {
            var instance = Create();

            Initialize(instance, parameter);

            return instance;
        }
    }
}
