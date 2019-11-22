using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Gsafety.PTMS.Media.Pes;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Platform.Silverlight;
using System.Linq;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public sealed class TsMediaStreamSource : MediaStreamSource, IDisposable
    {
        public TimeSpan? TotalTime { get; set; }

        [Flags]
        public enum Operation
        {
            None = 0,
            Audio = 1,
            Video = 2,
            Seek = 4
        }

#if DEBUG
        MediaStreamFsm _mediaStreamFsm = new MediaStreamFsm();
#endif
        static readonly Dictionary<MediaSampleAttributeKeys, string> NoMediaSampleAttributes = new Dictionary<MediaSampleAttributeKeys, string>();
        readonly PesStream _pesStream = new PesStream();
        readonly object _stateLock = new object();
        readonly SingleThreadSignalTaskScheduler _taskScheduler;

        float _bufferingProgress;
        TaskCompletionSource<object> _closeCompleted;
        bool _isClosed = true;
        int _isDisposed;
        volatile int _pendingOperations;
        TimeSpan _pendingSeekTarget;
        TimeSpan? _seekTarget;
        SourceState _state;

        MediaStreamDescription _videoStreamDescription;
        MediaStreamDescription _audioStreamDescription;

        Operation _streamOpenFlags;

        IStreamSource _videoStreamSource;
        IStreamSource _audioStreamSource;
        readonly IMediaStreamControl _streamControl;

        IStreamSource VideoStreamSource
        {
            get { return _videoStreamSource; }
            set
            {
                lock (_stateLock)
                {
                    if (null == value)
                    {
                        _streamOpenFlags &= ~Operation.Video;
                        _videoStreamSource = null;
                    }
                    else
                    {
                        _streamOpenFlags |= Operation.Video;
                        _videoStreamSource = value;
                    }
                }
            }
        }

        IStreamSource AudioStreamSource
        {
            get { return _audioStreamSource; }
            set
            {
                lock (_stateLock)
                {
                    if (null == value)
                        _streamOpenFlags &= ~Operation.Audio;
                    else
                        _streamOpenFlags |= Operation.Audio;

                    _audioStreamSource = value;
                }
            }
        }

        public TsMediaStreamSource(IMediaStreamControl mediaStreamControl)
        {
            if (null == mediaStreamControl)
                throw new ArgumentNullException("mediaStreamControl");

            _streamControl = mediaStreamControl;

            AudioBufferLength = 150;     // 150ms of internal buffering, instead of 1s.

#if DEBUG
            _mediaStreamFsm.Reset();
#endif

            _taskScheduler = new SingleThreadSignalTaskScheduler("TsMediaStreamSource", SignalHandler);
        }

        bool IsDisposed
        {
            get { return 0 != _isDisposed; }
        }

        SourceState State
        {
            get { lock (_stateLock) return _state; }
            set
            {
                lock (_stateLock)
                {
                    if (_state == value)
                        return;

                    _state = value;
                }

                CheckPending();
            }
        }

        public TimeSpan? SeekTarget
        {
            get { lock (_stateLock) return _seekTarget; }
            set { lock (_stateLock) _seekTarget = value; }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (0 != Interlocked.Exchange(ref _isDisposed, 1))
                return;

            LoggerInstance.Debug("TsMediaStreamSource.Dispose()");
            ValidateEvent(MediaStreamFsm.MediaEvent.DisposeCalled);

            TaskCompletionSource<object> closeCompleted;

            lock (_stateLock)
            {
                _isClosed = true;
                closeCompleted = _closeCompleted;
                _closeCompleted = null;
            }

            if (null != closeCompleted)
                closeCompleted.TrySetResult(string.Empty);

            if (null != _taskScheduler)
                _taskScheduler.Dispose();

            ForceClose();

            _pesStream.Dispose();
        }

        void ForceClose()
        {
            var operations = HandleOperation(Operation.Video | Operation.Audio | Operation.Seek);

            //LoggerInstance.Debug("TsMediastreamSource.ForceClose() " + operations);

            if (0 != (operations & Operation.Seek))
            {
                ValidateEvent(MediaStreamFsm.MediaEvent.CallingReportSeekCompleted);
                ReportSeekCompleted(0);
            }

            if (0 != (operations & Operation.Video) && null != _videoStreamDescription)
                SendLastStreamSample(_videoStreamDescription);

            if (0 != (operations & Operation.Audio) && null != _audioStreamDescription)
                SendLastStreamSample(_audioStreamDescription);
        }

        public void ReportError(string message)
        {
            var task = Task.Factory.StartNew(() => ErrorOccurred(message), CancellationToken.None, TaskCreationOptions.None, _taskScheduler);

            TaskCollector.Default.Add(task, "TsMediaStreamSource ReportError");
        }

        public Task CloseAsync()
        {
            LoggerInstance.Debug("TsMediaStreamSource.CloseAsync(): close {0}",
                null == _closeCompleted ? "<none>" : _closeCompleted.Task.Status.ToString());

            TaskCompletionSource<object> closeCompleted;

            bool closedState;

            lock (_stateLock)
            {
                _isClosed = true;

                closedState = SourceState.Closed == _state;

                if (!closedState)
                    _state = SourceState.WaitForClose;

                closeCompleted = _closeCompleted;

                if (null != closeCompleted && closeCompleted.Task.IsCompleted)
                {
                    closeCompleted = null;
                    _closeCompleted = null;
                }
            }

            if (0 == _streamOpenFlags || closedState)
            {
                if (null != closeCompleted)
                    closeCompleted.TrySetResult(string.Empty);

                return TplTaskExtensions.CompletedTask;
            }

            if (null == closeCompleted)
                return TplTaskExtensions.CompletedTask;

            CheckPending();

            var timeout = TaskExt.Delay(7 * 1000)
                .ContinueWith(
                    t =>
                    {
                        if (closeCompleted.TrySetCanceled())
                        {
                            LoggerInstance.Debug("TsMediaStreamSource.CloseAsync() close timeout (remember to set MediaElement.Source to null before removing it from the visual tree)");

                            FireCloseMediaHandler();
                        }
                    });

            TaskCollector.Default.Add(timeout, "TsMediaStreamSource CloseAsync timeout");

            return closeCompleted.Task;
        }

        [Conditional("DEBUG")]
        public void ValidateEvent(MediaStreamFsm.MediaEvent mediaEvent)
        {
#if DEBUG
            _mediaStreamFsm.ValidateEvent(mediaEvent);
#endif
        }

        public void CheckForSamples()
        {
            //LoggerInstance.Debug("TsMediaStreamSource.CheckForSamples(): " + (Operation)_pendingOperations);

            if (0 == (_pendingOperations & (int)(Operation.Audio | Operation.Video)))
                return;

            _taskScheduler.Signal();
        }

        void CheckPending()
        {
            if (0 == _pendingOperations)
                return;

            _taskScheduler.Signal();
        }

        async Task SeekHandler()
        {
            TimeSpan seekTimestamp;

            _taskScheduler.ThrowIfNotOnThread();

            lock (_stateLock)
            {
                if (_isClosed)
                    return;

                seekTimestamp = _pendingSeekTarget;
            }

            try
            {
                var position = await _streamControl.SeekAsync(seekTimestamp, CancellationToken.None).ConfigureAwait(true);

                _taskScheduler.ThrowIfNotOnThread();

                if (_isClosed)
                    return;

                ValidateEvent(MediaStreamFsm.MediaEvent.CallingReportSeekCompleted);
                ReportSeekCompleted(position.Ticks);

                LoggerInstance.Debug("TsMediaStreamSource.SeekHandler({0}) completed, actual: {1}", seekTimestamp, position);

                State = SourceState.Play;
                _bufferingProgress = -1;
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("TsMediaStreamSource.SeekHandler({0}) failed: {1}", seekTimestamp, ex.Message);
                ErrorOccurred("Seek failed: " + ex.Message);

                _taskScheduler.ThrowIfNotOnThread();
            }

            CheckPending();
        }

        void SignalHandler()
        {
            //LoggerInstance.Debug("TsMediaStreamSource.SignalHandler() pending {0}", (Operation)_pendingOperations);

            _taskScheduler.ThrowIfNotOnThread();

            var previousOperations = Operation.None;
            var requestedOperations = Operation.None;

            try
            {
                if (_isClosed)
                {
                    ForceClose();

                    return;
                }

                for (; ; )
                {
                    if (0 != HandleOperation(Operation.Seek))
                    {
                        // Request the previous operation(s) again if we
                        // detect a possible Seek/GetSample race.
                        if (Operation.None != previousOperations)
                            requestedOperations |= previousOperations;

                        var task = SeekHandler();

                        TaskCollector.Default.Add(task, "TsMediaStreamSource.SignalHandler SeekHandler()");

                        return;
                    }

                    if (SourceState.Play != State)
                        return;

                    previousOperations = HandleOperation(Operation.Video | Operation.Audio);

                    requestedOperations |= previousOperations;

                    if (0 == requestedOperations)
                        return;

                    var reportBufferingMask = _streamOpenFlags;

                    var canCallReportBufferingProgress = reportBufferingMask == (requestedOperations & reportBufferingMask);

                    var gotPackets = false;

                    if (0 != (requestedOperations & Operation.Video))
                    {
                        if (null != VideoStreamSource)
                        {
                            if (SendStreamSample(VideoStreamSource, _videoStreamDescription, canCallReportBufferingProgress))
                            {
                                requestedOperations &= ~Operation.Video;
                                gotPackets = true;
                            }
                        }
                    }

                    if (0 != (requestedOperations & Operation.Audio))
                    {
                        if (null != AudioStreamSource)
                        {
                            if (SendStreamSample(AudioStreamSource, _audioStreamDescription, canCallReportBufferingProgress))
                            {
                                requestedOperations &= ~Operation.Audio;
                                gotPackets = true;
                            }
                        }
                    }

                    if (!gotPackets)
                        return;
                }
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("TsMediaStreamSource.SignalHandler() failed: " + ex.ExtendedMessage());
            }
            finally
            {
                if (0 != requestedOperations)
                {
                    LoggerInstance.Debug("TsMediaStreamSource.SignalHandler() re-requesting " + requestedOperations);
                    RequestOperation(requestedOperations);
                }
            }
        }

        void ThrowIfDisposed()
        {
            if (0 == _isDisposed)
                return;

            throw new ObjectDisposedException(GetType().Name);
        }

        bool SendStreamSample(IStreamSource source, MediaStreamDescription mediaStreamDescription, bool canCallReportBufferingProgress)
        {
            _taskScheduler.ThrowIfNotOnThread();

            var packet = source.GetNextSample();

            if (null == packet)
            {
                if (source.IsEof)
                    return SendLastStreamSample(mediaStreamDescription);

                if (canCallReportBufferingProgress)
                {
                    var progress = source.BufferingProgress;

                    if (progress.HasValue)
                    {
                        if (Math.Abs(_bufferingProgress - progress.Value) < 0.05)
                            return false;

                        //System.Diagnostics.Debug.WriteLine("Sample {0} buffering {1:F2}%", mediaStreamDescription.Type, progress * 100);

                        _bufferingProgress = progress.Value;

                        ValidateEvent(MediaStreamFsm.MediaEvent.CallingReportSampleCompleted);
                        ReportGetSampleProgress(progress.Value);
                    }
                    else
                    {
                        LoggerInstance.Debug("Sample {0} not buffering", mediaStreamDescription.Type);

                        // Try again, data might have arrived between the last call to GetNextSample() and
                        // when we checked the buffering progress.
                        packet = source.GetNextSample();
                    }
                }

                if (null == packet)
                    return false;
            }

            _bufferingProgress = -1;

            try
            {
                _pesStream.Packet = packet;

                var sample = new MediaStreamSample(mediaStreamDescription, _pesStream, 0, packet.Length,
                    packet.PresentationTimestamp.Ticks, NoMediaSampleAttributes);

                //System.Diagnostics.Debug.WriteLine("Sample {0} at {1}", sample.MediaStreamDescription.Type, TimeSpan.FromTicks(sample.Timestamp));

                ValidateEvent(MediaStreamFsm.MediaEvent.CallingReportSampleCompleted);
                ReportGetSampleCompleted(sample);
            }
            finally
            {
                _pesStream.Packet = null;

                source.FreeSample(packet);
            }

            return true;
        }

        bool SendLastStreamSample(MediaStreamDescription mediaStreamDescription)
        {
            _taskScheduler.ThrowIfNotOnThread();

            ReportGetSampleProgress(1);

            var sample = new MediaStreamSample(mediaStreamDescription, null, 0, 0, 0, NoMediaSampleAttributes);

            LoggerInstance.Debug("Sample {0} is null", mediaStreamDescription.Type);

            switch (mediaStreamDescription.Type)
            {
                case MediaStreamType.Audio:
                    AudioStreamSource = null;
                    break;
                case MediaStreamType.Video:
                    VideoStreamSource = null;
                    break;
                default:
                    Debug.Assert(false, "Unknown stream type: " + mediaStreamDescription.Type);
                    break;
            }

            var allClosed = null == VideoStreamSource && null == AudioStreamSource;

            if (allClosed)
            {
                LoggerInstance.Debug("TsMediaStreamSource.SendLastStreamSample() All streams closed");

                lock (_stateLock)
                {
                    _isClosed = true;

                    if (SourceState.Closed != _state)
                        _state = SourceState.WaitForClose;
                }
            }

            ValidateEvent(MediaStreamFsm.MediaEvent.CallingReportSampleCompleted);
            ReportGetSampleCompleted(sample);

            if (allClosed)
                ValidateEvent(MediaStreamFsm.MediaEvent.StreamsClosed);

            return true;
        }

        /// <summary>
        ///     The <see cref="T:System.Windows.Controls.MediaElement" /> calls this method to ask the
        ///     <see
        ///         cref="T:System.Windows.Media.MediaStreamSource" />
        ///     to open the media.
        /// </summary>
        protected override async void OpenMediaAsync()
        {
            LoggerInstance.Debug("TsMediaStreamSource.OpenMediaAsync()");
            ValidateEvent(MediaStreamFsm.MediaEvent.OpenMediaAsyncCalled);

            ThrowIfDisposed();

            lock (_stateLock)
            {
                _isClosed = false;

                _state = SourceState.Open;

                Debug.Assert(null == _closeCompleted, "TsMediaStreamSource.OpenMediaAsync() stream is already playing");

                _closeCompleted = new TaskCompletionSource<object>();
            }

            _bufferingProgress = -1;

            try
            {
                var configuration = await _streamControl.OpenAsync(CancellationToken.None).ConfigureAwait(false);

                Configure(configuration);
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("TsMediaStreamSource.OpenMediaAsync() failed: " + ex.Message);
                ReportError("Unable to open stream " + ex.Message);
            }
        }

        void Configure(IMediaStreamConfiguration configuration)
        {
            var descriptions = configuration.Descriptions;

            LoggerInstance.Debug("TsMediaStreamSource: ReportOpenMediaCompleted ({0} streams)", descriptions.Count);

            VideoStreamSource = configuration.VideoStreamSource;
            AudioStreamSource = configuration.AudioStreamSource;

            var attributes = configuration.Attributes;

            foreach (var kv in attributes)
                LoggerInstance.Debug("TsMediaStreamSource: ReportOpenMediaCompleted {0} = {1}", kv.Key, kv.Value);

            foreach (var m in descriptions)
            {
                switch (m.Type)
                {
                    case MediaStreamType.Video:
                        _videoStreamDescription = m;
                        break;
                    case MediaStreamType.Audio:
                        _audioStreamDescription = m;
                        break;
                }
            }

            var canSeek = configuration.Duration.HasValue;

            var task = Task.Factory.StartNew(() =>
            {
                _taskScheduler.ThrowIfNotOnThread();
                ValidateEvent(canSeek ? MediaStreamFsm.MediaEvent.CallingReportOpenMediaCompleted : MediaStreamFsm.MediaEvent.CallingReportOpenMediaCompletedLive);
                ReportOpenMediaCompleted(attributes, descriptions);
                State = canSeek ? SourceState.Seek : SourceState.Play;
            }, CancellationToken.None, TaskCreationOptions.None, _taskScheduler);

            TaskCollector.Default.Add(task, "TsMediaStreamSource CompleteConfigure");

            //ReportGetSampleProgress(0);
        }

        /// <summary>
        ///     The <see cref="T:System.Windows.Controls.MediaElement" /> calls this method to ask the
        ///     <see
        ///         cref="T:System.Windows.Media.MediaStreamSource" />
        ///     to seek to the nearest randomly accessible point before the specified time. Developers respond to this method by
        ///     calling
        ///     <see
        ///         cref="M:System.Windows.Media.MediaStreamSource.ReportSeekCompleted(System.Int64)" />
        ///     and by ensuring future calls to
        ///     <see
        ///         cref="M:System.Windows.Media.MediaStreamSource.ReportGetSampleCompleted(System.Windows.Media.MediaStreamSample)" />
        ///     will return samples from that point in the media.
        /// </summary>
        /// <param name="seekToTime">
        ///     The time as represented by 100 nanosecond increments to seek to. This is typically measured
        ///     from the beginning of the media file.
        /// </param>
        protected override void SeekAsync(long seekToTime)
        {
            var seekTimestamp = TimeSpan.FromTicks(seekToTime);

            LoggerInstance.Debug("TsMediaStreamSource.SeekAsync({0})", seekTimestamp);
            ValidateEvent(MediaStreamFsm.MediaEvent.SeekAsyncCalled);

            StartSeek(seekTimestamp);
        }

        public async void ManualSeekAsync(long seekToTime)
        {
            var seekTimestamp = new TimeSpan(seekToTime * 1000 * 1000 * 10);

            LoggerInstance.Debug("TsMediaStreamSource.SeekAsync({0})", seekTimestamp);

            var position = await _streamControl.SeekAsync(seekTimestamp, CancellationToken.None).ConfigureAwait(true);
        }

        void StartSeek(TimeSpan seekTimestamp)
        {
            lock (_stateLock)
            {
                if (_isClosed)
                    return;

                _state = SourceState.Seek;

                _pendingSeekTarget = _seekTarget ?? seekTimestamp;
            }

            RequestOperationAndSignal(Operation.Seek);
        }

        /// <summary>
        ///     The <see cref="T:System.Windows.Controls.MediaElement" /> calls this method to ask the
        ///     <see
        ///         cref="T:System.Windows.Media.MediaStreamSource" />
        ///     to prepare the next
        ///     <see
        ///         cref="T:System.Windows.Media.MediaStreamSample" />
        ///     of the requested stream type for the media pipeline.  Developers can respond to this method by calling either
        ///     <see
        ///         cref="M:System.Windows.Media.MediaStreamSource.ReportGetSampleCompleted(System.Windows.Media.MediaStreamSample)" />
        ///     or
        ///     <see
        ///         cref="M:System.Windows.Media.MediaStreamSource.ReportGetSampleProgress(System.Double)" />
        ///     .
        /// </summary>
        /// <param name="mediaStreamType">
        ///     The description of the stream that the next sample should come from which will be either
        ///     <see
        ///         cref="F:System.Windows.Media.MediaStreamType.Audio" />
        ///     or <see cref="F:System.Windows.Media.MediaStreamType.Video" /> .
        /// </param>
        protected override void GetSampleAsync(MediaStreamType mediaStreamType)
        {
            //LoggerInstance.Debug("TsMediaStreamSource.GetSampleAsync({0})", mediaStreamType);

            var op = LookupOperation(mediaStreamType);

            RequestOperationAndSignal(op);
        }

        /// <summary>
        ///     Called when a stream switch is requested on the <see cref="T:System.Windows.Controls.MediaElement" />.
        /// </summary>
        /// <param name="mediaStreamDescription"> The stream switched to. </param>
        protected override void SwitchMediaStreamAsync(MediaStreamDescription mediaStreamDescription)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     The <see cref="T:System.Windows.Controls.MediaElement" /> can call this method to request information about the
        ///     <see
        ///         cref="T:System.Windows.Media.MediaStreamSource" />
        ///     . Developers respond to this method by calling
        ///     <see
        ///         cref="M:System.Windows.Media.MediaStreamSource.ReportGetDiagnosticCompleted(System.Windows.Media.MediaStreamSourceDiagnosticKind,System.Int64)" />
        ///     .
        /// </summary>
        /// <param name="diagnosticKind">
        ///     A member of the <see cref="T:System.Windows.Media.MediaStreamSourceDiagnosticKind" /> enumeration describing what
        ///     type of information is desired.
        /// </param>
        protected override void GetDiagnosticAsync(MediaStreamSourceDiagnosticKind diagnosticKind)
        {
            LoggerInstance.Debug("TsMediaStreamSource.GetDiagnosticAsync({0})", diagnosticKind);

            throw new NotImplementedException();
        }

        /// <summary>
        ///     The <see cref="T:System.Windows.Controls.MediaElement" /> can call this method when going through normal shutdown
        ///     or as a result of an error. This lets the developer perform any needed cleanup of the
        ///     <see
        ///         cref="T:System.Windows.Media.MediaStreamSource" />
        ///     .
        /// </summary>
        protected override void CloseMedia()
        {
            LoggerInstance.Debug("TsMediaStreamSource.CloseMedia()");
            ValidateEvent(MediaStreamFsm.MediaEvent.CloseMediaCalled);

            lock (_stateLock)
            {
                _isClosed = true;

                _state = SourceState.Closed;
            }

            var task = Task.Factory.StartNew(CloseMediaHandler, CancellationToken.None, TaskCreationOptions.None, _taskScheduler);

            TaskCollector.Default.Add(task, "TsMediaStreamSource CloseMedia");
        }

        void CloseMediaHandler()
        {
            LoggerInstance.Debug("TsMediaStreamSource.CloseMediaHandler()");

            _taskScheduler.ThrowIfNotOnThread();

            TaskCompletionSource<object> closeCompleted;

            lock (_stateLock)
            {
                closeCompleted = _closeCompleted;
            }

            if (null != closeCompleted)
                closeCompleted.TrySetResult(string.Empty);

            FireCloseMediaHandler();
        }

        void FireCloseMediaHandler()
        {
            var task = _streamControl.CloseAsync(CancellationToken.None);

            TaskCollector.Default.Add(task, "TsMediaStreamSource CloseMediaHandler");
        }

        static Operation LookupOperation(MediaStreamType mediaStreamType)
        {
            switch (mediaStreamType)
            {
                case MediaStreamType.Audio:
                    return Operation.Audio;
                case MediaStreamType.Video:
                    return Operation.Video;
            }
            return 0;
        }

        void RequestOperationAndSignal(Operation operation)
        {
            if (RequestOperation(operation))
                _taskScheduler.Signal();
        }

        bool RequestOperation(Operation operation)
        {
            //LoggerInstance.Debug("TsMediaStreamSource.RequestOperation({0}) pending {1}", operation, _pendingOperations);

            var op = (int)operation;
            var current = _pendingOperations;

            for (; ; )
            {
                var value = current | op;

                if (value == current)
                    return false;

#pragma warning disable 0420
                var existing = Interlocked.CompareExchange(ref _pendingOperations, value, current);
#pragma warning restore 0420

                if (existing == current)
                    return true;

                current = existing;
            }
        }

        Operation HandleOperation(Operation operation)
        {
            var op = (int)operation;
            var current = _pendingOperations;

            for (; ; )
            {
                var value = current & ~op;

                if (value == current)
                    //不是当前的operator
                    return Operation.None;

#pragma warning disable 0420
                var existing = Interlocked.CompareExchange(ref _pendingOperations, value, current);
#pragma warning restore 0420

                if (existing == current)
                    return (Operation)(current & op);

                current = existing;
            }
        }
    }
}
