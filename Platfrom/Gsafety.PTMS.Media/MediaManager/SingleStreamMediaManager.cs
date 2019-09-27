using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Media.Buffering;
using Gsafety.PTMS.Media.Content;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Metadata;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Web;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media.MediaManager
{
    public class SingleStreamMediaManager : IMediaManager
    {
        readonly Func<IBufferingManager> _bufferingManagerFactory;
        readonly object _lock = new object();
        readonly IMediaParserFactory _mediaParserFactory;
        readonly IMediaStreamConfigurator _mediaStreamConfigurator;
        readonly SignalTask _reportStateTask;
        Task _playTask;
        MediaManagerState _mediaState;
        int _isDisposed;
        string _mediaStateMessage;
        CancellationTokenSource _playCancellationTokenSource;

        readonly IWebMetadataFactory _webMetadataFactory;
        readonly IWebReaderManager _webReaderManager;

        public SingleStreamMediaManager(Func<IBufferingManager> bufferingManagerFactory, IMediaParserFactory mediaParserFactory,
            IMediaStreamConfigurator mediaStreamConfigurator, IWebMetadataFactory webMetadataFactory, IWebReaderManager webReaderManager)
        {
            if (null == bufferingManagerFactory)
                throw new ArgumentNullException("bufferingManagerFactory");
            if (null == mediaParserFactory)
                throw new ArgumentNullException("mediaParserFactory");
            if (null == mediaStreamConfigurator)
                throw new ArgumentNullException("mediaStreamConfigurator");
            if (null == webMetadataFactory)
                throw new ArgumentNullException("webMetadataFactory");
            if (null == webReaderManager)
                throw new ArgumentNullException("webReaderManager");

            _bufferingManagerFactory = bufferingManagerFactory;
            _mediaParserFactory = mediaParserFactory;
            _mediaStreamConfigurator = mediaStreamConfigurator;
            _webMetadataFactory = webMetadataFactory;
            _webReaderManager = webReaderManager;

            _reportStateTask = new SignalTask(ReportState);
        }

        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 1))
                return;

            Dispose(true);

            GC.SuppressFinalize(this);
        }

        public TimeSpan? SeekTarget { get; set; }
        public ContentType ContentType { get; set; }
        public ContentType StreamContentType { get; set; }

        public MediaManagerState State
        {
            get { lock (_lock) return _mediaState; }
            private set { SetMediaState(value, null); }
        }

        public Task PlayingTask
        {
            get
            {
                Task playingTask = null;

                lock (_lock)
                {
                    playingTask = _playTask;
                }

                return playingTask ?? TplTaskExtensions.CompletedTask;
            }
        }

        public async Task<IMediaStreamConfigurator> OpenMediaAsync(ICollection<Uri> source, CancellationToken cancellationToken)
        {
            State = MediaManagerState.OpenMedia;

            var response = new WebResponse();

            using (var rootWebReader = _webReaderManager.CreateRootReader())
            {
                foreach (var url in source)
                {
                    IWebReader webReader = null;
                    IWebStreamResponse webStream = null;
                    CancellationTokenSource playCancellationTokenSource = null;
                    Task playTask = null;

                    try
                    {
                        webReader = rootWebReader.CreateChild(url, ContentKind.Unknown, ContentType ?? StreamContentType);

                        webStream = await webReader.GetWebStreamAsync(null, false, cancellationToken, response: response).ConfigureAwait(false);

                        if (!webStream.IsSuccessStatusCode)
                            continue;

                        var contentType = response.ContentType;

                        if (null == contentType)
                            continue;

                        if (ContentKind.Playlist == contentType.Kind)
                            throw new FileNotFoundException("Content not supported with this media manager");

                        var configurationTaskCompletionSource = new TaskCompletionSource<bool>();

                        playCancellationTokenSource = new CancellationTokenSource();

                        var localPlayCancellationTokenSource = playCancellationTokenSource;

                        var cancelPlayTask = configurationTaskCompletionSource.Task
                            .ContinueWith(t =>
                            {
                                if (t.IsFaulted || t.IsCanceled)
                                    localPlayCancellationTokenSource.Cancel();
                            });

                        TaskCollector.Default.Add(cancelPlayTask, "SingleStreamMediaManager play cancellation");

                        var localWebReader = webReader;
                        var localWebStream = webStream;
                        var playCancellationToken = playCancellationTokenSource.Token;

                        playTask = TaskExt.Run(() => SimplePlayAsync(contentType, localWebReader, localWebStream, response, configurationTaskCompletionSource, playCancellationToken), playCancellationToken);

                        lock (_lock)
                        {
                            _playCancellationTokenSource = playCancellationTokenSource;
                            playCancellationTokenSource = null;

                            _playTask = playTask;
                            playTask = null;
                        }

                        var isConfigured = await configurationTaskCompletionSource.Task.ConfigureAwait(false);

                        if (isConfigured)
                        {
                            webReader = null;
                            webStream = null;

                            return _mediaStreamConfigurator;
                        }
                    }
                    finally
                    {
                        if (null != webStream)
                            webStream.Dispose();

                        if (null != webReader)
                            webReader.Dispose();

                        if (null != playCancellationTokenSource)
                            playCancellationTokenSource.Cancel();

                        if (null != playTask)
                            TaskCollector.Default.Add(playTask, "SingleStreamMediaManager play task");
                    }
                }
            }

            throw new FileNotFoundException();
        }

        public Task StopMediaAsync(CancellationToken cancellationToken)
        {
            Task playTask;
            CancellationTokenSource playCancellationTokenSource;

            lock (_lock)
            {
                playTask = _playTask;
                playCancellationTokenSource = _playCancellationTokenSource;
            }

            if (null != playCancellationTokenSource)
                playCancellationTokenSource.Cancel();

            return playTask ?? TplTaskExtensions.CompletedTask;
        }

        public Task CloseMediaAsync()
        {
            return StopMediaAsync(CancellationToken.None);
        }

        public Task<TimeSpan> SeekMediaAsync(TimeSpan position)
        {
            return TaskExt.FromResult(position);
        }

        public event EventHandler<MediaManagerStateEventArgs> OnStateChange;

        protected virtual void Dispose(bool disposing)
        {
            LoggerInstance.Debug("SingleStreamMediaManager.Dispose(bool)");

            if (!disposing)
                return;

            if (null != OnStateChange)
            {
                LoggerInstance.Debug("SingleStreamMediaManager.Dispose(bool): OnStateChange is not null");

                if (Debugger.IsAttached)
                    Debugger.Break();

                OnStateChange = null;
            }

            _mediaStreamConfigurator.MediaManager = null;

            _reportStateTask.Dispose();

            CancellationTokenSource pcts;

            lock (_lock)
            {
                pcts = _playCancellationTokenSource;
                _playCancellationTokenSource = null;
            }

            if (null != pcts)
                pcts.Dispose();
        }

        Task ReportState()
        {
            LoggerInstance.Debug("SingleStreamMediaManager.ReportState() state {0} message {1}", _mediaState, _mediaStateMessage);

            MediaManagerState state;
            string message;

            lock (_lock)
            {
                state = _mediaState;
                message = _mediaStateMessage;
                _mediaStateMessage = null;
            }

            var handlers = OnStateChange;

            if (null != handlers)
                handlers(this, new MediaManagerStateEventArgs(state, message));

            if (null != message)
            {
                var mss = _mediaStreamConfigurator;

                if (null != mss)
                    mss.ReportError(message);
            }

            return TplTaskExtensions.CompletedTask;
        }

        void SetMediaState(MediaManagerState state, string message)
        {
            lock (_lock)
            {
                if (state == _mediaState)
                    return;

                LoggerInstance.Debug("SingleStreamMediaState.SetMediaState() {0} -> {1}", _mediaState, state);

                _mediaState = state;

                if (null != message)
                    _mediaStateMessage = message;
            }

            _reportStateTask.Fire();
        }

        void CancelPlayback()
        {
            CancellationTokenSource playCancellationTokenSource;

            lock (_lock)
            {
                playCancellationTokenSource = _playCancellationTokenSource;
            }

            if (null == playCancellationTokenSource)
                return;

            if (!playCancellationTokenSource.IsCancellationRequested)
                playCancellationTokenSource.Cancel();
        }

        async Task SimplePlayAsync(ContentType contentType, IWebReader webReader, IWebStreamResponse webStreamResponse, WebResponse webResponse, TaskCompletionSource<bool> configurationTaskCompletionSource, CancellationToken cancellationToken)
        {
            try
            {
                _mediaStreamConfigurator.Initialize();

                _mediaStreamConfigurator.MediaManager = this;

                var mediaParser = await _mediaParserFactory.CreateAsync(new MediaParserParameters(), contentType, cancellationToken).ConfigureAwait(false);

                if (null == mediaParser)
                    throw new NotSupportedException("Unsupported content type: " + contentType);

                State = MediaManagerState.Opening;

                EventHandler configurationComplete = null;

                configurationComplete = (sender, args) =>
                {
                    // ReSharper disable once AccessToModifiedClosure
                    mediaParser.ConfigurationComplete -= configurationComplete;

                    configurationTaskCompletionSource.TrySetResult(true);
                };

                mediaParser.ConfigurationComplete += configurationComplete;

                using (var bufferingManager = _bufferingManagerFactory())
                {
                    var throttle = new QueueThrottle();

                    bufferingManager.Initialize(throttle, _mediaStreamConfigurator.CheckForSamples);

                    mediaParser.Initialize(bufferingManager);

                    var streamMetadata = _webMetadataFactory.CreateStreamMetadata(webResponse);

                    mediaParser.InitializeStream(streamMetadata);

                    Task reader = null;

                    try
                    {
                        using (webReader)
                        {
                            try
                            {
                                if (null == webStreamResponse)
                                    webStreamResponse = await webReader.GetWebStreamAsync(null, false, cancellationToken, response: webResponse).ConfigureAwait(false);

                                reader = ReadResponseAsync(mediaParser, webStreamResponse, webResponse, throttle, cancellationToken);

                                await TaskExt.WhenAny(configurationTaskCompletionSource.Task, cancellationToken.AsTask()).ConfigureAwait(false);

                                cancellationToken.ThrowIfCancellationRequested();

                                await _mediaStreamConfigurator.PlayAsync(mediaParser.MediaStreams, new TimeSpan(1, 0, 0), cancellationToken).ConfigureAwait(false);

                                State = MediaManagerState.Playing;

                                await reader.ConfigureAwait(false);

                                reader = null;
                            }
                            finally
                            {
                                if (null != webStreamResponse)
                                    webStreamResponse.Dispose();
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    { }
                    catch (Exception ex)
                    {
                        var message = ex.ExtendedMessage();

                        LoggerInstance.Debug("SingleStreamMediaManager.SimplePlayAsync() failed: " + message);

                        SetMediaState(MediaManagerState.Error, message);
                    }

                    State = MediaManagerState.Closing;

                    if (null != reader)
                    {
                        try
                        {
                            await reader.ConfigureAwait(false);
                        }
                        catch (OperationCanceledException)
                        { }
                        catch (Exception ex)
                        {
                            LoggerInstance.Debug("SingleStreamMediaManager.SimplePlayAsync() reader failed: " + ex.ExtendedMessage());
                        }
                    }

                    mediaParser.ConfigurationComplete -= configurationComplete;

                    mediaParser.EnableProcessing = false;
                    mediaParser.FlushBuffers();

                    bufferingManager.Flush();

                    bufferingManager.Shutdown(throttle);

                    await _mediaStreamConfigurator.CloseAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("SingleStreamMediaManager.SimplePlayAsync() cleanup failed: " + ex.ExtendedMessage());
            }

            _mediaStreamConfigurator.MediaManager = null;

            if (!configurationTaskCompletionSource.Task.IsCompleted)
                configurationTaskCompletionSource.TrySetCanceled();

            State = MediaManagerState.Closed;

            await _reportStateTask.WaitAsync().ConfigureAwait(false);
        }

        async Task ReadResponseAsync(IMediaParser mediaParser, IWebStreamResponse webStreamResponse, WebResponse webResponse, QueueThrottle throttle, CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("SingleStreamMediaManager.ReadResponseAsync()");

            var buffer = new byte[16 * 1024];

            var cancellationTask = cancellationToken.AsTask();

            try
            {
                var segmentMetadata = _webMetadataFactory.CreateSegmentMetadata(webResponse);

                mediaParser.StartSegment(segmentMetadata);

                using (var stream = await webStreamResponse.GetStreamAsync(cancellationToken).ConfigureAwait(false))
                {
                    _mediaStreamConfigurator.TotalTime = TimeSpan.FromMinutes(2);

                    for (; ; )
                    {
                        var waitTask = throttle.WaitAsync();

                        if (!waitTask.IsCompleted)
                        {
                            await TaskExt.WhenAny(waitTask, cancellationTask).ConfigureAwait(false);

                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        var length = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

                        if (length <= 0)
                            return;

                        mediaParser.ProcessData(buffer, 0, length);
                    }
                }
            }
            finally
            {
                mediaParser.ProcessEndOfData();

                LoggerInstance.Debug("SingleStreamMediaManager.ReadResponseAsync() done");
            }
        }

        sealed class QueueThrottle : IQueueThrottling
        {
            readonly AsyncManualResetEvent _event = new AsyncManualResetEvent();

            public void Pause()
            {
                _event.Reset();
            }

            public void Resume()
            {
                _event.Set();
            }

            public Task WaitAsync()
            {
                return _event.WaitAsync();
            }
        }
    }
}
