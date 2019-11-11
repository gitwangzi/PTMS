using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Spreadsheet;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.InstallLogVm)]
    public class InstallLogViewModel : SystemLogBaseViewModel<InstallLogInfo>
    {
        private InstallLogServiceClient InitialClient()
        {
            InstallLogServiceClient client = ServiceClientFactory.Create<InstallLogServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetInstallLogCompleted += client_GetInstallLogCompleted;
            return client;
        }

        private InstallStationServiceClient InitialStationService()
        {
            InstallStationServiceClient client = ServiceClientFactory.Create<InstallStationServiceClient>();

            client.GetInstallStationsCompleted += client_GetInstallStationsCompleted;

            return client;
        }

        void client_GetInstallStationsCompleted(object sender, GetInstallStationsCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            _stations.Add(new NameValueModel<string>() { Name = item.Name, Value = item.ID });
                        }

                        RaisePropertyChanged(() => InstallStations);
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
                CloseStationClient(sender);
            }
        }

        private void CloseStationClient(object sender)
        {
            InstallStationServiceClient client = sender as InstallStationServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
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

        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                _stations.Clear();
                InstallStationServiceClient client = InitialStationService();
                client.GetInstallStationsAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
                _stations.Add(new NameValueModel<string>() { Name = ApplicationContext.Instance.StringResourceReader.GetString("All"), Value = string.Empty });

                RaisePropertyChanged(() => InstallStations);
                InstallStation = InstallStations[0];

                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_GetInstallLogCompleted(object sender, GetInstallLogCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.InstalledTime = item.InstalledTime.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<InstallLogInfo>()
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
                ApplicationContext.Instance.Logger.LogException("client_GetInstallLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            InstallLogServiceClient client = sender as InstallLogServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<InstallLogInfo>((pageIndex, pageSize) =>
                {
                    if (InstallStation != null)
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                        Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                        InstallLogServiceClient client = InitialClient();
                        client.GetInstallLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, InstallStation.Value, InstallStaff, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), pagingInfo);
                    }
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private string installstaff;
        /// <summary>
        /// 
        /// </summary>
        public string InstallStaff
        {
            get
            {
                return installstaff;
            }
            set
            {
                this.installstaff = value;
                RaisePropertyChanged(() => this.InstallStaff);
            }
        }

        protected override void LogQueryAction(object obj)
        {
            base.LogQueryAction(obj);
            try
            {
                Data.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected override void ExportAction(object obj)
        {
            base.ExportAction(obj);
            if (!base.ExportDate())
            {
                return;
            }
            try
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Excel Document (.xlsx)|*.xlsx";
                dlg.DefaultExt = ".xlsx";
                bool? dialogResult = dlg.ShowDialog();
                if (dialogResult == true)
                {
                    setExportBtnStatus(false);
                    InstallLogServiceClient client = ServiceClientFactory.Create<InstallLogServiceClient>();
                    client.GetInstallLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                item.InstalledTime = item.InstalledTime.ToLocalTime();
                            }
                            List<string> Codes = new List<string>();
                            Codes.Add("SetupStaff");
                            Codes.Add("SetupStation");
                            Codes.Add("Vechicle_ID");
                            Codes.Add("SuiteID");
                            Codes.Add("InstalledTime");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_InstalledPerson"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Support_StationName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_CarNo"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ANTNumber"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_InstallTime"));

                            XLSXExporter xlsx = new XLSXExporter();
                            xlsx.Export(e.Result.Result.ToList(), dlg.OpenFile(), Codes, Names);
                            setExportBtnStatus(true);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportSucceed"), MessageDialogButton.Ok);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), ApplicationContext.Instance.StringResourceReader.GetString("BaseInfo_ExportFaild"), MessageDialogButton.Ok);
                        }

                        CloseClient(client);
                    };
                    ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

                    if (Data.TotalItemCount > 10000)
                    {
                        Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetInstallLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, InstallStation.Value, InstallStaff, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                    else
                    {
                        Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetInstallLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, InstallStation.Value, InstallStaff, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
