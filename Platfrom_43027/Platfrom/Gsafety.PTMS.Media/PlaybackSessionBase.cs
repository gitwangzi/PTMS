using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public abstract class PlaybackSessionBase<TMediaSource> : IDisposable
        where TMediaSource : class
    {
#if DEBUG
        // ReSharper disable once StaticFieldInGenericType
        static int _idCount;
        readonly int _id = Interlocked.Increment(ref _idCount);
#endif
        readonly TaskCompletionSource<TMediaSource> _mediaSourceTaskCompletionSource = new TaskCompletionSource<TMediaSource>();
        readonly IMediaStreamFacadeBase<TMediaSource> _mediaStreamFacade;
        readonly CancellationTokenSource _playingCancellationTokenSource = new CancellationTokenSource();
        int _isDisposed;

        protected PlaybackSessionBase(IMediaStreamFacadeBase<TMediaSource> mediaStreamFacade)
        {
            if (null == mediaStreamFacade)
                throw new ArgumentNullException("mediaStreamFacade");

            _mediaStreamFacade = mediaStreamFacade;
        }

        public ContentType ContentType { get; set; }

        protected IMediaStreamFacadeBase<TMediaSource> MediaStreamFacade
        {
            get { return _mediaStreamFacade; }
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 1))
                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            _mediaSourceTaskCompletionSource.TrySetCanceled();

            _playingCancellationTokenSource.CancelDisposeSafe();
        }

        public virtual Task PlayAsync(Uri source, CancellationToken cancellationToken)
        {
            LoggerInstance.Debug("PlaybackSessionBase.PlayAsync() " + this);

            var playingTask = PlayerAsync(source, cancellationToken);

            TaskCollector.Default.Add(playingTask, "StreamingMediaPlugin PlayerAsync");

            return MediaStreamFacade.PlayingTask;
        }

        async Task PlayerAsync(Uri source, CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("PlaybackSessionBase.PlayerAsync() " + this);

            try
            {
                using (var createMediaCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_playingCancellationTokenSource.Token, cancellationToken))
                {
                    MediaStreamFacade.ContentType = ContentType;

                    var mss = await MediaStreamFacade.CreateMediaStreamSourceAsync(source, createMediaCancellationTokenSource.Token).ConfigureAwait(false);

                    if (!_mediaSourceTaskCompletionSource.TrySetResult(mss))
                        throw new OperationCanceledException();
                }

                return;
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                LoggerInstance.Debug("PlaybackSessionBase.PlayerAsync() failed: " + ex.ExtendedMessage());
            }

            try
            {
                _mediaSourceTaskCompletionSource.TrySetCanceled();

                await MediaStreamFacade.CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("PlaybackSessionBase.PlayerAsync() cleanup failed: " + ex.ExtendedMessage());
            }

            //LoggerInstance.Debug("PlaybackSessionBase.PlayerAsync() completed " + this);
        }

        public virtual async Task<TMediaSource> GetMediaSourceAsync(CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("PlaybackSessionBase.GetMediaSourceAsync() " + this);

            return await _mediaSourceTaskCompletionSource.Task.WithCancellation(cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("PlaybackSessionBase.StopAsync() " + this);

            await MediaStreamFacade.StopAsync(cancellationToken).ConfigureAwait(false);

            await MediaStreamFacade.PlayingTask.ConfigureAwait(false);
        }

        public virtual async Task CloseAsync()
        {
            //LoggerInstance.Debug("PlaybackSessionBase.CloseAsync() " + this);

            if (!_playingCancellationTokenSource.IsCancellationRequested)
                _playingCancellationTokenSource.Cancel();

            await MediaStreamFacade.PlayingTask.ConfigureAwait(false);
        }

        public virtual async Task OnMediaFailedAsync()
        {
            LoggerInstance.Debug("PlaybackSessionBase.OnMediaFailedAsync() " + this);

            try
            {
                await CloseAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("PlaybackSessionBase.OnMediaFailedAsync() CloseAsync() failed: " + ex.Message);
            }
        }

        public override string ToString()
        {
#if DEBUG
            return String.Format("Playback ID {0} IsCompleted {1}", _id, MediaStreamFacade.PlayingTask.IsCompleted);
#else
            return String.Format("Playback IsCompleted " + _mediaStreamFacade.PlayingTask.IsCompleted);
#endif
        }
    }
}
