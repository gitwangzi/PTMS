using Gsafety.PTMS.Media;
using Gsafety.PTMS.Media.Common.Loggers;
using Gsafety.PTMS.Media.MediaManager;
using Gsafety.PTMS.Media.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gsafety.PTMS.Media.Player
{
    public partial class MediaPlayer : UserControl
    {
        #region 字段

        DispatcherTimer _progressTimer;

        IMediaStreamFacade _mediaStreamFacade;

        private bool _removeBtnVisibility = false;
        #endregion



        #region 属性
        private bool _realVideoMode;
        /// <summary>
        /// 模式，历史视频/实时视频
        /// </summary>
        public bool RealVideoMode
        {
            get { return _realVideoMode; }
            set
            {
                _realVideoMode = value;

                if (value)
                {
                    //实时
                    AutoPlay = true;
                    SetProcessBarVisisbility(Visibility.Collapsed);
                    SetControlBarVisibility(Visibility.Visible);
                }
                else
                {
                    AutoPlay = false;
                    SetProcessBarVisisbility(Visibility.Visible);
                    SetControlBarVisibility(Visibility.Visible);
                }
            }
        }

        /// <summary>
        /// 是否自动播放
        /// </summary>
        public bool AutoPlay
        {
            get
            {
                return mediaElement1.AutoPlay;
            }
            set
            {
                mediaElement1.AutoPlay = value;
            }
        }

        /// <summary>
        /// 播放路径 绝对路径
        /// </summary>
        public string Url { get; set; }

        private TimeSpan? _totalTime;
        /// <summary>
        /// 历史视频总时长
        /// </summary>
        public TimeSpan? TotalTime
        {
            get
            {
                return _totalTime;
            }
            set
            {
                _totalTime = value;
                if (_totalTime.HasValue)
                {
                    txtTotoalTime.Text = _totalTime.Value.ToString(@"hh\:mm\:ss");
                    sliderProcess.Maximum = _totalTime.Value.TotalSeconds;
                }
                else
                {
                    txtTotoalTime.Text = "00:00:00";
                }
            }
        }

        public MediaElementState CurrentState
        {
            get
            {
                if (mediaElement1 != null)
                {
                    return mediaElement1.CurrentState;
                }
                else
                {
                    return MediaElementState.Closed;
                }
            }
        }

        public Action<MediaPlayer> OnClickRemoveButtonHandle;

        public MouseButtonEventHandler DoubleClickHandle;
        #endregion

        #region 公共方法

        /// <summary>
        /// 设置播放器背景
        /// </summary>
        /// <param name="uri"></param>
        public void SetBackImage(Uri uri)
        {
            var imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(uri);
            ContentPanel.Background = imageBrush;
        }

        /// <summary>
        /// 拖拽 定位
        /// </summary>
        /// <param name="startTime"></param>
        /// <returns></returns>
        public async Task SeekTarget(TimeSpan startTime)
        {
            await PlayFromTarget(startTime);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            CloseMedia();
        }

        /// <summary>
        /// 连接视频
        /// </summary>
        /// <returns></returns>
        public async Task Connect()
        {
            await ConnectMedia();
            System.Diagnostics.Debug.WriteLine("ConnectMedia End");
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            play_Click(null, null);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            PauseMedia();
        }

        /// <summary>
        /// 设置进度条是否可见
        /// </summary>
        /// <param name="visibility"></param>
        public void SetProcessBarVisisbility(Visibility visibility)
        {
            progressAndTimeGrid.Visibility = visibility;
        }

        /// <summary>
        /// 设置控制按钮的显示和隐藏
        /// </summary>
        /// <param name="visibility"></param>
        public void SetControlBarVisibility(Visibility visibility)
        {
            controlGrid.Visibility = visibility;
        }

        /// <summary>
        /// 移除按钮是否可见
        /// </summary>
        /// <param name="visibility"></param>
        public void SetRemoveBtnVisibility(Visibility visibility)
        {
            _removeBtnVisibility = visibility == Visibility.Visible;
        }
        #endregion

        public event EventHandler<ProcessUpdateEventArgs> ProcessUpdated;

        private TimeSpan? _seekTarget;

        TimeSpan _previousPosition;

        public MediaPlayer()
        {
            InitializeComponent();

            SetControlBtnVisibility(playButton);

            _progressTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };

            _progressTimer.Tick += OnPositionSamplerOnTick;

            this.Loaded += MediaPlayer_Loaded;
        }

        void mediaElement1_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            var state = null == mediaElement1 ? MediaElementState.Closed : mediaElement1.CurrentState;

            if (null != _mediaStreamFacade)
            {
                var managerState = _mediaStreamFacade.State;

                if (MediaElementState.Closed == state)
                {
                    if (MediaManagerState.OpenMedia == managerState || MediaManagerState.Opening == managerState || MediaManagerState.Playing == managerState)
                        state = MediaElementState.Opening;
                    if (MediaManagerState.Closed == managerState || MediaManagerState.Closing == managerState || MediaManagerState.Error == managerState)
                        state = MediaElementState.Closed;
                }
            }

            UpdateState(state);
        }

        void UpdateState(MediaElementState state)
        {
            LoggerInstance.Debug("MediaElement State: " + state);

            messageBox.Text = "";
            //if (MediaElementState.Buffering == state && null != mediaElement1)
            //    messageBox.Text = string.Format("Buffering {0:F2}%", mediaElement1.BufferingProgress * 100);
            //else
            //    messageBox.Text = state.ToString();

            if (MediaElementState.Closed == state)
            {
                SetControlBtnVisibility(playButton);
            }
            else if (MediaElementState.Paused == state || MediaElementState.Stopped == state)
            {
                SetControlBtnVisibility(playButton);
            }
            else
            {
                if (RealVideoMode)
                {
                    SetControlBtnVisibility(stopButton);
                }
                else
                {
                    SetControlBtnVisibility(pauseButton);
                }
            }

            OnPositionSamplerOnTick(null, null);
        }

        void OnPositionSamplerOnTick(object o, EventArgs ea)
        {
            if (null == mediaElement1 || (MediaElementState.Playing != mediaElement1.CurrentState && MediaElementState.Paused != mediaElement1.CurrentState && MediaElementState.Stopped != mediaElement1.CurrentState))
            {
                PositionBox.Text = "00:00:00";
                return;
            }
            var positionSample = mediaElement1.Position;

            if (positionSample == _previousPosition)
                return;

            _previousPosition = positionSample;

            var newValue = positionSample.TotalSeconds + (_seekTarget.HasValue == false ? 0 : _seekTarget.Value.TotalSeconds);
            if (TotalTime.HasValue && _isDragProcessSlider == false)
            {
                sliderProcess.Value = newValue;
            }

            var currentTime = TimeSpan.FromSeconds(newValue);
            PositionBox.Text = FormatTimeSpan(currentTime);
            if (ProcessUpdated != null)
            {
                ProcessUpdated(this, new ProcessUpdateEventArgs(currentTime));
            }
        }

        string FormatTimeSpan(TimeSpan timeSpan)
        {
            var sb = new StringBuilder();

            if (timeSpan < TimeSpan.Zero)
            {
                sb.Append('-');

                timeSpan = -timeSpan;
            }

            if (timeSpan.Days > 1)
                sb.AppendFormat(timeSpan.ToString(@"%d\."));

            sb.Append(timeSpan.ToString(@"hh\:mm\:ss"));

            return sb.ToString();
        }

        async void play_Click(object sender, RoutedEventArgs e)
        {
            //messageBox.Visibility = Visibility.Collapsed;

            if (mediaElement1.NaturalDuration.TimeSpan == mediaElement1.Position)
            {
                mediaElement1.Position = TimeSpan.FromSeconds(0);
            }

            if (MediaElementState.Paused == mediaElement1.CurrentState || MediaElementState.Stopped == mediaElement1.CurrentState)
            {

                mediaElement1.Play();
                LoggerInstance.Debug("Start Position:" + mediaElement1.Position);
                _progressTimer.Start();
                return;
            }
            else
            {
                await ConnectMedia();
            }
        }

        private async Task ConnectMedia()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            InitializeMediaStream();
            var source = new Uri(Url);

            try
            {
                _mediaStreamFacade.SeekTarget = _seekTarget;
                _mediaSource = await _mediaStreamFacade.CreateMediaStreamSourceAsync(source, CancellationToken.None).ConfigureAwait(false) as TsMediaStreamSource;
                if (null == _mediaSource)
                {
                    LoggerInstance.Error("MainPage Play unable to create media stream source");
                    return;
                }

                this.Dispatcher.BeginInvoke(() =>
                {
                    mediaElement1.SetSource(_mediaSource);

                    this.TotalTime = _mediaSource.TotalTime;
                    if (AutoPlay && this.TotalTime.HasValue)
                    {
                        _progressTimer.Start();
                    }
                });
            }
            catch (FileNotFoundException)
            {
                this.Dispatcher.BeginInvoke(() =>
                {
                    messageBox.Visibility = Visibility.Visible;
                    messageBox.Text = "Video Stream Error";
                    SetControlBtnVisibility(playButton);
                });
            }
            catch (Exception ex)
            {
                messageBox.Text = ex.ToString();
                LoggerInstance.Exception(ex);
            }

            stopwatch.Stop();
        }

        void InitializeMediaStream()
        {
            if (null != _mediaStreamFacade)
                return;

            MediaStreamFacadeSettings.Parameters.UseRtspStreamMediaManager = true;
            MediaStreamFacadeSettings.Parameters.UseHttpConnection = true;
            MediaStreamFacadeSettings.Parameters.IsLogIncomingRtcpPacket = false;
            MediaStreamFacadeSettings.Parameters.IsLogIncomingRtpPacket = false;
            MediaStreamFacadeSettings.Parameters.IsLogOutgoingRtcpPacket = false;
            MediaStreamFacadeSettings.Parameters.IsLogRtpDataWith0x = false;

            _mediaStreamFacade = MediaStreamFacadeSettings.Parameters.Create();
            _mediaStreamFacade.SetParameter(new ApplicationInformation("PTMS Siverlight", "1.0.0.0"));

            _mediaStreamFacade.StateChange += TsMediaManagerOnStateChange;
        }

        void PauseMedia()
        {
            _progressTimer.Stop();

            if (null != mediaElement1)
            {
                mediaElement1.Pause();
            }
        }

        void StopMedia()
        {
            _progressTimer.Stop();

            if (null != mediaElement1)
            {
                mediaElement1.Stop();
               
            }
        }

        async Task CloseMedia()
        {
            StopMedia();

            if (null != mediaElement1)
                mediaElement1.Source = null;

            if (null == _mediaStreamFacade)
                return;
            GC.Collect();
            var mediaStreamFacade = _mediaStreamFacade;

            await mediaStreamFacade.CloseAsync().ConfigureAwait(false);
            mediaStreamFacade.StateChange -= TsMediaManagerOnStateChange;
            _mediaStreamFacade = null;

            System.Diagnostics.Debug.WriteLine("mediaStreamFacade.CloseAsync().ConfigureAwait End");

            mediaStreamFacade.DisposeBackground("MainPage CloseMedia");
            System.Diagnostics.Debug.WriteLine("mediaStreamFacade.DisposeBackground End");
        }

        void TsMediaManagerOnStateChange(object sender, MediaManagerStateEventArgs tsMediaManagerStateEventArgs)
        {
            Dispatcher.BeginInvoke(() =>
            {
                mediaElement1_CurrentStateChanged(null, null);
            });
        }

        void mediaElement1_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            CloseMedia();

            playButton.IsEnabled = true;
        }

        void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            LoggerInstance.Debug("MainPage MediaEnded");

            sliderProcess.Value = 0;

            CloseMedia();
        }

        void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            PauseMedia();
        }

        void mediaElement1_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            mediaElement1_CurrentStateChanged(sender, e);
        }

        private async void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            SetControlBtnVisibility(playButton);
            await CloseMedia();
        }

        private void SetControlBtnVisibility(Button setToVisibilityBtn)
        {
            playButton.Visibility = Visibility.Collapsed;
            pauseButton.Visibility = Visibility.Collapsed;
            stopButton.Visibility = Visibility.Collapsed;
            setToVisibilityBtn.Visibility = Visibility.Visible;
        }

        private async void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            await CloseMedia();

            if (OnClickRemoveButtonHandle != null)
            {
                OnClickRemoveButtonHandle(this);
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
            {
                return;
            }

            if (DoubleClickHandle != null)
            {
                DoubleClickHandle(sender, e);
            }
        }
        bool isOpend = false;
        private void mediaElement1_MediaOpened(object sender, RoutedEventArgs e)
        {
            isOpend = true;
        }
    }
}
