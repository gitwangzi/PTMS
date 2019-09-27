using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Controls
{
    public class MediaInfo
    {
        /// <summary>
        /// 是否显示地图
        /// </summary>
        public bool ShowHistoryLine { get; set; }

        /// <summary>
        /// 控制窗口的模式
        /// </summary>
        public bool IsHideProgressControl { get; set; }

        public Orientation Orientation { get; set; }

        public string VehicleId { get; set; }

        /// <summary>
        /// 是否自动播放
        /// </summary>
        public bool AutoPlay { get; set; }

        public List<MediaInfoItem> MediaInfoItems { get; set; }

        public MediaInfo()
        {
            MediaInfoItems = new List<MediaInfoItem>();
            ShowHistoryLine = false;
        }

        public class MediaInfoItem : INotifyPropertyChanged
        {
            public int Channel { get; set; }

            public string Url { get; set; }

            public TimeSpan? MediaPeriod
            {
                get
                {
                    if (StartTime.HasValue && EndTime.HasValue)
                    {
                        return StartTime.Value.Subtract(EndTime.Value);
                    }

                    return null;
                }
            }

            public DateTime? StartTime { get; set; }

            public DateTime? EndTime { get; set; }

            private double _indent;
            public double Indent
            {
                get
                {
                    return _indent;
                }
                set
                {
                    _indent = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Indent"));
                    }
                }
            }

            private double _width;
            public double Width
            {
                get
                {
                    return _width;
                }
                set
                {
                    _width = value;
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Width"));
                    }
                }
            }

            /// <summary>
            /// 是否显示播放暂停按钮
            /// </summary>
            public bool IsShowControlBar { get; set; }

            /// <summary>
            /// 是否显示进度条
            /// 实时视频自动隐藏进度条
            /// </summary>
            public bool IsShowProcessBar { get; set; }

            /// <summary>
            /// 是否显示移除按钮
            /// </summary>
            public bool ShowRemoveBtn { get; set; }

            private bool _isRealVideo;
            /// <summary>
            /// 控制每个播放器的模式,
            /// </summary>
            public bool IsRealVideo
            {
                get { return _isRealVideo; }
                set
                {
                    _isRealVideo = value;
                    if (_isRealVideo == true)
                    {
                        IsShowProcessBar = false;
                    }
                }
            }

            #region INotifyPropertyChanged 成员

            public event PropertyChangedEventHandler PropertyChanged;

            #endregion
        }
    }

}
