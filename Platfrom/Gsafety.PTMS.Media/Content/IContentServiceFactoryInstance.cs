﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Content
{
    public interface IContentServiceFactoryInstance<TService, TParameter>
    {
        ICollection<ContentType> KnownContentTypes { get; }
        Task<TService> CreateAsync(TParameter parameter, ContentType contentType, CancellationToken cancellationToken);
    }
}
