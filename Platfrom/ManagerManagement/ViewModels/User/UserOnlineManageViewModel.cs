using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Manager;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
namespace PTMS0628Management.ViewModels
{
    [ExportAsViewModel(ManagerName.UserOnlineInfoVm)]
    public class UserOnlineManageViewModel : ListViewModel<UserOnline>
    {
        UserOnlineClient client = null;
        /// <summary>
        /// 初始化内容
        /// </summary>
        public UserOnlineManageViewModel()
            : base()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UserOnlineMangeViewModel()", ex);
            }
        }

        private void InitialClient()
        {
            client = ServiceClientFactory.Create<UserOnlineClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetUserOnlineListCompleted += client_GetUserOnlineListCompleted;
        }

        private void client_GetUserOnlineListCompleted(object sender, GetUserOnlineListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.OnlineTime = item.OnlineTime.ToLocalTime();
                            if (item.OnlineTime < DateTime.Now)
                            {
                                TimeSpan timespan = DateTime.Now.Subtract(item.OnlineTime);

                                timespan = TimeSpan.FromSeconds((int)timespan.TotalSeconds);
                                //item.OnlineTimeSpan = timespan.ToString();
                                //var totalSecounds = (int)timespan.TotalSeconds;
                                var day = timespan.Days;
                                var hour = timespan.Hours + day * 24;
                                var mi = timespan.Minutes;
                                string hourFormat = "00";
                                string miFormat = "00";
                                string secondformat = "00";
                                if (hour < 10)
                                {
                                    hourFormat = "0" + hour.ToString();
                                }
                                else
                                {
                                    hourFormat = hour.ToString();
                                }
                                if (mi < 10)
                                {
                                    miFormat = "0" + mi.ToString();
                                }
                                else
                                {
                                    miFormat = mi.ToString();
                                }

                                if (timespan.Seconds < 10)
                                {
                                    secondformat = "0" + timespan.Seconds.ToString();
                                }
                                else
                                {
                                    secondformat = timespan.Seconds.ToString();
                                }
                                item.OnlineTimeSpan = hourFormat + ":" + miFormat + ":" + secondformat;
                            }
                            else
                            {
                                item.OnlineTimeSpan = "00:00:00";
                            }
                        }

                        Data.loader_Finished(new BaseLib.Model.PagedResult<UserOnline>()
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
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
            finally
            {
                CloseClient();
            }

        }

        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }
        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {

            try
            {
                Data = new BaseLib.Model.PagedServerCollection<UserOnline>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    if (client == null)
                    {
                        InitialClient();
                    }

                    client.GetUserOnlineListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName, pageIndex, pageSize);

                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private void CloseClient()
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
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


    }
}

