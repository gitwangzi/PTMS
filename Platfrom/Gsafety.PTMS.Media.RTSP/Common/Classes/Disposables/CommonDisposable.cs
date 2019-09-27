using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.RTSP.Common
{
    /// <summary>
    /// Provides a default implementation of the <see cref="BaseDisposable"/>
    /// </summary>
    public class CommonDisposable : BaseDisposable
    {
        //Could store Created time?

        //Should be on base class...

        //public readonly DateTime Created = DateTime.UtcNow;

        /// <summary>
        /// Creates an instance
        /// </summary>
        /// <param name="shouldDispose">Indicates if <see cref="Dispose"/> will change the state of the instance</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public CommonDisposable(bool shouldDispose = true) : base(shouldDispose) { }
    }
}


//Tests