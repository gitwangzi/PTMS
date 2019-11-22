using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.LogService;
using Gsafety.PTMS.Share;
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

namespace Gsafety.PTMS.Manager.ViewModels.SystemLog
{
    [ExportAsViewModel(ManagerName.VideoLogVM)]
    public class LogDataViewModel : ListViewModel<LogData>
    {
        LogDataClient client = null;

        public LogDataViewModel()
            : base()
        {
            try
            {
                client = ServiceClientFactory.Create<LogDataClient>();
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

                client.GetLogDataListCompleted += client_GetLogDataListCompleted;
                client.GetLogDataListByConditionCompleted += client_GetLogDataListByConditionCompleted;
                client.DeleteLogDataByIDCompleted += client_DeleteLogDataByIDCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("LogDataMangeViewModel()", ex);
            }
        
        }

        private void client_DeleteLogDataByIDCompleted(object sender, DeleteLogDataByIDCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void client_GetLogDataListByConditionCompleted(object sender, GetLogDataListByConditionCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    Data.loader_Finished(new BaseLib.Model.PagedResult<LogData>()
                    {
                        Count = e.Result.TotalRecord,
                        Items = e.Result.Result,//数据列表
                        PageIndex = currentIndex
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void client_GetLogDataListCompleted(object sender, GetLogDataListCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null)
                {
                    Data.loader_Finished(new BaseLib.Model.PagedResult<LogData>()
                    {
                        Count = e.Result.TotalRecord,
                        Items = e.Result.Result,//数据列表
                        PageIndex = currentIndex
                    });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            //EventAggregator.Publish(
            //   new ViewNavigationArgs(BaseInformationName.DevGpsDetailViewV, new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } })
            //   );
          
        }
        protected override void Add(string name)
        {
            //EventAggregator.Publish(
            //   new ViewNavigationArgs(BaseInformationName.DevGpsDetailViewV, new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } })
            //   );
          
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
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    client.DeleteLogDataByIDAsync(CurrentLogData.ID);
                }
            }
        }

        protected override void ViewDetail(string name)
        {
            //EventAggregator.Publish(
            //   new ViewNavigationArgs(BaseInformationName.DevGpsDetailViewV, new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } })
            //   );
            //DevGpsDetailWindow window = new DevGpsDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentDevGps } });
            //window.Closed += window_Closed;
            //window.Show();
        }

        /// <summary>
        /// 查询   page有点问题要解决
        /// </summary>
        protected override void Query()
        {
            if ((SearchByName == null || SearchByName == "")&&(BeginTime==null))
            {
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
                PagingInfo page = new PagingInfo();
                page.PageIndex = currentIndex;
                page.PageSize = PageSizeValue;
                client.GetLogDataListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID);

            }
            else
            {
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
                PagingInfo page = new PagingInfo();
                page.PageIndex = currentIndex;
                page.PageSize = PageSizeValue;
                if ((SearchByName!=null&&SearchByName!="")||(BeginTime != null && EndTime != null))
                    client.GetLogDataListByConditionAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName, BeginTime, EndTime);

            }
        }


        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                client = ServiceClientFactory.Create<LogDataClient>();
                ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
                Data = new BaseLib.Model.PagedServerCollection<LogData>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.VehicleService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = PageSizeValue;
                    client.GetLogDataListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID);

                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        #region do completed...

       
        #endregion




        private LogData currentLogData;

        public LogData CurrentLogData
        {
            get { return currentLogData; }
            set
            {
                currentLogData = value;
                RaisePropertyChanged(() => this.CurrentLogData);
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
        private DateTime? beginTime;
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
                RaisePropertyChanged(() => this.BeginTime);
            }
        }
        private DateTime? endTime;
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
                RaisePropertyChanged(() => this.EndTime);
            }
        }

    }
}
