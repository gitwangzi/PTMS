using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Media.Player;
using Jounce.Core.Event;
using System.ComponentModel.Composition;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Threading.Tasks;
using Gsafety.PTMS.Share;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.Common.Converts;
using System.Threading;

namespace Gsafety.Common.Controls
{
    public partial class MediaPlayerContainer : UserControl
    {
        #region 字段
        private bool _needSetPlayBtnDisable = true;

        private CameraInstallLocationConverter _converter = new CameraInstallLocationConverter();
        private bool _isPlayingBeforeDrage = false;
        private SolidColorBrush _borderSolidColorBrush = new SolidColorBrush(Color.FromArgb((byte)255, (byte)28, (byte)31, (byte)35));
        private Thumb _sliderProcessThumb;

        private List<PlayerInfo> PlayerInfoList = new List<PlayerInfo>();
        //private List<WeakReference> _connectReadyMediaElement = new List<WeakReference>();
        DispatcherTimer _positionSamplerTimer;

        private int _maxRowCount;
        private int _maxColumnCount;
        private double _mediaPlayerDesighWidth = 354;
        private double _mediaPlayerDesighHeight = 256;
        private bool _autoSize = true;

        private MediaInfo _mediaInfo = new MediaInfo();

        #endregion

        #region 属性

        private bool _realVideoMode;
        public bool IsHideProgressControl
        {
            get { return _realVideoMode; }
            set
            {
                _realVideoMode = value;
            }
        }

        private DateTime? _minTime;
        public DateTime? MinTime
        {
            get { return _minTime; }
        }

        private DateTime? _maxTime;
        public DateTime? MaxTime
        {
            get { return _maxTime; }
        }

        private Orientation _orientation = Orientation.Horizontal;
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                _orientation = value;
            }
        }

        public bool AutoPlay { get; set; }

        public Action<DateTime> OnProcessChanged;

        #endregion

        public MediaPlayerContainer(MediaInfo info, bool autoSize = true)
            : this(GetRowCount(info), GetColumnCount(info), autoSize)
        {
            _mediaInfo = info;
            IsHideProgressControl = info.IsHideProgressControl;
            Orientation = info.Orientation;
            AutoPlay = info.AutoPlay;

            _positionSamplerTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            _positionSamplerTimer.Tick += OnPositionSamplerOnTick;

            _minTime = info.MediaInfoItems.Min(t => t.StartTime);
            if (_minTime.HasValue)
            {
                txtStartTime.Text = _minTime.Value.ToString(@"HH\:mm\:ss");
            }

            _maxTime = info.MediaInfoItems.Max(t => t.EndTime);
            if (_maxTime.HasValue)
            {
                txtEndTime.Text = _maxTime.Value.ToString(@"HH\:mm\:ss");
            }

            if (_minTime.HasValue && _maxTime.HasValue)
            {
                sliderProcess.Maximum = _maxTime.Value.Subtract(_minTime.Value).TotalSeconds;
            }

            foreach (var item in info.MediaInfoItems)
            {
                Play(item);
            }
        }

        public MediaPlayerContainer(int maxRowCount = 2, int maxColumnCount = 2, bool autoSize = true)
        {
            InitComponent(maxRowCount, maxColumnCount);

            _autoSize = autoSize;
        }

        private void InitComponent(int maxRowCount = 2, int maxColumnCount = 2)
        {
            InitializeComponent();

            _maxRowCount = maxRowCount;
            _maxColumnCount = maxColumnCount;

            for (int rowIndex = 0; rowIndex < maxRowCount; rowIndex++)
            {
                var rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                rootGrid.RowDefinitions.Add(rowDef);
            }

            for (int columnIndex = 0; columnIndex < maxColumnCount; columnIndex++)
            {
                var columnDef = new ColumnDefinition();
                columnDef.Width = new GridLength(1, GridUnitType.Star);
                rootGrid.ColumnDefinitions.Add(columnDef);
            }

            for (int rowIndex = 0; rowIndex < maxRowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < maxColumnCount; columnIndex++)
                {
                    var border = new Border();
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = _borderSolidColorBrush;
                    Grid.SetRow(border, rowIndex);
                    Grid.SetColumn(border, columnIndex);
                    rootGrid.Children.Add(border);
                }
            }

            this.sliderProcess.Loaded += MediaPlayerContainer_Loaded;
        }

        private static int GetRowCount(MediaInfo info)
        {
            if (info.Orientation == Orientation.Horizontal)
            {
                return info.MediaInfoItems.Count <= 2 ? 1 : 2;
            }
            else
            {
                return info.MediaInfoItems.Count <= 1 ? 1 : 2;
            }
        }

        private static int GetColumnCount(MediaInfo info)
        {
            if (info.Orientation == Orientation.Horizontal)
            {
                return info.MediaInfoItems.Count <= 1 ? 1 : 2;
            }
            else
            {
                return info.MediaInfoItems.Count <= 2 ? 1 : 2;
            }
        }

        void MediaPlayerContainer_Loaded(object sender, RoutedEventArgs e)
        {
            InitProcessSliderThumbEvent();

            if (_minTime.HasValue && _maxTime.HasValue)
            {
                var percent = sliderProcess.Maximum / sliderProcess.ActualWidth;

                foreach (var item in _mediaInfo.MediaInfoItems)
                {
                    item.Indent = item.StartTime.Value.Subtract(_minTime.Value).TotalSeconds / percent;
                    item.Width = item.EndTime.Value.Subtract(item.StartTime.Value).TotalSeconds / percent;
                }
            }

            if (IsHideProgressControl == false)
            {
                if (_mediaInfo.MediaInfoItems.Count > 1)
                {
                    processItemsControl.ItemsSource = _mediaInfo.MediaInfoItems;
                    TrackRectangle.Visibility = Visibility;
                    TrackRectangle.Margin = new Thickness(txtStartTime.ActualWidth +
                        txtStartTime.Margin.Left + txtStartTime.Margin.Right
                        + _sliderProcessThumb.ActualWidth / 2 - TrackRectangle.Width / 2 + (sliderProcess.ActualWidth - 10) / sliderProcess.Maximum * sliderProcess.Value,
                        TrackRectangle.Margin.Top, TrackRectangle.Margin.Right, TrackRectangle.Margin.Bottom);
                }
            }
            else
            {
                controlGrid.Visibility = Visibility.Collapsed;
            }
        }

        public void OnClosed()
        {
            try
            {
                foreach (var item in PlayerInfoList)
                {
                    item.Dispose();
                }
            }
            catch (System.Exception ex)
            {
            }

            try
            {
                PlayerInfoList.Clear();

                var grid = this.Parent as Grid;
                if (grid != null)
                {
                    grid.Children.Remove(this);
                }
            }
            catch (System.Exception ex)
            {
            }
            try
            {
                if (_positionSamplerTimer != null)
                {
                    _positionSamplerTimer.Stop();
                    _positionSamplerTimer = null;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        public static string GenerateRealVideoUrl(MediaInfo.MediaInfoItem itemInfo)
        {
             var paras = itemInfo.Url.Split(':');

            if (ApplicationContext.Instance.ServerConfig.DisplayParameter.DisplayParameterMode == true)
            {
                var displayParameter = ApplicationContext.Instance.ServerConfig.DisplayParameter;
                if (displayParameter.VideoFileDic.ContainsKey(paras[1]))
                {
                    var files = displayParameter.VideoFileDic[paras[1]];
                    var channel = int.Parse(paras[0]);
                    if (files.Count > channel)
                    {
                        var file = files[channel];
                        return string.Format("rtsp://{0}:{1}/local/{2}", ApplicationContext.Instance.ServerConfig.RTSPServiceIP, ApplicationContext.Instance.ServerConfig.RTSPServicePort, file);
                    }
                }
            }

            var url = string.Format("rtsp://{0}:{1}/live/{2}:{3}:{4}", ApplicationContext.Instance.ServerConfig.RTSPServiceIP, ApplicationContext.Instance.ServerConfig.RTSPServicePort, paras[0], ApplicationContext.Instance.ServerConfig.RTSPStreamChannel, paras[1]);
            return url;
        }

        public void Play(MediaInfo.MediaInfoItem itemInfo)
        {
            try
            {
                string url;
                if (itemInfo.IsRealVideo == false)
                {
                    url = string.Format("rtsp://{0}:{1}/local/{2}", ApplicationContext.Instance.ServerConfig.RTSPServiceIP, ApplicationContext.Instance.ServerConfig.RTSPServicePort, itemInfo.Url);
                }
                else
                {
                    url = GenerateRealVideoUrl(itemInfo);
                }

                if (IsHideProgressControl)
                {
                    var exist = PlayerInfoList.Any(t => string.Equals(url, t.Url));
                    if (exist)
                    {
                        return;
                    }
                }

                var mediaPlayer = new MediaPlayer();
                if (_autoSize == false)
                {
                    mediaPlayer.Width = _mediaPlayerDesighWidth;
                    mediaPlayer.Height = _mediaPlayerDesighHeight;
                }
                var playerInfo = FindUseablePlayerInfo();

                if (playerInfo.Player != null)
                {
                    RemovePlayer(playerInfo);
                }

                //rootGrid.Children.Add(mediaPlayer);

                var border = new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = _borderSolidColorBrush;
                rootGrid.Children.Add(border);
                border.Child = mediaPlayer;

                Grid.SetRow(border, playerInfo.RowIndex);
                Grid.SetColumn(border, playerInfo.ColumnIndex);

                playerInfo.Url = url;
                playerInfo.Player = mediaPlayer;
                playerInfo.MediaInfoItem = itemInfo;

                mediaPlayer.Url = url;
                mediaPlayer.RealVideoMode = itemInfo.IsRealVideo;

                mediaPlayer.AutoPlay = AutoPlay;
                mediaPlayer.SetRemoveBtnVisibility(itemInfo.ShowRemoveBtn ? Visibility.Visible : Visibility.Collapsed);
                mediaPlayer.SetControlBarVisibility(itemInfo.IsShowControlBar ? Visibility.Visible : Visibility.Collapsed);
                mediaPlayer.SetProcessBarVisisbility(itemInfo.IsShowProcessBar ? Visibility.Visible : Visibility.Collapsed);

                if (itemInfo.ShowRemoveBtn)
                {
                    mediaPlayer.OnClickRemoveButtonHandle += mediaPlayerOnClickRemoveButtonHandle;
                }

                mediaPlayer.DoubleClickHandle += FullScreenHandle;

                mediaPlayer.InfoMessage = _converter.Convert((CameraInstallLocationEnum)itemInfo.Channel, typeof(string), null, null) as string;
                mediaPlayer.Connect();

                if (IsHideProgressControl == false)
                {
                    this.playButton.IsEnabled = false;
                    SetPlayButtonEnable();

                    mediaPlayer.MediaElement.CurrentStateChanged += MediaElement_CurrentStateChanged;
                }

                mediaPlayer.ProcessUpdated += mediaPlayer_ProcessUpdated;
            }
            catch (System.Exception ex)
            {
            }
        }

        void MediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            var element = sender as MediaElement;

            var currentTime = _minTime.Value.AddSeconds(sliderProcess.Value);
            var needReadyList = PlayerInfoList.Where(t => t.MediaInfoItem.StartTime <= currentTime && currentTime <= t.MediaInfoItem.EndTime).ToList();
            if (needReadyList.Any(t => t.Player.MediaElement == element) == false)
            {
                return;
            }

            var readyCount = needReadyList.Count(t => t.Player.MediaElement.CurrentState == MediaElementState.Paused || t.Player.MediaElement.CurrentState == MediaElementState.Playing);
            if (readyCount == needReadyList.Count)
            {
                playButton.IsEnabled = true;
            }
            else
            {
                if (_needSetPlayBtnDisable)
                {
                    playButton.IsEnabled = false;
                }
            }
        }

        private void RemovePlayer(PlayerInfo playerInfo)
        {
            if (playerInfo.Player.MediaElement != null)
            {
                playerInfo.Player.MediaElement.CurrentStateChanged -= MediaElement_CurrentStateChanged;
            }

            playerInfo.Player.DoubleClickHandle -= FullScreenHandle;
            rootGrid.Children.Remove(playerInfo.Player.Parent as Border);
            playerInfo.Dispose();
        }

        private void mediaPlayerOnClickRemoveButtonHandle(MediaPlayer obj)
        {
            var playerInfo = PlayerInfoList.FirstOrDefault(t => t.Player == obj);
            if (playerInfo != null)
            {
                PlayerInfoList.Remove(playerInfo);
                rootGrid.Children.Remove(playerInfo.Player.Parent as Border);
            }
        }

        public void SeekTarget(DateTime targetTime)
        {
            if (targetTime > _maxTime || targetTime < _minTime)
            {
                return;
            }

            sliderProcessThumb_DragStarted(null, null);
            sliderProcess.Value = (double)((targetTime - _minTime.Value).TotalSeconds);
            sliderProcessThumb_DragCompleted(null, null);
        }

        void mediaPlayer_ProcessUpdated(object sender, ProcessUpdateEventArgs e)
        {
            //PositionBox.Text = e.CurrentProcess.ToString(@"hh\:mm\:ss");
        }

        private PlayerInfo FindUseablePlayerInfo()
        {
            lock (this)
            {
                var maxCount = 4 * 4;
                if (PlayerInfoList.Count < maxCount)
                {
                    int rowIndex = 0, columnIndex = 0;

                    bool find = false;
                    if (Orientation == Orientation.Horizontal)
                    {
                        for (int y = 0; y < _maxRowCount; y++)
                        {
                            if (find)
                            {
                                break;
                            }

                            for (int x = 0; x < _maxColumnCount; x++)
                            {
                                if (find)
                                {
                                    break;
                                }

                                if (PlayerInfoList.Any(t => t.RowIndex == y && t.ColumnIndex == x) == false)
                                {
                                    rowIndex = y;
                                    columnIndex = x;
                                    find = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int x = 0; x < _maxColumnCount; x++)
                        {
                            if (find)
                            {
                                break;
                            }

                            for (int y = 0; y < _maxRowCount; y++)
                            {
                                if (find)
                                {
                                    break;
                                }

                                if (PlayerInfoList.Any(t => t.RowIndex == y && t.ColumnIndex == x) == false)
                                {
                                    rowIndex = y;
                                    columnIndex = x;
                                    find = true;
                                }
                            }
                        }
                    }

                    var info = new PlayerInfo()
                    {
                        AddTime = DateTime.Now,
                        RowIndex = rowIndex,
                        ColumnIndex = columnIndex,
                    };

                    PlayerInfoList.Add(info);
                    return info;
                }
                else
                {
                    var info = PlayerInfoList.OrderBy(t => t.AddTime).FirstOrDefault();
                    info.AddTime = DateTime.Now;
                    return info;
                }
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (playButton.IsChecked == true)
            {
                _positionSamplerTimer.Start();
            }
            else
            {
                _positionSamplerTimer.Stop();

                foreach (var item in PlayerInfoList)
                {
                    item.Player.Pause();
                }
            }
        }

        #region 拖拽进度条
        private void InitProcessSliderThumbEvent()
        {
            if (_sliderProcessThumb != null)
            {
                return;
            }

            _sliderProcessThumb = FindTemplateChild(sliderProcess, "HorizontalThumb") as Thumb;
            if (_sliderProcessThumb == null)
            {
                return;
            }

            _sliderProcessThumb.DragStarted += sliderProcessThumb_DragStarted;
            _sliderProcessThumb.DragCompleted += sliderProcessThumb_DragCompleted;
        }

        async void sliderProcessThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            var currentTime = _minTime.Value.AddSeconds(sliderProcess.Value);
            var taskList = new List<Task>();
            foreach (var item in PlayerInfoList)
            {
                if (item.MediaInfoItem.StartTime <= currentTime && currentTime <= item.MediaInfoItem.EndTime)
                {
                    taskList.Add(item.Player.SeekTarget(currentTime.Subtract(item.MediaInfoItem.StartTime.Value)));
                    //播
                }
                else
                {
                    //暂停
                    if (item.Player.CurrentState == MediaElementState.Playing)
                    {
                        item.Player.Pause();
                    }
                }
            }

            foreach (var item in taskList)
            {
                await item;
            }

            if (_isPlayingBeforeDrage)
            {
                _positionSamplerTimer.Start();
                playButton.IsChecked = true;
            }

            playButton.IsEnabled = false;
            SetPlayButtonEnable();
        }

        void sliderProcessThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            _isPlayingBeforeDrage = playButton.IsChecked == true;

            _positionSamplerTimer.Stop();
            playButton.IsChecked = false;

            foreach (var item in PlayerInfoList)
            {
                item.Player.Pause();
            }
        }

        private FrameworkElement FindTemplateChild(DependencyObject obj, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i) as FrameworkElement;

                if (child.Name == name)
                {
                    return child;
                }

                child = FindTemplateChild(child, name);
                if (child != null)
                {
                    return child;
                }
            }

            return null;
        }
        #endregion

        void OnPositionSamplerOnTick(object o, EventArgs ea)
        {
            var currentTime = _minTime.Value.AddSeconds(sliderProcess.Value + 1);
            var needStartList = PlayerInfoList.Where(t => t.MediaInfoItem.StartTime <= currentTime && t.MediaInfoItem.EndTime >= currentTime).ToList();
            if (needStartList.Count > 0 && needStartList.Any(t => t.Player.MediaElement.CurrentState == MediaElementState.Playing || t.Player.MediaElement.CurrentState == MediaElementState.Paused) == false)
            {
                return;
            }

            sliderProcess.Value += 1;
            if (sliderProcess.Value == sliderProcess.Maximum)
            {
                _positionSamplerTimer.Stop();
                playButton.IsChecked = false;
            }

            var needStartMedia = PlayerInfoList.Where(t => t.MediaInfoItem.StartTime <= currentTime && t.MediaInfoItem.EndTime >= currentTime && t.Player.CurrentState != MediaElementState.Closed && t.Player.CurrentState != MediaElementState.Playing).ToList();
            foreach (var item in needStartMedia)
            {
                item.Player.Play();
            }
        }

        class PlayerInfo : IDisposable
        {
            public int RowIndex { get; set; }

            public int ColumnIndex { get; set; }

            public string Url { get; set; }

            public MediaPlayer Player { get; set; }

            public DateTime AddTime { get; set; }

            public MediaInfo.MediaInfoItem MediaInfoItem { get; set; }

            public void Dispose()
            {
                if (Player != null)
                {
                    try
                    {
                        Player.Close();
                        Player = null;
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
        }

        private void sliderProcess_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (OnProcessChanged != null && _minTime.HasValue)
            {
                var time = _minTime.Value.Add(TimeSpan.FromSeconds((int)sliderProcess.Value));
                try
                {
                    OnProcessChanged(time);
                }
                catch (System.Exception ex)
                {
                }
            }

            TrackRectangle.Margin = new Thickness(txtStartTime.ActualWidth + txtStartTime.Margin.Left + txtStartTime.Margin.Right + _sliderProcessThumb.ActualWidth / 2 - TrackRectangle.Width / 2 + (sliderProcess.ActualWidth - 10) / sliderProcess.Maximum * sliderProcess.Value,
                TrackRectangle.Margin.Top, TrackRectangle.Margin.Right, TrackRectangle.Margin.Bottom);
        }

        private void SetPlayButtonEnable()
        {
            _needSetPlayBtnDisable = true;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(5000);
                playButton.Dispatcher.InvokeAsync(() =>
                   {
                       if (playButton.IsEnabled == false)
                       {
                           playButton.IsEnabled = true;
                       }
                   });
                _needSetPlayBtnDisable = false;
            });
        }

        #region 设置窗口数量
        public void SetVideoCount(int rowCount, int columnCount)
        {
            if (_maxRowCount == rowCount && _maxColumnCount == columnCount)
            {
                return;
            }

            //设置和添加列
            if (rowCount > _maxRowCount)
            {
                for (int i = _maxRowCount; i < rootGrid.RowDefinitions.Count && i < rowCount; i++)
                {
                    rootGrid.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);
                }

                var tempCount = rowCount - rootGrid.RowDefinitions.Count;
                for (int i = 0; i < tempCount; i++)
                {
                    var rowDef = new RowDefinition();
                    rowDef.Height = new GridLength(1, GridUnitType.Star);
                    rootGrid.RowDefinitions.Add(rowDef);
                }
            }
            else if (rowCount < _maxRowCount)
            {
                for (int i = rootGrid.RowDefinitions.Count - 1; i > rowCount - 1; i--)
                {
                    rootGrid.RowDefinitions[i].Height = new GridLength(0, GridUnitType.Pixel);
                }
            }

            if (columnCount > _maxColumnCount)
            {
                for (int i = _maxColumnCount; i < rootGrid.ColumnDefinitions.Count && i < columnCount; i++)
                {
                    rootGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
                }

                var tempCount = rootGrid.ColumnDefinitions.Count;
                for (int i = 0; i < columnCount - tempCount; i++)
                {
                    var columnDef = new ColumnDefinition();
                    columnDef.Width = new GridLength(1, GridUnitType.Star);
                    rootGrid.ColumnDefinitions.Add(columnDef);
                }
            }
            else if (columnCount < _maxColumnCount)
            {
                for (int i = rootGrid.ColumnDefinitions.Count - 1; i > columnCount - 1; i--)
                {
                    rootGrid.ColumnDefinitions[i].Width = new GridLength(0, GridUnitType.Pixel);
                }
            }

            _maxRowCount = rowCount;
            _maxColumnCount = columnCount;

            //添加Border
            var borders = rootGrid.Children.Select(t => t as Border).ToList();
            for (int rowIndex = 0; rowIndex < _maxRowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _maxColumnCount; columnIndex++)
                {
                    if (borders.Any(t => Grid.GetColumn(t) == columnIndex && Grid.GetRow(t) == rowIndex))
                    {
                        continue;
                    }
                    else
                    {
                        var border = new Border();
                        border.BorderThickness = new Thickness(1);
                        border.BorderBrush = _borderSolidColorBrush;
                        Grid.SetRow(border, rowIndex);
                        Grid.SetColumn(border, columnIndex);
                        rootGrid.Children.Add(border);
                    }
                }
            }

            //设置控件位置
            var tempOrderList = PlayerInfoList.OrderBy(t => t.RowIndex).ThenBy(t => t.ColumnIndex).ToList();
            PlayerInfoList.Clear();
            var maxCount = _maxColumnCount * _maxRowCount;
            for (int i = 0; i < tempOrderList.Count; i++)
            {

                var info = FindUseablePlayerInfo();
                info.MediaInfoItem = tempOrderList[i].MediaInfoItem;
                info.Player = tempOrderList[i].Player;
                info.Url = tempOrderList[i].Url;
                info.AddTime = tempOrderList[i].AddTime;
                if (maxCount >= i + 1)
                {  
                    Grid.SetColumn(info.Player.Parent as Border, info.ColumnIndex);
                    Grid.SetRow(info.Player.Parent as Border, info.RowIndex);
                }
                //else
                //{
                //    RemovePlayer(tempOrderList[i]);
                //    tempOrderList[i].Player = null;
                //}
            }

        }
        #endregion

        #region 全屏
        private Size _previewSize;
        private int _previewRow, _previewColumn, _previewZIndex;
        private bool _isFullScreen = false;
        private void FullScreenHandle(object sender, MouseButtonEventArgs e)
        {
            var mediaPlayer = sender as MediaPlayer;
            var border = mediaPlayer.Parent as Border;

            if (_isFullScreen == false)
            {
                _previewSize = this.RenderSize;
                _previewRow = Grid.GetRow(border);
                _previewColumn = Grid.GetColumn(border);
                _previewZIndex = Canvas.GetZIndex(border);

                UIElement mainPage;
                if (_autoSize)
                {
                    //视频墙
                    mainPage = this;
                }
                else
                {
                    //弹出框
                    mainPage = Application.Current.RootVisual;
                }

                this.Width = mainPage.RenderSize.Width;
                this.Height = mainPage.RenderSize.Height - 15;

                Grid.SetColumnSpan(border, rootGrid.ColumnDefinitions.Count);
                Grid.SetRowSpan(border, rootGrid.RowDefinitions.Count);
                Grid.SetRow(border, 0);
                Grid.SetColumn(border, 0);
                Canvas.SetZIndex(border, 99);

                if (_autoSize == false)
                {
                    mediaPlayer.Width = double.NaN;
                    mediaPlayer.Height = double.NaN;
                }

                _isFullScreen = true;
            }
            else
            {
                this.Width = _previewSize.Width;
                this.Height = _previewSize.Height;

                Grid.SetColumnSpan(border, 1);
                Grid.SetRowSpan(border, 1);
                Grid.SetRow(border, _previewRow);
                Grid.SetColumn(border, _previewColumn);
                Canvas.SetZIndex(border, _previewZIndex);

                if (_autoSize == false)
                {
                    mediaPlayer.Width = _mediaPlayerDesighWidth;
                    mediaPlayer.Height = _mediaPlayerDesighHeight;
                }

                _isFullScreen = false;
            }

            this.LayoutUpdated += MediaPlayerContainer_LayoutUpdated;
        }

        void MediaPlayerContainer_LayoutUpdated(object sender, EventArgs e)
        {
            MediaPlayerContainer_Loaded(null, null);
            this.LayoutUpdated -= MediaPlayerContainer_LayoutUpdated;
        }
        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

            foreach (var item in PlayerInfoList)
            {
                try
                {
                    RemovePlayer(item);
                }
                catch (System.Exception ex)
                {
                }
            }

            PlayerInfoList.Clear();
        }
    }
}

