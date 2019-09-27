using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.LogService;
using Gsafety.PTMS.ServiceReference.OrderClientService;
using Gsafety.PTMS.ServiceReference.PTMSLogManageService;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.SuperPowerManagement;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.ViewModels
{
    [ExportAsViewModel(SuperPowerName.ManageLogVm)]
    public class ManageLogViewModel : ListViewModel<Gsafety.PTMS.ServiceReference.LogService.LogManager>
    {
        public ManageLogViewModel()
        {
            this.SearchByName = "";
        }

        private LogManagerClient InitialClient()
        {
            LogManagerClient client = ServiceClientFactory.Create<LogManagerClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetLogManagerListByConditionCompleted += client_GetLogManagerListByConditionCompleted;
            return client;
        }

        void client_GetLogManagerListByConditionCompleted(object sender, GetLogManagerListByConditionCompletedEventArgs e)
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
                            item.Content = item.Content + " " + item.ClientName;
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<Gsafety.PTMS.ServiceReference.LogService.LogManager>()
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
            LogManagerClient client = sender as LogManagerClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            this.Query();
        }


        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<Gsafety.PTMS.ServiceReference.LogService.LogManager>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    Gsafety.PTMS.ServiceReference.LogService.PagingInfo pagingInfo = new Gsafety.PTMS.ServiceReference.LogService.PagingInfo() { PageIndex = pageIndex, PageSize = pageSize };
                    LogManagerClient client = InitialClient();
                    client.GetLogManagerListByConditionAsync(pagingInfo, this.SearchByName, BeginTime.Value.Date.ToUniversalTime(), EndTime.Value.Date.AddDays(1).ToUniversalTime());
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
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

        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }
    }
}
