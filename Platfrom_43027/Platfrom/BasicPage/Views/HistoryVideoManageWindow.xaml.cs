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
using Gsafety.PTMS.BasicPage.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.Entity;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.Common.Data.Enum;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class HistoryVideoManageWindow : ChildWindow
    {
        private int _maxSelectCount;
        public HistoryVideoManageContentViewModel HistoryVideoManageContentViewModel;

        private string _historyVideoSelectMaxCountString;

        public List<VideoInfoItem> SelectVideoInfoItems
        {
            get
            {
                return HistoryVideoManageContentViewModel.SelectVideoInfoItems;
            }
        }

        public HistoryVideoManageWindow(string carNo = "", int maxSelectCount = 4)
        {
            InitializeComponent();

            _maxSelectCount = maxSelectCount;
            HistoryVideoManageContentViewModel = new HistoryVideoManageContentViewModel(carNo);
            historyVideoManagerView.DataContext = HistoryVideoManageContentViewModel;

            historyVideoManagerView.CheckBoxCheckHandle = CheckBox_Checked;

            _historyVideoSelectMaxCountString = string.Format(ApplicationContext.Instance.StringResourceReader.GetString("HistoryVideoSelectMaxCount"), _maxSelectCount);
            MouseRightButtonUp += PopupHandler.popup_MouseRightButtonDown;
            MouseRightButtonDown += PopupHandler.popup_MouseRightButtonDown;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (SelectVideoInfoItems.Count == _maxSelectCount)
            {
                var checkBox = sender as CheckBox;
                checkBox.IsChecked = false;

                MessageBoxHelper.ShowDialog(_historyVideoSelectMaxCountString);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (HistoryVideoManageContentViewModel.SelectVideoInfoItems.Count > _maxSelectCount)
            {
                MessageBoxHelper.ShowDialog(_historyVideoSelectMaxCountString);

                return;
            }

            if (HistoryVideoManageContentViewModel.SelectVideoInfoItems.Count == 0)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("HistoryVideoSelectCountZero"));

                return;
            }

            if (HistoryVideoManageContentViewModel.SelectVideoInfoItems.Count > 0)
            {
                LogVideoClient client = null;
                try
                {
                    var videoLogs = new ObservableCollection<LogVideo>();
                    var suite = ApplicationContext.Instance.BufferManager.VehicleOrganizationManage.VehicleList.FirstOrDefault(t => t.UniqueId == HistoryVideoManageContentViewModel.MdvrId);
                    foreach (var item in HistoryVideoManageContentViewModel.SelectVideoInfoItems)
                    {
                        var log = new LogVideo()
                        {
                            ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID,
                            Channel = (short)item.CameraInstallLocation,
                            ID = Guid.NewGuid().ToString(),
                            LogType = (short)VideoLogTypeEnum.Play,
                            MdvrCoreSn = HistoryVideoManageContentViewModel.MdvrId,
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
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

