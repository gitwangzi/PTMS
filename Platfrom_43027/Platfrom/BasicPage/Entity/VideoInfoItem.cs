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
using System.Windows.Threading;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Share;
using System.Reflection;
using Gs.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Data;
using Gsafety.PTMS.Common.Data.Enum;

namespace Gsafety.PTMS.BasicPage.Entity
{
    public class VideoInfoItem : INotifyPropertyChanged
    {
        public QueryServerFileListMessage Model { get; set; }

        public CameraInstallLocationEnum CameraInstallLocation
        {
            get
            {
                return (CameraInstallLocationEnum)Model.Channel;
            }
            set
            {
                Model.Channel = (decimal)value;
            }
        }

        private DownloadStatus _downloadStatus;
        public DownloadStatus DownloadStatus
        {
            get
            {
                return _downloadStatus;
            }
            set
            {
                if (_downloadStatus != value)
                {
                    _downloadStatus = value;
                    OnPropertyChanged("DownloadStatus");
                    OnPropertyChanged("CheckForPlayVisibility");
                    OnPropertyChanged("DownloadFileVisibility");
                    OnPropertyChanged("SaveFileVisibility");
                    OnPropertyChanged("ProcessVisible");
                    OnPropertyChanged("ImageSource");
                    OnPropertyChanged("EditNoteVisibility");
                }
            }
        }

        private bool _checkForPlay;
        public bool CheckForPlay
        {
            get { return _checkForPlay; }
            set { _checkForPlay = value; }
        }

        public Visibility CheckForPlayVisibility
        {
            get
            {
                return this.DownloadStatus == DownloadStatus.DownloadFinished ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility DownloadFileVisibility
        {
            get
            {
                return this.DownloadStatus == DownloadStatus.UnDownload ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility SaveFileVisibility
        {
            get
            {
                return this.DownloadStatus == DownloadStatus.DownloadFinished ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility ProcessVisible
        {
            get
            {
                return DownloadStatus == DownloadStatus.Downloading ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility EditNoteVisibility
        {
            get
            {
                return DownloadStatus == DownloadStatus.UnDownload ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private decimal _processValue;
        public decimal ProcessValue
        {
            get
            {
                return _processValue;
            }
            set
            {
                if (_processValue != value)
                {
                    if (value >= 0 && value <= 100)
                    {
                        _processValue = value;
                        this.OnPropertyChanged("ProcessValue");
                    }
                }
            }
        }

        public string Duration
        {
            get
            {
                return (Model.EndTime - Model.StartTime).ToString();
            }
        }

        private string _url;

        public string Url
        {
            get { return _url; }
            set
            {
                _url = value;
                this.OnPropertyChanged("Url");
            }
        }

        public string ImageSource
        {
            get
            {
                if (this.DownloadStatus == DownloadStatus.UnDownload)
                {
                    return "/ExternalResource;component/Images/Video/UnDownloadVideo.png";
                }
                else if (this.DownloadStatus == DownloadStatus.Downloading)
                {
                    return "/ExternalResource;component/Images/Video/DownloadingVideo.png";
                }
                else if (this.Model.VideoType == (int)VideoTypeEnum.AlarmVideo)
                {
                    return "/ExternalResource;component/Images/Video/AlarmVideo.png";
                }
                else if (this.Model.VideoType == (int)VideoTypeEnum.CommonVideo || this.DownloadStatus == DownloadStatus.DownloadFinished)
                {
                    return "/ExternalResource;component/Images/Video/ServerVideo.png";
                }

                return "";
            }
        }

        private void OnPropertyChanged(string propName)
        {
            if (!string.IsNullOrWhiteSpace(propName) && PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
