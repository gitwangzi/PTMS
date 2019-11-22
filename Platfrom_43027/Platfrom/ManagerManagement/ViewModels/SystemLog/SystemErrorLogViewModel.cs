using Jounce.Core.ViewModel;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using System.Reflection;
using Gsafety.Common.Controls;
using System.Collections.Generic;
using Gsafety.PTMS.Spreadsheet;
using System.Linq;

namespace Gsafety.PTMS.Manager.ViewModels.SystemLog
{
    [ExportAsViewModel(ManagerName.ErrorLogVM)]
    public class SystemErrorLogViewModel
        : SystemLogBaseViewModel<LogError>
    {
        private string errorReason;
        /// <summary>
        /// 
        /// </summary>
        public string ErrorReason
        {
            get
            {
                return errorReason;
            }
            set
            {
                this.errorReason = value;
                RaisePropertyChanged(() => this.ErrorReason);
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

        public SystemErrorLogViewModel()
        {
            this.ErrorReason = "";
        }

        private PTMSLogErrorClient InitialClient()
        {
            PTMSLogErrorClient client = ServiceClientFactory.Create<PTMSLogErrorClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetErrorLogCompleted += Client_GetErrorLogCompleted; 
            return client;
        }

        private void Client_GetErrorLogCompleted(object sender, GetErrorLogCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.CreateTime = item.CreateTime.ToLocalTime();                           
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<LogError>()
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
            PTMSLogErrorClient client = sender as PTMSLogErrorClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        ///// <summary>
        ///// 初始化分页数据
        ///// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<LogError>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    PTMSLogErrorClient client = InitialClient();
                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = PageSizeValue;
                    client.GetErrorLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, ErrorReason, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), page);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
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
                    PTMSLogErrorClient client = ServiceClientFactory.Create<PTMSLogErrorClient>();
                    client.GetErrorLogCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                item.CreateTime = item.CreateTime.ToLocalTime();                              
                            }
                            List<string> Codes = new List<string>();
                            Codes.Add("ErrorReason");
                            Codes.Add("CreateTime");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("WrongContent"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("CreateTime"));
      
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
                        client.GetErrorLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, ErrorReason, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetErrorLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, ErrorReason, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo);
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

