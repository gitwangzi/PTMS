using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Spreadsheet;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(ManagerName.AlarmDisposedLogVm)]
    public class AlarmDisposedLogView : SystemLogBaseViewModel<AlarmDealLogInfo>
    {

        #region 属性

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


        #endregion


        #region 构造函数


        #endregion


        #region 方法

        /// <summary>
        /// 初始化日志服务客户端
        /// </summary>
        /// <returns></returns>
        private AlarmDealLogServiceClient InitialClient()
        {
            AlarmDealLogServiceClient client = ServiceClientFactory.Create<AlarmDealLogServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetAlarmDealLogCompleted += client_GetAlarmDealLogCompleted;
            return client;
        }


        void client_GetAlarmDealLogCompleted(object sender, GetAlarmDealLogCompletedEventArgs e)
        {
            //try
            //{
            //    if(e.Result != null)
            //    {
            //        foreach(var item in e.Result.Result)
            //        {
            //            item.AlarmTime = item.AlarmTime.Value.ToLocalTime();
            //        }
            //        Data.loader_Finished(new BaseLib.Model.PagedResult<AlarmDealLogInfo>()
            //        {
            //            Count = e.Result.TotalRecord,
            //            Items = e.Result.Result,//数据列表
            //            PageIndex = currentIndex
            //        });
            //    }
            //}
            //catch(Exception ex)
            //{
            //    throw;
            //}
            //finally
            //{
            //    CloseClient(sender);
            //}

            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            if (item.AlarmTime.HasValue)
                                item.AlarmTime = item.AlarmTime.Value.ToLocalTime();
                            if (item.DealTime.HasValue)
                                item.DealTime = item.DealTime.Value.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<AlarmDealLogInfo>()
                        {
                            Count = e.Result.TotalRecord,
                            Items = e.Result.Result,//数据列表
                            PageIndex = currentIndex
                        });
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                            ApplicationContext.Instance.Logger.LogException("client_GetAlarmDealLogCompleted", e.Result.ExceptionMessage);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException("client_GetAlarmDealLogCompleted", e.Result.ExceptionMessage);
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException("client_GetAlarmDealLogCompleted", e.Result.ExceptionMessage);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_GetAlarmDealLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }



        }

        /// <summary>
        /// 关闭服务连接
        /// </summary>
        /// <param name="sender"></param>
        private static void CloseClient(object sender)
        {
            AlarmDealLogServiceClient client = sender as AlarmDealLogServiceClient;
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        ///// <summary>
        ///// 查询   page有点问题要解决
        ///// </summary>
        //protected override void Query()
        //{

        //}

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<AlarmDealLogInfo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.PTMSLogManageService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    AlarmDealLogServiceClient client = InitialClient();
                    client.GetAlarmDealLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, Disposer, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), pagingInfo);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
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
                    AlarmDealLogServiceClient client = ServiceClientFactory.Create<AlarmDealLogServiceClient>();
                    client.GetAlarmDealLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                if (item.AlarmTime.HasValue)
                                {
                                    item.AlarmTime = item.AlarmTime.Value.ToLocalTime();
                                }
                                if (item.DealTime.HasValue)
                                {
                                    item.DealTime = item.DealTime.Value.ToLocalTime();
                                }
                            }
                            List<string> Codes = new List<string>();
                            Codes.Add("AlarmVihcleID");
                            Codes.Add("VehicleType");
                            Codes.Add("DealPerson");
                            Codes.Add("AlarmTime");
                            Codes.Add("DealTime");
                            Codes.Add("Description");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleId"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_VehicleType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DealPerson"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_AlarmTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DellTime"));
                            
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
                        client.GetAlarmDealLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, Disposer, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetAlarmDealLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, Disposer, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                }

            }
            catch (Exception ex)
            {
                setExportBtnStatus(true);
            }
        }


        #endregion


    }
}
