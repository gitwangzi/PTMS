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
    [ExportAsViewModel(InstallationName.InstallStatisticsVm)]
    public class InstallStatisticsViewModel : ListViewModel<InstallStatisticsView>
    {
        public InstallStatisticsViewModel()
        {
            _selectOrganizationCommand = new ActionCommand<object>(q => SelectOrganizationCommandExecute(q));
        }

        private VehicleTypeClient InitVehicleTypeClient()
        {
            VehicleTypeClient vehicleClient = ServiceClientFactory.Create<VehicleTypeClient>();
            vehicleClient.GetVehicleTypeListCompleted += vehicleClient_GetVehicleTypeListCompleted;

            return vehicleClient;
        }

        void vehicleClient_GetVehicleTypeListCompleted(object sender, GetVehicleTypeListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            vehciletypes.Add(new NameValueModel<string>() { Name = item.Name, Value = item.ID });
                        }

                        RaisePropertyChanged(() => VehicleTypes);
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        List<NameValueModel<string>> vehciletypes = new List<NameValueModel<string>>();

        public List<NameValueModel<string>> VehicleTypes
        {
            get { return vehciletypes; }
            set { vehciletypes = value; }
        }

        private NameValueModel<string> vehicletype = null;

        public NameValueModel<string> VehicleType
        {
            get { return vehicletype; }
            set { vehicletype = value; }
        }

        private DeviceInstallServiceClient InitialClient()
        {
            DeviceInstallServiceClient client = ServiceClientFactory.Create<DeviceInstallServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetInstallStatisticsViewListCompleted += client_GetInstallStatisticsViewListCompleted;
            return client;
        }

        void client_GetInstallStatisticsViewListCompleted(object sender, GetInstallStatisticsViewListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        Data.loader_Finished(new BaseLib.Model.PagedResult<InstallStatisticsView>()
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
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallStatisticsViewModel", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        List<NameValueModel<string>> _stations = new List<NameValueModel<string>>();

        public List<NameValueModel<string>> InstallStations
        {
            get { return _stations; }
            set
            {
                _stations = value;
            }
        }

        NameValueModel<string> station = null;

        public NameValueModel<string> InstallStation
        {
            get { return station; }
            set
            {
                station = value;
                RaisePropertyChanged(() => InstallStation);
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
            SelecSignleOrganizationWindow window = sender as SelecSignleOrganizationWindow;
            if (window.DialogResult == true)
            {
                this.OrganizationName = "Selected";
                var result = window._viewModel.SelectedOrganizationItem;
                if (result != null)
                {
                    this.OrganizationName = result.Organization.Name;
                    _organizationID = result.Organization.ID;
                }
            }
        }


        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                _stations.Clear();
                _stations.Add(new NameValueModel<string>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("All"), Value = string.Empty });

                foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Stations)
                {
                    _stations.Add(new NameValueModel<string> { Name = item.Name, Value = item.ID });
                }

                RaisePropertyChanged(() => InstallStations);
                InstallStation = InstallStations[0];
                RaisePropertyChanged(() => InstallStation);

                VehicleTypeClient client = InitVehicleTypeClient();
                client.GetVehicleTypeListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);

                vehciletypes.Clear();
                vehciletypes.Add(new NameValueModel<string>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("All"), Value = string.Empty });
                RaisePropertyChanged(() => VehicleTypes);

                VehicleType = vehciletypes[0];
                RaisePropertyChanged(() => VehicleType);

                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private static void CloseClient(object sender)
        {
            DeviceInstallServiceClient client = sender as DeviceInstallServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
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

                Data = new BaseLib.Model.PagedServerCollection<InstallStatisticsView>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    DeviceInstallServiceClient client = InitialClient();
                    ObservableCollection<string> stations = new ObservableCollection<string>();
                    if (InstallStation.Value == string.Empty)
                    {
                        foreach (var item in ApplicationContext.Instance.AuthenticationInfo.Stations)
                        {
                            stations.Add(item.ID);
                        }
                    }
                    else
                    {
                        stations.Add(InstallStation.Value);
                    }
                    client.GetInstallStatisticsViewListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, this._organizationID, stations, this.VehicleType.Value, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).Date.ToUniversalTime(), pagingInfo);

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
