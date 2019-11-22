using BaseLib.ViewModels;
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
    [ExportAsViewModel(ManagerName.VideoPlayLogVm)]
    public class VideoPlayViewModel : SystemLogBaseViewModel<LogVideo>
    {
        public VideoPlayViewModel()
        {
        }

        private LogVideoClient InitialClient()
        {
            LogVideoClient client = ServiceClientFactory.Create<LogVideoClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetVideoPlayLogListCompleted += client_GetVideoPlayLogListCompleted;
            client.ClearVideoPlayLogCompleted += client_ClearVideoPlayLogCompleted;
            return client;
        }

        void client_ClearVideoPlayLogCompleted(object sender, ClearVideoPlayLogCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("client_ClearVideoPlayLogCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        void client_GetVideoPlayLogListCompleted(object sender, GetVideoPlayLogListCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("client_GetVideoPlayLogListCompleted", ex);
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
                        log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("ClearVideoPlayLog");
                        log.OperateTime = DateTime.Now.ToUniversalTime();
                        log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        LogVideoClient client = InitialClient();
                        client.ClearVideoPlayLogAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, log);
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
                    client.GetVideoPlayLogListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, null, MdvrCoreSn, BeginTime.Value.ToUniversalTime(), EndTime.Value.AddDays(1).ToUniversalTime(), pageIndex, pageSize);

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
                    LogVideoClient client = ServiceClientFactory.Create<LogVideoClient>();
                    client.GetVideoPlayLogListCompleted += (s, e) =>
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
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_Player"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_PlayTime"));
                            Names.Add(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MdvrCoreId"));
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
                        client.GetVideoPlayLogListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, null, MdvrCoreSn, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), pagingInfo.PageIndex, pagingInfo.PageSize);
                    }
                    else
                    {
                        PagingInfo pagingInfo = new PagingInfo() { PageIndex = 1, PageSize = Data.TotalItemCount };
                        client.GetVideoPlayLogListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, VehicleID, null, MdvrCoreSn, BeginTime.Value.ToUniversalTime(), EndTime.Value.ToUniversalTime(), pagingInfo.PageIndex, pagingInfo.PageSize);
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
