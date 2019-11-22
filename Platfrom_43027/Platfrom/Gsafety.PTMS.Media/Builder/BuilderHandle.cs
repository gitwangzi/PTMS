using System;
using Autofac;
using Gsafety.PTMS.Media.MediaParser;

namespace Gsafety.PTMS.Media.Builder
{
    public sealed class BuilderHandle<TBuild> : IBuilderHandle<TBuild>
    {
        readonly ILifetimeScope _scope;
        TBuild _instance;

        public BuilderHandle(ILifetimeScope scope)
        {
            if (null == scope)
                throw new ArgumentNullException("scope");

            _scope = scope;
        }

        public TBuild Instance
        {
            get
            {
                if (Equals(default(TBuild), _instance))
                    _instance = _scope.Resolve<TBuild>();

                return _instance;
            }
        }

        public void Dispose()
        {
            using (_scope)
            { }
        }

    }
}
