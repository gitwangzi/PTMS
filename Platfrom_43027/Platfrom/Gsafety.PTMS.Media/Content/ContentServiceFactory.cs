using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.Content
{
    public interface IContentServiceFactory<TService, TParameter>
    {
        Task<TService> CreateAsync(TParameter parameter, ContentType contentType, CancellationToken cancellationToken);
    }

    public abstract class ContentServiceFactory<TService, TParameter> : IContentServiceFactory<TService, TParameter>
    {
        static readonly Task<TService> NoHandler = TaskExt.FromResult(default(TService));
        readonly IContentServiceFactoryFinder<TService, TParameter> _factoryFinder;

        protected ContentServiceFactory(IContentServiceFactoryFinder<TService, TParameter> factoryFinder)
        {
            if (null == factoryFinder)
                throw new ArgumentNullException("factoryFinder");

            _factoryFinder = factoryFinder;
        }

        public virtual Task<TService> CreateAsync(TParameter parameter, ContentType contentType, CancellationToken cancellationToken)
        {
            if (null == contentType)
                throw new ArgumentNullException("contentType");

            var factory = _factoryFinder.GetFactory(contentType);

            if (null != factory)
            {
                var task = factory.CreateAsync(parameter, contentType, cancellationToken);
                LoggerInstance.Debug(task.Status.ToString());
                return task;
            }

            return NoHandler;
        }

    }
}
