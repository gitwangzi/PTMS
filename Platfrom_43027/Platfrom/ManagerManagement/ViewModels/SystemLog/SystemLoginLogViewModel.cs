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

namespace Gsafety.PTMS.Manager.ViewModels.SystemLog
{
    [ExportAsViewModel(ManagerName.SystemLoginLogViewModel)]
    public class SystemLoginLogViewModel : SystemLogBaseViewModel<LogAccess>
    {
        public SystemLoginLogViewModel()
        {
            this.SearchByName = "";
        }

        private LoginLogServiceClient InitialClient()
        {
            LoginLogServiceClient client = ServiceClientFactory.Create<LoginLogServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetLoginLogCompleted += client_GetLoginLogCompleted;
            client.ClearLoginLogCompleted += client_ClearLoginLogCompleted;
            return client;
        }

        void client_ClearLoginLogCompleted(object sender, ClearLoginLogCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        LogQueryAction(null);
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
                ApplicationContext.Instance.Logger.LogException("client_ClearLoginLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void client_GetLoginLogCompleted(object sender, GetLoginLogCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.LoginTime = item.LoginTime.ToLocalTime();
                            if (item.LogoutTime.HasValue)
                            {
                                item.LogoutTime = item.LogoutTime.Value.ToLocalTime();
                            }
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<LogAccess>()
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
                ApplicationContext.Instance.Logger.LogException("client_GetLoginLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            LoginLogServiceClient client = sender as LoginLogServiceClient;
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
            LogQueryAction(null);
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    string content = ApplicationContext.Instance.StringResourceReader.GetString("ClearLoginLog");
                    LoginLogServiceClient client = InitialClient();
                    client.ClearLoginLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, ApplicationContext.Instance.AuthenticationInfo.UserID, ApplicationContext.Instance.AuthenticationInfo.Account, content);
                }
            }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<LogAccess>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    LoginLogServiceClient client = InitialClient();
                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = PageSizeValue;
                    client.GetLoginLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), page);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        #region do completed...


        #endregion
        private LogAccess currentLogAccess;

        public LogAccess CurrentLogAccess
        {
            get { return currentLogAccess; }
            set
            {
                currentLogAccess = value;
                RaisePropertyChanged(() => this.CurrentLogAccess);
            }
        }

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
                    LoginLogServiceClient client = ServiceClientFactory.Create<LoginLogServiceClient>();
                    client.GetLoginLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                item.LoginTime = item.LoginTime.ToLocalTime();
                                if (item.LogoutTime.HasValue)
                                {
                                    item.LogoutTime = item.LogoutTime.Value.ToLocalTime();
                                }
                            }
                            List<string> Codes = new List<string>();
                            Codes.Add("LoginUser");
                            Codes.Add("ShowRoleName");
                            Codes.Add("LoginTime");
                            Codes.Add("LogoutTime");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("User_UserName"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserType"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_LoginTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_LogoutTime"));

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
                        client.GetLoginLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetLoginLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
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
