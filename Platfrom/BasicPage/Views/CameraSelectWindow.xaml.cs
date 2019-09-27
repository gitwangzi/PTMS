//using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using System;
using Gsafety.PTMS.Common.Data.Enum;


namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class CameraSelectWindow : ChildWindow
    {
        private string _mdvrCoreID;
        private List<CheckBox> CheckBoxList = new List<CheckBox>();
        private ObservableCollection<CameraInfo> CameraInfos = new ObservableCollection<CameraInfo>();
        //private FunctionMode _mode;
        private int _maxSelectCount;

        public List<MediaInfo.MediaInfoItem> SelectResult = new List<MediaInfo.MediaInfoItem>();

        public CameraSelectWindow(string mdvrCoreID, int maxSelectCount = 4)
        {
            InitializeComponent();

            CheckBoxList.Add(OuterBehind);
            CheckBoxList.Add(OuterBefore);
            CheckBoxList.Add(OuterRight);
            CheckBoxList.Add(OuterLeft);
            CheckBoxList.Add(InnerLeftDriver);
            CheckBoxList.Add(InnerRightDriver);
            CheckBoxList.Add(InnerCenter);
            CheckBoxList.Add(InnerBehind);

            _mdvrCoreID = mdvrCoreID;
            _maxSelectCount = maxSelectCount;

            GetCameraInstallLocationInfo();

            this.MouseRightButtonUp += PopupHandler.popup_MouseRightButtonDown;
        }

        private void SetCameraVisibility()
        {
            foreach (var item in CameraInfos)
            {
                switch (item.InstallLocation)
                {
                    case CameraInstallLocationEnum.InnerBehind:
                        InnerBehind.Visibility = Visibility.Visible;
                        InnerBehind.Tag = item;
                        break;
                    case CameraInstallLocationEnum.InnerCenter:
                        InnerCenter.Visibility = Visibility.Visible;
                        InnerCenter.Tag = item;
                        break;
                    case CameraInstallLocationEnum.InnerLeftDriver:
                        InnerLeftDriver.Visibility = Visibility.Visible;
                        InnerLeftDriver.Tag = item;
                        break;
                    case CameraInstallLocationEnum.InnerRightDriver:
                        InnerRightDriver.Visibility = Visibility.Visible;
                        InnerRightDriver.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterBefore:
                        OuterBefore.Visibility = Visibility.Visible;
                        OuterBefore.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterBehind:
                        OuterBehind.Visibility = Visibility.Visible;
                        OuterBehind.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterLeft:
                        OuterLeft.Visibility = Visibility.Visible;
                        OuterLeft.Tag = item;
                        break;
                    case CameraInstallLocationEnum.OuterRight:
                        OuterRight.Visibility = Visibility.Visible;
                        OuterRight.Tag = item;
                        break;
                }
            }

            //OuterBehind.Visibility = Visibility.Collapsed;
            //OuterBefore.Visibility = Visibility.Collapsed;
            //OuterRight.Visibility = Visibility.Collapsed;
            //OuterLeft.Visibility = Visibility.Collapsed;
            //InnerLeftDriver.Visibility = Visibility.Visible;
            //InnerRightDriver.Visibility = Visibility.Visible;
            //InnerCenter.Visibility = Visibility.Visible;
            //InnerBehind.Visibility = Visibility.Visible;
        }

        private void GetCameraInstallLocationInfo()
        {
            var client = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            client.GetCameraInfoByMdvrIDCompleted += client_GetCameraInfoByMdvrIDCompleted;
            client.GetCameraInfoByMdvrIDAsync(_mdvrCoreID);
        }

        void client_GetCameraInfoByMdvrIDCompleted(object sender, GetCameraInfoByMdvrIDCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            if (e.Error != null)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                return;
            }

            try
            {
                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }

                if (result.Result != null)
                {
                    CameraInfos = result.Result;

                    SetCameraVisibility();
                }
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            var selectCameras = CheckBoxList.Where(t => t.IsChecked == true).Select(t => t.Tag as CameraInfo).ToList();
            if (selectCameras.Count == 0)
            {
                return;
            }

            if (selectCameras.Count > _maxSelectCount)
            {
                MessageBoxHelper.ShowDialog(string.Format(ApplicationContext.Instance.StringResourceReader.GetString("HistoryVideoSelectMaxCount"), _maxSelectCount));

                return;
            }

            var infos = new List<MediaInfo.MediaInfoItem>();
            foreach (var item in selectCameras)
            {
                infos.Add(new MediaInfo.MediaInfoItem()
                {
                    Url = string.Format("{0}:{1}", item.ChannelID, _mdvrCoreID),
                    Channel = int.Parse(item.ChannelID),
                    IsRealVideo = true,
                    IsShowControlBar = true,
                    IsShowProcessBar = false,
                    ShowRemoveBtn = false,
                });
            }

            SelectResult = infos;

            if (selectCameras.Count > 0)
            {
                LogVideoClient client = null;
                try
                {
                    var videoLogs = new ObservableCollection<LogVideo>();
                    var suite = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.UniqueId == _mdvrCoreID);
                    foreach (var item in selectCameras)
                    {
                        var log = new LogVideo()
                        {
                            ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                            Channel = short.Parse(item.ChannelID),
                            ID = Guid.NewGuid().ToString(),
                            LogType = (short)VideoLogTypeEnum.Play,
                            MdvrCoreSn = _mdvrCoreID,
                            OperateTime = DateTime.UtcNow,
                            OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID,
                            OperatorName = ApplicationContext.Instance.AuthenticationInfo.UserName,
                            VehicleID = suite.VehicleId,
                        };
                        videoLogs.Add(log);
                    }

                    client = ServiceClientFactory.Create<Gsafety.PTMS.ServiceReference.PTMSLogManageService.LogVideoClient>();
                    client.InsertVideoPlayLogAsync(videoLogs);
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

            this.DialogResult = true;

            //if (_mode == FunctionMode.Monitor)
            //{
            //    var info = new MediaInfo()
            //    {
            //        MediaInfoItems = infos,
            //        RealVideoMode = true
            //    };

            //    ApplicationContext.Instance.EventAggregator.Publish<MediaInfo>(info);
            //}
            //else
            //{
            //    var info = new MediaInfoEx()
            //    {
            //        MediaInfoItems = infos,
            //        RealVideoMode = true
            //    };

            //    ApplicationContext.Instance.EventAggregator.Publish<MediaInfoEx>(info);
            //}
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        //public enum FunctionMode
        //{
        //    Monitor = 0,
        //    VideoWall,
        //}
    }
}

