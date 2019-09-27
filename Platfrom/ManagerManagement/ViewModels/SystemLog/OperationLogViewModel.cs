using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.Common.Converts;
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
    [ExportAsViewModel(ManagerName.OperateLogVm)]
    public class OperationLogViewModel : SystemLogBaseViewModel<LogOperate>
    {
        public OperationLogViewModel()
        {
            this.SearchByName = "";
        }

        private LogOperateClient InitialClient()
        {
            LogOperateClient client = ServiceClientFactory.Create<LogOperateClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetOperationLogListCompleted += client_GetOperationLogListCompleted;
            client.ClearOperateLogCompleted += client_ClearOperateLogCompleted;
            return client;
        }

        void client_ClearOperateLogCompleted(object sender, ClearOperateLogCompletedEventArgs e)
        {
            try
            {
                try
                {
                    if (e.Error == null)
                    {
                        if (e.Result.IsSuccess)
                        {
                            Query();
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
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_ClearOperateLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void client_GetOperationLogListCompleted(object sender, GetOperationLogListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.OperateTime = item.OperateTime.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<LogOperate>()
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
                ApplicationContext.Instance.Logger.LogException("client_GetOperationLogListCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            LogOperateClient client = sender as LogOperateClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        protected override void Delete()
        {
            var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
            dialogResult.Closed += dialogResult_Closed;
        }

        void window_Closed(object sender, EventArgs e)
        {
            this.Query();
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            try
            {
                SelfMessageBox dialog = sender as SelfMessageBox;
                if (dialog != null)
                {
                    if (dialog.DialogResult == true)
                    {
                        LogOperate log = new LogOperate();
                        log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                        log.ID = Guid.NewGuid().ToString();
                        log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("ClearOperateLog");
                        log.OperateTime = DateTime.Now.ToUniversalTime();
                        log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        LogOperateClient client = InitialClient();
                        client.ClearOperateLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, log);

                    }
                }
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
                Data = new BaseLib.Model.PagedServerCollection<LogOperate>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    LogOperateClient client = InitialClient();
                    client.GetOperationLogListAsync(this.SearchByName, ApplicationContext.Instance.AuthenticationInfo.ClientID, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pageIndex, pageSize);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        #region do completed...


        #endregion
        private string searchByName;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByName
        {
            get
            {
                return searchByName;
            }
            set
            {
                this.searchByName = value;
                RaisePropertyChanged(() => this.SearchByName);
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
                    LogOperateClient client = ServiceClientFactory.Create<LogOperateClient>();
                    client.GetOperationLogListCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            OperateTypeConveter typeconveter = new OperateTypeConveter();
                            DateTimeConverFormate timeconverter = new DateTimeConverFormate();
                            foreach (var item in e.Result.Result)
                            {
                                item.OperateTime = item.OperateTime.ToLocalTime();
                                item.OperateTypeExt = typeconveter.Convert(item.OperateType, null, null, null).ToString();
                                item.OperateTimeExt = timeconverter.Convert(item.OperateTime, null, null, null).ToString();
                            }
                            List<string> Codes = new List<string>();
                            Codes.Add("LoginName");
                            Codes.Add("ShowRoleName");
                            Codes.Add("OperatorName");
                            Codes.Add("OperateTypeExt");
                            Codes.Add("OperateTimeExt");
                            Codes.Add("OperateContent");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MAINPAGE_UserName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Info_Operator"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("Info_OperateType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MAINTAIN_OperationTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_Content"));
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
                        client.GetOperationLogListAsync(this.SearchByName, ApplicationContext.Instance.AuthenticationInfo.ClientID, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo.PageIndex, pagingInfo.PageSize);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetOperationLogListAsync(this.SearchByName, ApplicationContext.Instance.AuthenticationInfo.ClientID, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo.PageIndex, pagingInfo.PageSize);
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
