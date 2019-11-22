using System;
using System.Threading;
using System.Threading.Tasks;

namespace Gsafety.PTMS.Media.Utility
{
    public sealed class CancellationTaskCompletionSource<TItem> : TaskCompletionSource<TItem>
    {
        readonly Action<CancellationTaskCompletionSource<TItem>> _cancellationAction;
        CancellationTokenRegistration _cancellationTokenRegistration;
        int _isDisposed;

        public CancellationTaskCompletionSource(Action<CancellationTaskCompletionSource<TItem>> cancellationAction, CancellationToken cancellationToken)
        {
            if (null == cancellationAction)
                throw new ArgumentNullException("cancellationAction");

            _cancellationAction = cancellationAction;
            _cancellationTokenRegistration = cancellationToken.Register(obj => ((CancellationTaskCompletionSource<TItem>)obj).Cancel(), this);
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 1))
                return;

            _cancellationTokenRegistration.DisposeSafe();

            TrySetCanceled();
        }

        void Cancel()
        {
            _cancellationAction(this);

            Dispose();
        }
    }
}
