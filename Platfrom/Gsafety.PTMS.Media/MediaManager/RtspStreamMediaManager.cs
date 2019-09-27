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
using Gsafety.PTMS.Media.TransportStream;
using Gsafety.PTMS.Media.RTSP;
using System.Linq;
using Gsafety.PTMS.Media.Common.Loggers;
using Gsafety.PTMS.Media.RTSP.RTP;

namespace Gsafety.PTMS.Media.MediaManager
{
    public class RtspStreamMediaManager : IMediaManager
    {
        private RtspClient rtspClient;
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
        Action<object, RtpPacket, RtpClient.TransportContext> _dataHandle;
        private CancellationToken _rtspCancellationToken;

        readonly IWebMetadataFactory _webMetadataFactory;

        public RtspStreamMediaManager(Func<IBufferingManager> bufferingManagerFactory, IMediaParserFactory mediaParserFactory,
            IMediaStreamConfigurator mediaStreamConfigurator, IWebMetadataFactory webMetadataFactory)
        {
            if (null == bufferingManagerFactory)
                throw new ArgumentNullException("bufferingManagerFactory");
            if (null == mediaParserFactory)
                throw new ArgumentNullException("mediaParserFactory");
            if (null == mediaStreamConfigurator)
                throw new ArgumentNullException("mediaStreamConfigurator");
            if (null == webMetadataFactory)
                throw new ArgumentNullException("webMetadataFactory");

            _bufferingManagerFactory = bufferingManagerFactory;
            _mediaParserFactory = mediaParserFactory;
            _mediaStreamConfigurator = mediaStreamConfigurator;
            _webMetadataFactory = webMetadataFactory;

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
            if (cancellationToken.IsCancellationRequested)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            State = MediaManagerState.OpenMedia;

            CancellationTokenSource playCancellationTokenSource = null;
            Task playTask = null;

            try
            {
                var configurationTaskCompletionSource = new TaskCompletionSource<bool>();

                playCancellationTokenSource = new CancellationTokenSource();

                var localPlayCancellationTokenSource = playCancellationTokenSource;

                var cancelPlayTask = configurationTaskCompletionSource.Task
                    .ContinueWith(t =>
                    {
                        if (t.IsFaulted || t.IsCanceled)
                            localPlayCancellationTokenSource.Cancel();
                    });

                TaskCollector.Default.Add(cancelPlayTask, "RtspStreamMediaManager play cancellation");

                var playCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, playCancellationTokenSource.Token).Token;
                //var playCancellationToken = playCancellationTokenSource.Token;

                var uri = source.FirstOrDefault();

                playTask = TaskExt.Run(() => SimplePlayAsync(uri, configurationTaskCompletionSource, playCancellationToken), playCancellationToken);

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
                    return _mediaStreamConfigurator;
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                if (null != playCancellationTokenSource)
                    playCancellationTokenSource.Cancel();

                if (null != playTask)
                    TaskCollector.Default.Add(playTask, "RtspStreamMediaManager play task");
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
            {
                System.Diagnostics.Debug.WriteLine("befor playCancellationTokenSource.Cancel()");
                try
                {
                    //playCancellationTokenSource.Cancel(true);
                    playCancellationTokenSource.CancelAfter(0);
                }
                catch (System.Exception ex)
                {                	
                }
                System.Diagnostics.Debug.WriteLine("after playCancellationTokenSource.Cancel()");
            }

            var result = playTask ?? TplTaskExtensions.CompletedTask;
            try
            {
                System.Diagnostics.Debug.WriteLine("IsCompleted:" + result.IsCompleted);
            }
            catch (System.Exception ex)
            {
            }
            return result;
        }

        public Task CloseMediaAsync()
        {
            return StopMediaAsync(CancellationToken.None);
        }

        public Task<TimeSpan> SeekMediaAsync(TimeSpan position)
        {
            if (position.Ticks == 0)
            {
                return TaskExt.FromResult(position);
            }

            var task = rtspClient.SendPause(location: null, force: true);
            //var newTask = task.ContinueWith(t => rtspClient.StartPlaying());
            var newTask = task.ContinueWith(t => rtspClient.SendPlay(startTime: position, endTime: rtspClient.EndTime));
            return newTask.ContinueWith(t => position);
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

            if (rtspClient != null)
            {
                rtspClient.Dispose();
                rtspClient = null;
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
            LoggerInstance.Debug("RtspStreamMediaManager.ReportState() state {0} message {1}", _mediaState, _mediaStateMessage);

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

                LoggerInstance.Debug(string.Format("RtspStreamMediaState.SetMediaState() {0} -> {1}", _mediaState, state));

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

        async Task SimplePlayAsync(Uri url, TaskCompletionSource<bool> configurationTaskCompletionSource, CancellationToken cancellationToken)
        {
            try
            {
                LoggerInstance.Debug("Enter SimplePlayAsync()");

                _mediaStreamConfigurator.Initialize();

                _mediaStreamConfigurator.MediaManager = this;

                var contentType = new ContentType("MPEG-2 Transport Stream", ContentKind.Container, "video/MP2T", ".ts");

                var mediaParser = await _mediaParserFactory.CreateAsync(new MediaParserParameters(), contentType, cancellationToken).ConfigureAwait(false);

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

                    Task reader = null;

                    try
                    {
                        try
                        {
                            reader = ReadResponseAsync(url, mediaParser, throttle, cancellationToken);
                            await reader.ConfigureAwait(false);
                            reader = null;

                            await TaskExt.WhenAny(configurationTaskCompletionSource.Task, cancellationToken.AsTask()).ConfigureAwait(false);

                            cancellationToken.ThrowIfCancellationRequested();

                            await _mediaStreamConfigurator.PlayAsync(mediaParser.MediaStreams, _mediaStreamConfigurator.TotalTime, cancellationToken).ConfigureAwait(false);

                            State = MediaManagerState.Playing;
                        }
                        finally
                        {
                        }
                    }
                    catch (OperationCanceledException)
                    {
                    }
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

            LoggerInstance.Debug("Leave SimplePlayAsync()");
        }

        async Task ReadResponseAsync(Uri url, IMediaParser mediaParser, QueueThrottle throttle, CancellationToken cancellationToken)
        {
            rtspClient = new RtspClient(url, RtspClient.ClientProtocolType.Tcp);
            rtspClient.Client.NotContiniousReceiveFinishedAction = () =>
            {

            };

            _dataHandle = (sender, packet, tc) =>
            {
                var buffer = packet.PayloadData.ToArray();
                mediaParser.ProcessData(buffer, 0, buffer.Length);
            };

            rtspClient.Client.RtpPacketReceieved += new RtpClient.RtpPacketHandler(_dataHandle);

            await rtspClient.StartPlaying(SeekTarget, _mediaStreamConfigurator.TotalTime);

            if (rtspClient.EndTime != System.TimeSpan.FromMilliseconds(System.Threading.Timeout.Infinite))
            {
                _mediaStreamConfigurator.TotalTime = rtspClient.EndTime;
            }

            _rtspCancellationToken = cancellationToken;
            cancellationToken.Register(RtspSendTeardown);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    RtpClient.TransportContext tc = null;
                    while (true)
                    {
                        Thread.Sleep(1000);

                        if (tc == null)
                        {
                            tc = rtspClient.Client.GetTransportContexts().FirstOrDefault();

                            if (tc.IsContinious == true)
                            {
                                return;
                            }
                        }

                        if (tc != null)
                        {
                            if (tc.TimeRemaining < TimeSpan.Zero)
                            {
                                try
                                {
                                    LoggerInstance.Info("ProcessEndOfData");
                                    mediaParser.ProcessEndOfData();
                                }
                                catch (System.Exception ex)
                                {
                                    return;
                                }

                                break;
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }, cancellationToken);
        }

        private void RtspSendTeardown()
        {
            var response = rtspClient.SendTeardown().Result;
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
