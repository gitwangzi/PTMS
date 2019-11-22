using Gsafety.PTMS.BasicPage.Entity;
using Gsafety.PTMS.ServiceReference.MessageServiceExt;
using Gsafety.PTMS.ServiceReference.VedioService;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using Jounce.Framework.Command;
using System;
using System.ComponentModel.Composition;
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
using System.Linq;
using Gsafety.Common.CommMessage;
using System.Windows.Data;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Common.Data.Enum;
using Gsafety.PTMS.Common.Data;
using System.Collections.Generic;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.Common.Controls;
using System.Reflection;

namespace Gsafety.PTMS.BasicPage.ViewModels
{
    public class HistoryVideoManageContentViewModel : VideoBaseVm, IEventSink<QueryServerFileListMessageResponse>, IPartImportsSatisfiedNotification
    {
        #region 字段
        public static string VDM_VideoServerFaild = ApplicationContext.Instance.StringResourceReader.GetString("VDM_VideoServerFaild");
        VideoInfoItem selecteditem = null;
        private VedioServiceClient _client;
        DispatcherTimer StatuTimer = null;
        private TimeSpan _halfHourSpan = TimeSpan.FromMinutes(30);
        #endregion

        #region 属性
        ICommand _mdvrdownloadcommand = null;
        public ICommand MdvrDownLoadCommand
        {
            get
            {
                return _mdvrdownloadcommand;
            }
        }

        ICommand _noteEditCommand = null;
        public ICommand EditNoteCommand
        {
            get
            {
                return _noteEditCommand;
            }
        }

        public List<VideoInfoItem> SelectVideoInfoItems
        {
            get
            {
                return _items.Where(t => t.DownloadStatus == DownloadStatus.DownloadFinished && t.CheckForPlay).ToList();
            }
        }
        #endregion

        public HistoryVideoManageContentViewModel(string carNo = "")
        {
            try
            {
                CompositionInitializer.SatisfyImports(this);

                _mdvrdownloadcommand = new ActionCommand<VideoInfoItem>(x =>
                {
                    selecteditem = x;
                    DownloadFile();
                });

                _noteEditCommand = new ActionCommand<object>(x => EditNote(x));

                QueryCommand = new ActionCommand<string>(x =>
                {
                    Query();
                });

                _client = ServiceClientFactory.Create<VedioServiceClient>();
                _client.QueryServerFileListCompleted += _client_QueryServerFileListCompleted;
                _client.QueryDownloadStatusCompleted += _client_QueryDownloadStatusCompleted;

                this.CarNo = carNo;

                StatuTimer = new DispatcherTimer();
                StatuTimer.Interval = TimeSpan.FromMilliseconds(1000);
                StatuTimer.Tick += _statuTimer_Tick;
                StatuTimer.Start();

                this.Query();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                if (viewParameters == null)
                    return;

                var q = viewParameters.Values.FirstOrDefault(x => x is VideoDownLoadArgs);
                if (q == null) return;

                var arg = (VideoDownLoadArgs)q;
                CarNo = arg.CarNo;
                this.StartTime = arg.StartTime;
                this.EndTime = arg.EndTime;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void Query()
        {
            try
            {
                if (IsValidQuery(false))
                {
                    IsQueryBusy = true;

                    _items.Clear();

                    QueryServerFileList();

                    QueryMdvrFileList();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region 备注

        private void EditNote(object item)
        {
            var info = item as VideoInfoItem;

            var noteWindow = new EditNoteWindow(info.Model);
            noteWindow.Show();
        }

        #endregion

        #region 查询已下载视频
        private void QueryServerFileList()
        {
            var arg = new QueryServerFileListArgs
            {
                Channel = 99,
                Start_Time = StartTime.Value.ToUniversalTime(),
                End_Time = EndTime.Value.ToUniversalTime(),
                Video_Type = (int)VideoTypeEnum.All,
                MdvrCoreSn = this.MdvrId,
                PageNum = 0,
                PageSize = 0,
            };

            _client.QueryServerFileListAsync(arg);
        }

        void _client_QueryServerFileListCompleted(object sender, QueryServerFileListCompletedEventArgs e)
        {
            try
            {
                this.IsQueryBusy = false;
                if (!e.Result.IsSuccess)
                {
                    //MessageBox.Show(VDM_VideoServerFaild, ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageDialogButton.Ok);
                }
                if (e.Result != null && e.Result.Result != null)
                {
                    var temp = e.Result.Result.OrderBy(x => x.StartTime).Where(p => p.EndTime > p.StartTime).Select(x =>
                    {
                        x.FileSize = ConvertFileSize(x.FileSize);
                        x.StartTime = x.StartTime.ToLocalTime();
                        x.EndTime = x.EndTime.ToLocalTime();

                        return new VideoInfoItem
                        {
                            DownloadStatus = (DownloadStatus)x.DownloadStatus,
                            Model = x,
                            Url = string.Format("http://{0}:{1}/{2}", ApplicationContext.Instance.ServerConfig.VideoServiceFileServerIP, ApplicationContext.Instance.ServerConfig.VideoServiceFileServerPort, x.FileID),
                        };
                    }).ToList();
                    _items.AddRange(temp);
                    DataGridItems = new PagedCollectionView(_items);
                }
            }
            catch (Exception)
            {
            }

            this.RaisePropertyChanged(() => DataGridItems);
        }
        #endregion

        #region 下载
        private void DownloadFile()
        {
            var window = new TimeRangeSelectWindow(selecteditem.Model.StartTime, selecteditem.Model.EndTime);

            window.Closed += window_Closed;
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            var window = sender as TimeRangeSelectWindow;
            if (window.DialogResult == false)
            {
                return;
            }

            SendDownloadCommand(window.StartDateTime, window.EndDateTime);
        }

        private void SendDownloadCommand(DateTime startTime, DateTime endTime)
        {
            var model = selecteditem.Model;

            var Aargs = new DownloadFile()
            {
                UUID = Guid.NewGuid().ToString(),
                MdvrCoreSn = this.MdvrId,
                ChannelID = model.Channel.ToString(),
                StartTime = startTime.ToUniversalTime(),
                EndTime = endTime.ToUniversalTime(),
                FileId = model.FileID,
                OffSet = 0,
                OffSetFlag = 2,
                FileType = model.VideoType,
                FileSize = (decimal)((endTime - startTime).TotalSeconds / (model.EndTime - model.StartTime).TotalSeconds) * model.FileSize * 1024 * 1024
            };

            var newFileMessage = new VideoInfoItem()
            {
                Model = new Gsafety.PTMS.ServiceReference.VedioService.QueryServerFileListMessage()
                {
                    //CameraInstallLocation = model.CameraInstallLocation,
                    Channel = model.Channel,
                    DownloadStatus = model.DownloadStatus,
                    EndTime = endTime,
                    StartTime = startTime,
                    FileID = model.FileID,
                    FileSize = Aargs.FileSize / 1024 / 1024,
                    UUID = Aargs.UUID,
                    VehicleSN = model.VehicleSN,
                    VideoType = model.VideoType
                },
                CameraInstallLocation = selecteditem.CameraInstallLocation,
                DownloadStatus = DownloadStatus.Downloading,
            };

            var index = DataGridItems.IndexOf(selecteditem);
            _items.Insert(index + 1, newFileMessage);

            DataGridItems = new PagedCollectionView(_items);

            //model.UUID = Aargs.UUID;
            //selecteditem.DownloadStatus = DownloadStatus.Downloading;

            ApplicationContext.Instance.MessageClient.SendDownloadMdvrFile(Aargs);

            LogVideoClient client = null;
            try
            {
                var videoLogs = new ObservableCollection<LogVideo>();
                var log = new LogVideo()
                {
                    ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                    Channel = (short)newFileMessage.Model.Channel,
                    ID = Guid.NewGuid().ToString(),
                    LogType = (short)VideoLogTypeEnum.Download,
                    MdvrCoreSn = this.MdvrId,
                    OperateTime = DateTime.UtcNow,
                    OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID,
                    OperatorName = ApplicationContext.Instance.AuthenticationInfo.UserName,
                    VehicleID = this.CarNo,
                };
                videoLogs.Add(log);

                client = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.PTMSLogManageService.LogVideoClient>();
                client.InsertVideoDownloadLogAsync(videoLogs);
            }
            catch (System.Exception ex)
            {
            }
            finally
            {
                if (client != null)
                {
                    client.CloseAsync();
                }
            }
        }
        #endregion

        #region 查询MDVR视频
        private void QueryMdvrFileList()
        {
            if (_isOnline == false)
            {
                return;
            }

            var list = new Gsafety.PTMS.ServiceReference.MessageServiceExt.QueryMdvrFileList();
            list.Channel = new ObservableCollection<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
            list.Mdvr_Id = this.MdvrId;
            list.Start_Time = this.StartTime.Value.ToUniversalTime();
            list.End_Time = this.EndTime.Value.ToUniversalTime();
            list.FileType = (int)VideoTypeEnum.All;
            list.StreamType = 1;
            list.UserID = ApplicationContext.Instance.AuthenticationInfo.UserID;

            ApplicationContext.Instance.MessageClient.SendGetVideoListCMD(list);
        }

        public void HandleEvent(QueryServerFileListMessageResponse publishedEvent)
        {
            _items.RemoveAll(x => x.DownloadStatus == DownloadStatus.UnDownload);
            IsQueryBusy = false;
            var temp = publishedEvent.QueryServerFileListMessages
                      .OrderBy(x => x.StartTime)
                      .Where(x => (x.EndTime - x.StartTime) > TimeSpan.FromMinutes(1)).ToList();
            if (temp.Count == 0)
            {
                return;
            }


            foreach (var x in temp)
            {
                //var endTime = x.StartTime.Date.AddHours(x.StartTime.Hour).Add(_halfHourSpan);
                //if (x.StartTime.Minute > 30)
                //{
                //    endTime = endTime.Add(_halfHourSpan);
                //}
                //var startTime = x.StartTime;
                //bool hasEnd = false;

                //while (true)
                //{
                //    if (endTime > x.EndTime)
                //    {
                //        endTime = x.EndTime;
                //        hasEnd = true;
                //    }

                //if (_items.Any(t => t.Model.Channel == x.Channel && t.Model.StartTime <= x.StartTime && x.EndTime <= t.Model.EndTime) == false)
                //{
                    var item = new VideoInfoItem
                    {
                        DownloadStatus = DownloadStatus.UnDownload,
                        Model = new Gsafety.PTMS.ServiceReference.VedioService.QueryServerFileListMessage()
                        {
                            //CameraInstallLocation = x.CameraInstallLocation,
                            Channel = x.Channel,
                            DownloadStatus = x.DownloadStatus,
                            EndTime = x.EndTime.ToLocalTime(),
                            StartTime = x.StartTime.ToLocalTime(),
                            FileID = x.FileID,
                            FileSize = ConvertFileSize(x.FileSize * (decimal)((x.EndTime - x.StartTime).TotalSeconds / (x.EndTime - x.StartTime).TotalSeconds)),
                            UUID = x.UUID,
                            VehicleSN = x.VehicleSN,
                            VideoType = x.VideoType
                        }
                    };
                    _items.Add(item);
                //}

                //if (hasEnd)
                //{
                //    break;
                //}

                //startTime = endTime;
                //endTime = endTime.Add(_halfHourSpan);
                //}
            }

            DataGridItems = new PagedCollectionView(_items);
        }
        #endregion

        #region 更新下载进度

        void _statuTimer_Tick(object sender, EventArgs e)
        {
            QueryDownloadStatusArgs _statuArgs = new QueryDownloadStatusArgs();
            _statuArgs.FileIDs = new ObservableCollection<string>();
            lock (_items)
            {
                var downloadItems = _items.Where(t => t.DownloadStatus == DownloadStatus.Downloading).ToList();
                if (downloadItems.Count != 0)
                {
                    foreach (var item in downloadItems)
                    {
                        _statuArgs.FileIDs.Add(item.Model.UUID);
                    }

                    _client.QueryDownloadStatusAsync(_statuArgs);
                }
            }
        }

        void _client_QueryDownloadStatusCompleted(object sender, QueryDownloadStatusCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                lock (_items)
                {
                    var downloadItems = _items.Where(t => t.DownloadStatus == DownloadStatus.Downloading).ToList();
                    for (int i = downloadItems.Count - 1; i >= 0; i--)
                    {
                        var item = downloadItems[i].Model as Gsafety.PTMS.ServiceReference.VedioService.QueryServerFileListMessage;
                        var status = e.Result.Result.FirstOrDefault(n => n.FileID.Trim() == item.UUID.Trim());
                        if (status != null)
                        {
                            if ((int)status.Status == (int)DownloadStatus.DownloadFinished)
                            {
                                downloadItems[i].Model.FileID = status.Url;
                                downloadItems[i].DownloadStatus = DownloadStatus.DownloadFinished;
                                downloadItems[i].Url = string.Format("http://{0}:{1}/{2}", ApplicationContext.Instance.ServerConfig.VideoServiceFileServerIP, ApplicationContext.Instance.ServerConfig.VideoServiceFileServerPort, status.Url);
                            }
                            else if ((int)status.Status == (int)DownloadStatus.Downloading)
                            {
                                downloadItems[i].DownloadStatus = DownloadStatus.Downloading;
                                var process = ConvertFileSize(status.Percent) / item.FileSize;
                                if (process >= 1)
                                {
                                    process = new decimal(0.99);
                                }
                                downloadItems[i].ProcessValue = process;
                            }
                            else if ((int)status.Status == (int)DownloadStatus.DownloadError)
                            {
                                downloadItems[i].DownloadStatus = DownloadStatus.DownloadError;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<QueryServerFileListMessageResponse>(this);
        }

        private decimal ConvertFileSize(decimal source)
        {
            return source / 1024 / 1024;
        }
    }
}
