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
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.AlertDisposeLogVm)]
    public class AlertDisposedLogViewModel : SystemLogBaseViewModel<CarAlertLogInfo>
    {
        private CarAlertDealLogServiceClient InitialClient()
        {
            CarAlertDealLogServiceClient client = ServiceClientFactory.Create<CarAlertDealLogServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetCarAlertDealLogCompleted += client_GetCarAlertDealLogCompleted;
            return client;
        }

        void client_GetCarAlertDealLogCompleted(object sender, GetCarAlertDealLogCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            int state = (int)item.AlertType;
                            switch (state)
                            {
                                case 0:
                                    item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_OverSpeed");
                                    break;
                                case 1:
                                    item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_InOutAera");
                                    break;
                                case 2:
                                    item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_InOutRoute");
                                    break;
                                case 3:
                                    item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_RouteOffset");
                                    break;
                            }


                            item.AlertTime = item.AlertTime.Value.ToLocalTime();
                            item.DealTime = item.DealTime.Value.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<CarAlertLogInfo>()
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
                ApplicationContext.Instance.Logger.LogException("client_GetCarAlertDealLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            CarAlertDealLogServiceClient client = sender as CarAlertDealLogServiceClient;
            client.CloseAsync();
            client = null;
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<CarAlertLogInfo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    CarAlertDealLogServiceClient client = InitialClient();
                    client.GetCarAlertDealLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, Disposer, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), pagingInfo);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private string _disposer;
        /// <summary>
        /// 
        /// </summary>
        public string Disposer
        {
            get
            {
                return _disposer;
            }
            set
            {
                this._disposer = value;
                RaisePropertyChanged(() => this.Disposer);
            }
        }

        private string _vehicleid;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleID
        {
            get
            {
                return _vehicleid;
            }
            set
            {
                this._vehicleid = value;
                RaisePropertyChanged(() => this.VehicleID);
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
                    CarAlertDealLogServiceClient client = ServiceClientFactory.Create<CarAlertDealLogServiceClient>();
                    client.GetCarAlertDealLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                int state = (int)item.AlertType;
                                switch (state)
                                {
                                    case 0:
                                        item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_OverSpeed");
                                        break;
                                    case 1:
                                        item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_InOutAera");
                                        break;
                                    case 2:
                                        item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_InOutRoute");
                                        break;
                                    case 3:
                                        item.ShowAlertType = ApplicationContext.Instance.StringResourceReader.GetString("BusinessAlertType_RouteOffset");
                                        break;
                                }


                                item.AlertTime = item.AlertTime.Value.ToLocalTime();
                                item.DealTime = item.DealTime.Value.ToLocalTime();
                            }

                            List<string> Codes = new List<string>();
                            Codes.Add("CarNumber");
                            Codes.Add("VihcleType");
                            Codes.Add("DealPerson");
                            Codes.Add("AlertTime");
                            Codes.Add("DealTime");
                            Codes.Add("ShowAlertType");
                            Codes.Add("DealContent");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleId"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealPerson"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_ALERTTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DellTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALERT_AlertType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealContent"));

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
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = 10000 };
                        client.GetCarAlertDealLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, Disposer, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetCarAlertDealLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, Disposer, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
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
