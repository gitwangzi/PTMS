using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.Builder;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.MediaManager;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public abstract class MediaStreamFacadeBase<TMediaStreamSource> : IMediaStreamFacadeBase<TMediaStreamSource>
        where TMediaStreamSource : class
    {
        readonly AsyncLock _asyncLock = new AsyncLock();
        readonly CancellationTokenSource _disposeCancellationTokenSource = new CancellationTokenSource();
        readonly object _lock = new object();
        CancellationTokenSource _closeCancellationTokenSource;
        int _isDisposed;
        IMediaManager _mediaManager;

        protected MediaStreamFacadeBase(IBuilder<IMediaManager> mediaManagerBuilder)
        {
            if (mediaManagerBuilder == null)
                throw new ArgumentNullException("mediaManagerBuilder");

            Builder = mediaManagerBuilder;

            // ReSharper disable once PossiblyMistakenUseOfParamsMethod
            _closeCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_disposeCancellationTokenSource.Token);
        }

        IMediaManager MediaManager
        {
            get
            {
                lock (_lock)
                {
                    return _mediaManager;
                }
            }
            set
            {
                SetMediaManager(value);
            }
        }

        void SetMediaManager(IMediaManager value)
        {
            if (ReferenceEquals(_mediaManager, value))
                return;

            IMediaManager mediaManager;

            lock (_lock)
            {
                mediaManager = _mediaManager;
                _mediaManager = value;
            }

            if (null != mediaManager)
            {
                LoggerInstance.Debug("**** MediaStreamFacadeBase.SetMediaManager() _mediaManager was not null");

                CleanupMediaManager(mediaManager);
            }
        }

        void ThrowIfDisposed()
        {
            if (0 != _isDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        protected virtual void Dispose(bool disposing)
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.Dispose({0})", disposing);

            if (!disposing)
                return;

            if (!_closeCancellationTokenSource.IsCancellationRequested)
                _closeCancellationTokenSource.Cancel();

            if (!_disposeCancellationTokenSource.IsCancellationRequested)
                _disposeCancellationTokenSource.Cancel();

            _asyncLock.LockAsync(CancellationToken.None).Wait();

            StateChange = null;

            IMediaManager mediaManager;

            lock (_lock)
            {
                mediaManager = _mediaManager;
                _mediaManager = null;
            }

            if (null != mediaManager)
                CleanupMediaManager(mediaManager);

            Builder.DisposeSafe();

            _asyncLock.Dispose();

            _closeCancellationTokenSource.Dispose();

            _disposeCancellationTokenSource.Dispose();
        }

        void CleanupMediaManager(IMediaManager mediaManager)
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.CleanupMediaManager()");

            if (null == mediaManager)
                return;

            mediaManager.OnStateChange -= MediaManagerOnStateChange;

            mediaManager.DisposeSafe();

            Builder.Destroy(mediaManager);

            LoggerInstance.Debug("MediaStreamFacadeBase.CleanupMediaManager() completed");
        }

        protected async Task<IMediaManager> CreateMediaManagerAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                var mediaManager = MediaManager;

                if (null != mediaManager)
                    throw new InvalidOperationException("A MediaManager already exists");

                cancellationToken.ThrowIfCancellationRequested();

                mediaManager = CreateMediaManager();

                Debug.Assert(null == _mediaManager);

                MediaManager = mediaManager;

                return mediaManager;
            }
        }

        IMediaManager CreateMediaManager()
        {
            LoggerInstance.Debug("Enter MediaStreamFacadeBase.CreateMediaManager()");

            Debug.Assert(null == MediaManager);

            var mediaManager = Builder.Create();

            mediaManager.OnStateChange += MediaManagerOnStateChange;

            LoggerInstance.Debug("Leave MediaStreamFacadeBase.CreateMediaManager()");

            return mediaManager;
        }

        async Task StopMediaAsync(IMediaManager mediaManager, CancellationToken cancellationToken)
        {
            LoggerInstance.Debug("Enter MediaStreamFacadeBase.StopMediaAsync()");

            if (MediaManager == null)
            {
                return;
            }

            using (await _asyncLock.LockAsync(cancellationToken).ConfigureAwait(false))
            {
                var mm = MediaManager;

                if (null == mm || !ReferenceEquals(mm, mediaManager))
                    return;

                await mm.StopMediaAsync(cancellationToken).ConfigureAwait(false);
            }

            LoggerInstance.Debug("Leave MediaStreamFacadeBase.StopMediaAsync()");
        }

        async Task CloseMediaManagerAsync(IMediaManager mediaManager)
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.CloseMediaAsync()");

            if (mediaManager == null)
            {
                return;
            }

            using (await _asyncLock.LockAsync(CancellationToken.None).ConfigureAwait(false))
            {
                await UnlockedCloseMediaManagerAsync(mediaManager).ConfigureAwait(false);
            }
        }

        async Task UnlockedCloseMediaManagerAsync(IMediaManager mediaManager)
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.UnlockedCloseMediaAsync()");

            IMediaManager mm;

            lock (_lock)
            {
                mm = _mediaManager;

                if (null == mm || !ReferenceEquals(mm, mediaManager))
                    return;

                _mediaManager = null;
            }

            try
            {
                if (!_closeCancellationTokenSource.IsCancellationRequested)
                    //_closeCancellationTokenSource.Cancel();
                    _closeCancellationTokenSource.CancelAfter(0);
            }
            catch (Exception)
            {
            }
            try
            {
                System.Diagnostics.Debug.WriteLine("mm.CloseMediaAsync() Begin");

                await mm.CloseMediaAsync().ConfigureAwait(false);

                System.Diagnostics.Debug.WriteLine("mm.CloseMediaAsync() End");
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("MediaStreamFacadeBase.UnlockedCloseMediaManagerAsync() Media manager close failed: " + ex.Message);
            }

            System.Diagnostics.Debug.WriteLine("CleanupMediaManager Begin");
            CleanupMediaManager(mm);
            System.Diagnostics.Debug.WriteLine("CleanupMediaManager End");
        }

        async void MediaManagerOnStateChange(object sender, MediaManagerStateEventArgs e)
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.MediaManagerOnStateChange() to {0}: {1}", e.State, e.Message);

            if (e.State == MediaManagerState.Closed)
            {
                try
                {
                    await StopMediaAsync((IMediaManager)sender, _closeCancellationTokenSource.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                { }
                catch (Exception ex)
                {
                    LoggerInstance.Debug("MediaStreamFacadeBase.MediaManagerOnStateChange() stop failed: " + ex.Message);
                }
            }

            var stateChange = StateChange;

            if (null == stateChange)
                return;

            try
            {
                stateChange(this, e);
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("MediaStreamFacadeBase.MediaManagerOnStateChange() Exception in StateChange event handler: " + ex.Message);
            }
        }

        #region IMediaStreamFacadeBase<TMediaStreamSource> Members

        public bool IsDisposed
        {
            get
            {
                return 0 != _isDisposed;
            }
        }

        public Task PlayingTask
        {
            get
            {
                IMediaManager mediaManager;

                lock (_lock)
                {
                    mediaManager = _mediaManager;
                }

                if (null == mediaManager)
                    return TplTaskExtensions.CompletedTask;

                return mediaManager.PlayingTask;
            }
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 1))
                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public ContentType ContentType { get; set; }

        /// <inheritdoc />
        public ContentType StreamContentType { get; set; }

        private TimeSpan? _seekTarget;
        public TimeSpan? SeekTarget
        {
            get
            {
                var mediaManager = MediaManager;

                return mediaManager == null ? _seekTarget : mediaManager.SeekTarget;
            }
            set
            {
                ThrowIfDisposed();

                _seekTarget = value;

                var mediaManager = MediaManager;

                if (null == mediaManager)
                    return;

                mediaManager.SeekTarget = value;
            }
        }

        public MediaManagerState State
        {
            get
            {
                var mediaManager = MediaManager;

                if (null == mediaManager)
                    return MediaManagerState.Closed;

                return mediaManager.State;
            }
        }

        public IBuilder<IMediaManager> Builder { get; set; }

        public event EventHandler<MediaManagerStateEventArgs> StateChange;

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.StopAsync()");

            ThrowIfDisposed();

            using (var linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _closeCancellationTokenSource.Token))
            {
                await StopMediaAsync(MediaManager, linkedToken.Token).ConfigureAwait(false);
            }
        }

        public async Task CloseAsync()
        {
            LoggerInstance.Debug("MediaStreamFacadeBase.CloseAsync()");
            try
            {
                await CloseMediaManagerAsync(MediaManager).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {

            }
        }

        public virtual async Task<TMediaStreamSource> CreateMediaStreamSourceAsync(Uri source, CancellationToken cancellationToken)
        {
            if (null != source && !source.IsAbsoluteUri)
            {
                throw new ArgumentException("source must be absolute: " + source);
            }

            LoggerInstance.Debug("Enter MediaStreamFacadeBase.CreateMediaStreamSourceAsync() With URL:" + source.ToString());

            Exception exception = null;

            var closeCts = CancellationTokenSource.CreateLinkedTokenSource(_disposeCancellationTokenSource.Token);

            if (!_closeCancellationTokenSource.IsCancellationRequested)
                _closeCancellationTokenSource.Cancel();

            try
            {
                CancellationTokenSource linkedToken = null;
                try
                {
                    linkedToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, closeCts.Token);

                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(MediaStreamFacadeSettings.Parameters.CreateTimeout);
                        try
                        {
                            var s = linkedToken.Token;
                            linkedToken.Cancel();
                        }
                        catch (System.Exception ex)
                        {
                        }
                    });

                    var mediaManager = MediaManager;

                    if (null != mediaManager)
                        await StopMediaAsync(mediaManager, linkedToken.Token).ConfigureAwait(false);

                    _closeCancellationTokenSource.Dispose();
                    _closeCancellationTokenSource = closeCts;

                    if (null == source)
                        return null;

                    mediaManager = MediaManager ?? await CreateMediaManagerAsync(linkedToken.Token).ConfigureAwait(false);

                    mediaManager.ContentType = ContentType;
                    mediaManager.StreamContentType = StreamContentType;
                    mediaManager.SeekTarget = _seekTarget;

                    try
                    {
                        var configurator = await mediaManager.OpenMediaAsync(new[] { source }, linkedToken.Token).ConfigureAwait(false);

                        var mss = await configurator.CreateMediaStreamSourceAsync<TMediaStreamSource>(linkedToken.Token).ConfigureAwait(false);

                        return mss;
                    }
                    catch (System.Exception ex)
                    {
                        exception = ex;
                        LoggerInstance.Debug("MediaStreamFacadeBase.CreateAsync() failed: " + ex.ExtendedMessage());
                    }
                }
                finally
                {
                    try
                    {
                        if (linkedToken != null)
                        {
                            if (linkedToken.IsCancellationRequested)
                            {
                            }
                            else
                            {
                                linkedToken.DisposeSafe();
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                exception = ex;
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("MediaStreamFacadeBase.CreateAsync() failed: " + ex.ExtendedMessage());

                //exception = new AggregateException(ex.Message, ex);
                exception = ex;
            }

            await CloseAsync().ConfigureAwait(false);

            throw exception;
        }

        #endregion
    }

    public static class MediaStreamFacadeExtensions
    {
        public static void RequestStop(this IMediaStreamFacadeBase mediaStreamFacade)
        {
            var stopTask = RequestStopAsync(mediaStreamFacade, TimeSpan.FromSeconds(5), CancellationToken.None);

            TaskCollector.Default.Add(stopTask, "MediaStreamFacade RequestStop");
        }

        public static async Task<bool> RequestStopAsync(this IMediaStreamFacadeBase mediaStreamFacade,
            TimeSpan timeout, CancellationToken cancellationToken)
        {
            // ReSharper disable once PossiblyMistakenUseOfParamsMethod
            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            {
                cts.CancelAfter(timeout);

                {
                    try
                    {
                        await mediaStreamFacade.StopAsync(cts.Token).ConfigureAwait(false);

                        return true;
                    }
                    catch (OperationCanceledException)
                    {
                        LoggerInstance.Debug(!cancellationToken.IsCancellationRequested ? "RequestStop timeout" : "RequestStop canceled");
                    }
                    catch (Exception ex)
                    {
                        LoggerInstance.Debug("RequestStop failed: " + ex.ExtendedMessage());
                    }
                }

                return false;
            }
        }

        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IMediaManagerParameters parameters)
        {
            mediaStreamFacade.Builder.RegisterSingleton(parameters);
        }

        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IBufferingPolicy policy)
        {
            mediaStreamFacade.Builder.RegisterSingleton(policy);
        }

        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IMediaStreamConfigurator mediaStreamConfigurator)
        {
            mediaStreamFacade.Builder.RegisterSingleton(mediaStreamConfigurator);
        }

        public static void SetParameter(this IMediaStreamFacadeBase mediaStreamFacade, IApplicationInformation applicationInformation)
        {
            mediaStreamFacade.Builder.RegisterSingleton(applicationInformation);
        }
    }
}
