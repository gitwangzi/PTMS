using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Gsafety.PTMS.Media.Configuration;
using Gsafety.PTMS.Media.MediaManager;
using Gsafety.PTMS.Media.MediaParser;
using Gsafety.PTMS.Media.Utility;
using Gsafety.PTMS.Media.Common.Loggers;

namespace Gsafety.PTMS.Media
{
    public sealed class MediaStreamConfigurator : IMediaStreamConfigurator, IMediaStreamControl
    {
        static readonly TimeSpan OpenTimeout = TimeSpan.FromSeconds(20);
        readonly Func<TsMediaStreamSource> _mediaStreamSourceFactory;
        readonly object _streamConfigurationLock = new object();
        MediaStreamDescription _audioStreamDescription;
        IStreamSource _audioStreamSource;
        IMediaManager _mediaManager;
        TsMediaStreamSource _mediaStreamSource;
        TaskCompletionSource<IMediaStreamConfiguration> _openCompletionSource;
        TaskCompletionSource<object> _playingCompletionSource;
        MediaStreamDescription _videoStreamDescription;
        IStreamSource _videoStreamSource;

        public MediaStreamConfigurator()
        {

        }

        public MediaStreamConfigurator(Func<TsMediaStreamSource> mediaStreamSourceFactory)
        {
            _mediaStreamSourceFactory = mediaStreamSourceFactory;
        }

        public MediaStreamDescription AudioStreamDescription
        {
            set { _audioStreamDescription = value; }
            get { return _audioStreamDescription; }
        }

        public MediaStreamDescription VideoStreamDescription
        {
            set { _videoStreamDescription = value; }
            get { return _videoStreamDescription; }
        }

        IStreamSource AudioStreamSource
        {
            set { _audioStreamSource = value; }
            get { return _audioStreamSource; }
        }

        IStreamSource VideoStreamSource
        {
            set { _videoStreamSource = value; }
            get { return _videoStreamSource; }
        }

        public TimeSpan? SeekTarget
        {
            get { return _mediaStreamSource.SeekTarget; }
            set { _mediaStreamSource.SeekTarget = value; }
        }

        public TimeSpan? TotalTime { get; set; }

        public void Dispose()
        {
            AudioStreamSource = null;
            AudioStreamDescription = null;

            VideoStreamSource = null;
            VideoStreamDescription = null;

            CleanupMediaStreamSource();
        }

        public IMediaManager MediaManager
        {
            get { return _mediaManager; }
            set
            {
                if (ReferenceEquals(_mediaManager, value))
                    return;

                if (null != value)
                {
                    _playingCompletionSource = new TaskCompletionSource<object>();

                    ResetOpenCompletionSource();
                }

                _mediaManager = value;

                if (null == value)
                    CleanupMediaStreamSource();
            }
        }

        public void Initialize()
        { }

        public void CheckForSamples()
        {
            var mm = _mediaStreamSource;

            if (null != mm)
                mm.CheckForSamples();
        }

        public void ValidateEvent(MediaStreamFsm.MediaEvent mediaEvent)
        {
            var mm = _mediaStreamSource;

            if (null != mm)
                mm.ValidateEvent(mediaEvent);
        }

        async Task<IMediaStreamConfiguration> IMediaStreamControl.OpenAsync(CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.IMediaStreamControl.OpenAsync()");

            if (null == _mediaStreamSource)
                throw new InvalidOperationException("MediaStreamSource has not been created");

            var mediaManager = MediaManager;

            if (null == mediaManager)
                throw new InvalidOperationException("MediaManager has not been initialized");

            var openCompletionSource = _openCompletionSource;

            Action cancellationAction = () =>
            {
                var task = mediaManager.CloseMediaAsync();

                TaskCollector.Default.Add(task, "MediaSteamConfigurator.OpenAsync mediaManager.CloseMediaAsync");

                openCompletionSource.TrySetCanceled();
            };

            using (cancellationToken.Register(cancellationAction))
            {
                var timeoutTask = TaskExt.Delay((int)OpenTimeout.TotalMilliseconds, cancellationToken);

                await TaskExt.WhenAny(_openCompletionSource.Task, timeoutTask).ConfigureAwait(false);
            }

            if (!_openCompletionSource.Task.IsCompleted)
                cancellationAction();

            return await _openCompletionSource.Task.ConfigureAwait(false);
        }

        Task<TimeSpan> IMediaStreamControl.SeekAsync(TimeSpan position, CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.IMediaStreamControl.SeekAsync() " + position);

            var mediaManager = MediaManager;

            if (null == mediaManager)
                throw new InvalidOperationException("MediaManager has not been initialized");

            return mediaManager.SeekMediaAsync(position);
        }

        Task IMediaStreamControl.CloseAsync(CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.IMediaStreamControl.CloseAsync");

            var mediaManager = MediaManager;

            if (null == mediaManager)
            {
                LoggerInstance.Debug("MediaStreamConfigurator.CloseMediaHandler() null media manager");
                return TplTaskExtensions.CompletedTask;
            }

            _playingCompletionSource.TrySetResult(null);

            return TplTaskExtensions.CompletedTask;
        }

        public Task PlayAsync(IMediaConfiguration configuration, CancellationToken cancellationToken)
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.PlayAsync()");

            if (null != configuration.Audio)
                ConfigureAudioStream(configuration.Audio);

            if (null != configuration.Video)
                ConfigureVideoStream(configuration.Video);

            lock (_streamConfigurationLock)
            {
                CompleteConfigure(configuration.Duration);
            }

            return _playingCompletionSource.Task;
        }

        public Task<TMediaStreamSource> CreateMediaStreamSourceAsync<TMediaStreamSource>(CancellationToken cancellationToken)
            where TMediaStreamSource : class
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.CreateMediaStreamSourceAsync()");

            cancellationToken.ThrowIfCancellationRequested();

            if (null != _mediaStreamSource)
                throw new InvalidOperationException("MediaStreamSource already exists");

            TsMediaStreamSource mediaStreamSource = null;
            try
            {
                mediaStreamSource = _mediaStreamSourceFactory();
            }
            catch (System.Exception)
            {

            }

            var ret = mediaStreamSource as TMediaStreamSource;

            if (null == ret)
            {
                mediaStreamSource.Dispose();

                _openCompletionSource.TrySetCanceled();

                _playingCompletionSource.TrySetResult(null);

                throw new InvalidCastException(string.Format("Cannot convert {0} to {1}", mediaStreamSource.GetType().FullName, typeof(TMediaStreamSource).FullName));
            }
            mediaStreamSource.TotalTime = this.TotalTime;
            _mediaStreamSource = mediaStreamSource;

            return TaskExt.FromResult(ret);
        }

        public async Task CloseAsync()
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.CloseAsync()");

            var mediaStreamSource = _mediaStreamSource;

            var pcs = _playingCompletionSource;

            if (null == mediaStreamSource)
            {
                if (null != _openCompletionSource)
                    _openCompletionSource.TrySetCanceled();

                if (null != pcs)
                    pcs.TrySetResult(null);

                return;
            }

            await mediaStreamSource.CloseAsync().ConfigureAwait(false);

            if (null != pcs)
                await pcs.Task.ConfigureAwait(false);
        }

        public void ReportError(string message)
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.ReportError() " + message);

            var mediaStreamSource = _mediaStreamSource;

            if (null == mediaStreamSource)
                LoggerInstance.Debug("MediaStreamConfigurator.ReportError() null _mediaStreamSource");
            else
                mediaStreamSource.ReportError(message);
        }

        void ResetOpenCompletionSource()
        {
            if (null != _openCompletionSource && !_openCompletionSource.Task.IsCompleted)
                _openCompletionSource.TrySetCanceled();

            var openCompletionSource = new TaskCompletionSource<IMediaStreamConfiguration>();

            _openCompletionSource = openCompletionSource;
        }

        void CleanupMediaStreamSource()
        {
            var mss = _mediaStreamSource;

            if (null != mss)
            {
                _mediaStreamSource = null;

                mss.DisposeSafe();
            }
        }

        void CompleteConfigure(TimeSpan? duration)
        {
            //LoggerInstance.Debug("MediaStreamConfigurator.CompleteConfigure() " + duration);

            try
            {
                var descriptions = new List<MediaStreamDescription>();

                if (null != _videoStreamSource && null != _videoStreamDescription)
                    descriptions.Add(_videoStreamDescription);

                if (null != _audioStreamSource && null != _audioStreamSource)
                    descriptions.Add(_audioStreamDescription);

                var mediaSourceAttributes = new Dictionary<MediaSourceAttributesKeys, string>();

                if (duration.HasValue)
                    mediaSourceAttributes[MediaSourceAttributesKeys.Duration] = duration.Value.Ticks.ToString(CultureInfo.InvariantCulture);
#if SILVERLIGHT
                else
                    mediaSourceAttributes[MediaSourceAttributesKeys.Duration] = string.Empty;
#endif

                var canSeek = duration.HasValue;

                mediaSourceAttributes[MediaSourceAttributesKeys.CanSeek] = canSeek.ToString();

                var configuration = new MediaStreamConfiguration
                {
                    VideoStreamSource = _videoStreamSource,
                    AudioStreamSource = _audioStreamSource,
                    Descriptions = descriptions,
                    Attributes = mediaSourceAttributes,
                    Duration = duration
                };

                _openCompletionSource.TrySetResult(configuration);
            }
            catch (OperationCanceledException)
            {
                _openCompletionSource.TrySetCanceled();
            }
            catch (Exception ex)
            {
                LoggerInstance.Debug("MediaStreamConfigurator.CompleteConfigure() failed: " + ex.Message);

                _openCompletionSource.TrySetException(ex);
            }
        }

        void ConfigureVideoStream(IMediaParserMediaStream video)
        {
            var configurationSource = (IVideoConfigurationSource)video.ConfigurationSource;

            var msa = new Dictionary<MediaStreamAttributeKeys, string>();

            msa[MediaStreamAttributeKeys.VideoFourCC] = configurationSource.VideoFourCc;

            var cpd = configurationSource.CodecPrivateData;

            LoggerInstance.Debug("MediaStreamConfigurator.ConfigureVideoStream(): CodecPrivateData: " + cpd);

            if (!string.IsNullOrWhiteSpace(cpd))
                msa[MediaStreamAttributeKeys.CodecPrivateData] = cpd;

            msa[MediaStreamAttributeKeys.Height] = configurationSource.Height.ToString();
            msa[MediaStreamAttributeKeys.Width] = configurationSource.Width.ToString();

            var videoStreamDescription = new MediaStreamDescription(MediaStreamType.Video, msa);

            lock (_streamConfigurationLock)
            {
                _videoStreamSource = video.StreamSource;
                _videoStreamDescription = videoStreamDescription;
            }
        }

        void ConfigureAudioStream(IMediaParserMediaStream audio)
        {
            var configurationSource = (IAudioConfigurationSource)audio.ConfigurationSource;

            var msa = new Dictionary<MediaStreamAttributeKeys, string>();

            var cpd = configurationSource.CodecPrivateData;

            LoggerInstance.Debug("TsMediaStreamSource.ConfigureAudioStream(): CodecPrivateData: " + cpd);

            if (!string.IsNullOrWhiteSpace(cpd))
                msa[MediaStreamAttributeKeys.CodecPrivateData] = cpd;

            var audioStreamDescription = new MediaStreamDescription(MediaStreamType.Audio, msa);

            lock (_streamConfigurationLock)
            {
                _audioStreamSource = audio.StreamSource;
                _audioStreamDescription = audioStreamDescription;
            }
        }
    }
}
