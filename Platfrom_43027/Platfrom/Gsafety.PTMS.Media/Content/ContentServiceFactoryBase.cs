using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Content
{
    public abstract class ContentServiceFactoryBase<TServiceImplementation, TService, TParameter>
        : IContentServiceFactoryInstance<TService, TParameter>
        where TServiceImplementation : TService
    {

        public abstract ICollection<ContentType> KnownContentTypes { get; }

        public virtual Task<TService> CreateAsync(TParameter parameter, ContentType contentType, CancellationToken cancellationToken)
        {
            var instance = Create(parameter, contentType);

            return TaskExt.FromResult<TService>(instance);
        }

        protected abstract TServiceImplementation Create(TParameter parameter, ContentType contentType);
    }
}
