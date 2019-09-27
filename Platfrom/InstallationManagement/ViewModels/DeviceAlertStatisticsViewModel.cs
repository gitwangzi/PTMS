using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Installation;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(InstallationName.DeviceAlertStatisticsVm)]
    public class DeviceAlertStatisticsViewModel : ListViewModel<DeviceAlertStatistics>
    {
        public DeviceAlertStatisticsViewModel()
        {
            _selectOrganizationCommand = new ActionCommand<object>(q => SelectOrganizationCommandExecute(q));
        }

        private DeviceInstallServiceClient InitialClient()
        {
            DeviceInstallServiceClient client = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetDeviceAlertStatisticsViewListCompleted += client_GetDeviceAlertStatisticsViewListCompleted;
            return client;
        }

        void client_GetDeviceAlertStatisticsViewListCompleted(object sender, GetDeviceAlertStatisticsViewListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Data.loader_Finished(new BaseLib.Model.PagedResult<DeviceAlertStatistics>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,//数据列表
                            PageIndex = currentIndex
                        });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceAlertStatisticsViewModel", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }


        private string _organizationName;
        /// <summary>
        /// 前台显示的组织机构名称
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
                RaisePropertyChanged(() => OrganizationName);
            }
        }

        protected string _organizationID = string.Empty;

        readonly ICommand _selectOrganizationCommand;
        public ICommand SelectOrganizationCommand
        {
            get
            {
                return this._selectOrganizationCommand;
            }
        }

        private void SelectOrganizationCommandExecute(object obj)
        {
            SelecSignleOrganizationWindow window = new SelecSignleOrganizationWindow(ApplicationContext.Instance.AuthenticationInfo.UserID);
            window.Show();
            window.Closed += window_Closed;
        }

        private void window_Closed(object sender, EventArgs e)
        {
            try
            {
                SelecSignleOrganizationWindow window = sender as SelecSignleOrganizationWindow;
                if (window.DialogResult == true)
                {
                    this.OrganizationName = "Selected";
                    var result = window._viewModel.SelectedOrganizationItem;
                    if (result != null)
                    {
                        this.OrganizationName = result.Name;
                        _organizationID = result.ID;
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private static void CloseClient(object sender)
        {
            DeviceInstallServiceClient client = sender as DeviceInstallServiceClient;
            client.CloseAsync();
            client = null;
        }

        /// <summary>
        /// 查询   page有点问题要解决
        /// </summary>
        protected override void Query()
        {
            try
            {
                currentIndex = 1;
                Data.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        string vehicleid = string.Empty;

        public string VehicleID
        {
            get { return vehicleid; }
            set { vehicleid = value; }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                if (EndTime.HasValue)
                {
                    if (EndTime.Value > DateTime.Now.Date.AddDays(1))
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("Rpt_EndTimeError"));
                        return;
                    }
                }

                Data = new BaseLib.Model.PagedServerCollection<DeviceAlertStatistics>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    DeviceInstallServiceClient client = InitialClient();
                    ObservableCollection<string> stations = new ObservableCollection<string>();
                    foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Stations)
                    {
                        stations.Add(item.ID);
                    }
                    client.GetDeviceAlertStatisticsViewListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, this._organizationID, this.VehicleID, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).Date.ToUniversalTime(), stations, pageSize, pageIndex);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }


        private DateTime? beginTime = DateTime.Now.AddMonths(-1);
        /// <summary>
        /// 
        /// </summary>
        public DateTime? BeginTime
        {
            get
            {
                return beginTime;
            }
            set
            {
                this.beginTime = value;
                if (BeginTime != null && EndTime != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginTime), (DateTime)BeginTime, ExtractPropertyName(() => EndTime), (DateTime)EndTime);
                RaisePropertyChanged(() => this.BeginTime);
            }
        }
        private DateTime? endTime = DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                this.endTime = value;             
                if (BeginTime != null && EndTime != null)
                    ValidateBeginAndEndDate(ExtractPropertyName(() => BeginTime), (DateTime)BeginTime, ExtractPropertyName(() => EndTime), (DateTime)EndTime);
                RaisePropertyChanged(() => this.EndTime);
            }
        }
    }
}
