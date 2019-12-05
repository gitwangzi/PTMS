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
    [ExportAsViewModel(ManagerName.VideoDownloadLogVm)]
    public class VideoDownloadViewModel : SystemLogBaseViewModel<LogVideo>
    {
        public VideoDownloadViewModel()
        {
        }
        private LogVideoClient InitialClient()
        {
            LogVideoClient client = ServiceClientFactory.Create<LogVideoClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetVideoDownloadLogListCompleted += client_GetVideoDownloadLogListCompleted;
            client.ClearVideoDownloadLogCompleted += client_ClearVideoDownloadLogCompleted;
            return client;
        }

        void client_ClearVideoDownloadLogCompleted(object sender, ClearVideoDownloadLogCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
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
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_ClearVideoDownloadLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void client_GetVideoDownloadLogListCompleted(object sender, GetVideoDownloadLogListCompletedEventArgs e)
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
                        Data.loader_Finished(new BaseLib.Model.PagedResult<LogVideo>()
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
                ApplicationContext.Instance.Logger.LogException("client_GetVideoDownloadLogListCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            LogVideoClient client = sender as LogVideoClient;
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
                        log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("ClearVideoDownloadLog");
                        log.OperateTime = DateTime.Now.ToUniversalTime();
                        log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        LogVideoClient client = InitialClient();
                        client.ClearVideoDownloadLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, log);
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
                Data = new BaseLib.Model.PagedServerCollection<LogVideo>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    LogVideoClient client = InitialClient();
                    client.GetVideoDownloadLogListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, null, MdvrCoreSn, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), pageIndex, pageSize);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        #region do completed...


        #endregion
        private string vehicleid;
        /// <summary>
        /// 
        /// </summary>
        public string VehicleID
        {
            get
            {
                return vehicleid;
            }
            set
            {
                this.vehicleid = value;
                RaisePropertyChanged(() => this.VehicleID);
            }
        }

        private string mdvrcoresn;
        /// <summary>
        /// 
        /// </summary>
        public string MdvrCoreSn
        {
            get
            {
                return mdvrcoresn;
            }
            set
            {
                this.mdvrcoresn = value;
                RaisePropertyChanged(() => this.MdvrCoreSn);
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
                    LogVideoClient client = ServiceClientFactory.Create<LogVideoClient>();
                    client.GetVideoDownloadLogListCompleted += (s, e) =>
                    {
                        if (e.Result != null && e.Result.TotalRecord >= 0)
                        {
                            foreach (var item in e.Result.Result)
                            {
                                item.OperateTime = item.OperateTime.ToLocalTime();
                            }
                            List<string> Codes = new List<string>();
                            Codes.Add("OperatorName");
                            Codes.Add("OperateTime");
                            Codes.Add("MdvrCoreSn");
                            Codes.Add("SuiteSn");
                            Codes.Add("VehicleID");
                            Codes.Add("Channel");

                            List<string> Names = new List<string>();
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("DowmLoadPerson"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_DownLoadTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_MDVRID"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("ANTNumber"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_VehicleNumber"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ChannelId"));

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
                        client.GetVideoDownloadLogListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, null, MdvrCoreSn, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo.PageIndex, pagingInfo.PageSize);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetVideoDownloadLogListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, null, MdvrCoreSn, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pagingInfo.PageIndex, pagingInfo.PageSize);
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
