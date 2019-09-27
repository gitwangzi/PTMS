using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation.Views;
using Gsafety.PTMS.ServiceReference.MaintainService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.MaintainRecordManageVm)]
    public class MaintainRecordManageViewModel : ListViewModel<MaintainRecord>
    {
        /// <summary>
        /// 初始化内容
        /// </summary>
        public MaintainRecordManageViewModel()
            : base()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MaintainRecordMangeViewModel()", ex);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {

        }

        protected override void Add(string name)
        {

        }

        protected override void ViewDetail(string name)
        {
            MaintainRecordDetailWindow detailWindow = new MaintainRecordDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name }, { "view", SelectedItem } });
            //detailWindow.Closed += detailWindow_Closed;
            detailWindow.Show();
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }

        private MaintainRecordClient InitialClient()
        {
            MaintainRecordClient _client = ServiceClientFactory.Create<MaintainRecordClient>();
            _client.GetMaintainRecordListCompleted += _client_GetMaintainRecordListCompleted;
            return _client;
        }

        private MaintainRecord selecteditem;
        public MaintainRecord SelectedItem
        {
            get { return selecteditem; }
            set
            {
                if (selecteditem != value)
                {
                    selecteditem = value;
                }

                RaisePropertyChanged(() => this.SelectedItem);
            }
        }

        private void _client_GetMaintainRecordListCompleted(object sender, GetMaintainRecordListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        List<MaintainRecord> items = new List<MaintainRecord>();
                        foreach (var item in e.Result.Result)
                        {

                            item.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("HasFinished");
                            items.Add(item);

                            item.CreateTime = item.CreateTime.ToLocalTime();
                            item.StartTime = item.StartTime.ToLocalTime();

                            if (item.EndTime.HasValue)
                            {
                                item.EndTime = item.EndTime.Value.ToLocalTime();
                            }

                            item.ScheduleDate = item.ScheduleDate.ToLocalTime();
                        }
                        Data.loader_Finished(new PagedResult<MaintainRecord>
                        {
                            Count = items.Count,
                            Items = items,
                            PageIndex = currentIndex
                        });

                        if (e.Result.Result.Count != 0)
                        {
                            SelectedItem = e.Result.Result[0];
                        }
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
                ApplicationContext.Instance.Logger.LogException("_client_GetMaintainApplicationListCompleted()", ex);
            }
            finally
            {
                MaintainRecordClient client = sender as MaintainRecordClient;
                CloseClient(client);
            }
        }

        private void CloseClient(MaintainRecordClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }
        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                try
                {
                    Data = new PagedServerCollection<MaintainRecord>((pageIndex, pageSize) =>
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                        MaintainRecordClient client = InitialClient();
                        //查询数据
                        DateTime? tempStartTime = null;
                        if (SearchByStartTime.HasValue)
                        {
                            if (SearchByStartTime.Value > DateTime.Now.Date.AddDays(1))
                            {
                                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("StartTimeError"));
                                return;
                            }
                            tempStartTime = SearchByStartTime.Value.ToUniversalTime();
                        }

                        client.GetMaintainRecordListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByWorker, tempStartTime, SearchByEndTime, pageIndex, pageSize);
                        //client.GetMaintainRecordUnfinishedListAsync(SearchByVehicleID, SearchByContact, SearchByWorker, pageIndex, pageSize);
                    });
                }
                catch (Exception ex)
                {
                    ApplicationContext.Instance.Logger.LogException("InitPagination()", ex);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }


        #region searchby....
        private string searchByWorker;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByWorker
        {
            get
            {
                return searchByWorker;
            }
            set
            {
                this.searchByWorker = value;
                RaisePropertyChanged(() => this.SearchByWorker);
            }
        }

        private DateTime? searchByStartTime;

        public DateTime? SearchByStartTime
        {
            get
            {
                return searchByStartTime;
            }
            set
            {
                this.searchByStartTime = value;
            
                RaisePropertyChanged(() => this.SearchByStartTime);
            }
        }

        private DateTime? searchByEndTime;

        public DateTime? SearchByEndTime
        {
            get
            {
                return searchByEndTime;
            }
            set
            {
                this.searchByEndTime = value;
                RaisePropertyChanged(() => this.SearchByEndTime);
            }
        }

        //private void ValidateScheduleDate(string prop, string value)
        //{
        //    ClearErrors(prop);
        //    if (SearchByStartTime!=null)
        //    {
        //        DateTime dateValue;
        //        if (!DateTime.TryParse(SearchByStartTime.ToString(), out dateValue))
        //            base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
        //    }
        //}

        #endregion


    }
}

