using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Installation.Views;
using Gsafety.PTMS.ServiceReference.MaintainService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.MaintainRecordUnfinishedManageVm)]
    public class MaintainRecordUnfinishedManageViewModel : ListViewModel<MaintainRecordUnfinished>
    {
        /// <summary>
        /// 初始化内容
        /// </summary>
        public MaintainRecordUnfinishedManageViewModel()
            : base()
        {
            try
            {
                this.SelectedItem = new MaintainRecordUnfinished();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MaintainRecordUnfinishedMangeViewModel()", ex);
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            try
            {
                if (SelectedItem != null)
                {
                    //SelectedItem.ZInstallStation = ZInstallStation;
                    MaintainRecordUnfinishedDetailWindow detailWindow = new MaintainRecordUnfinishedDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name }, { "model", SelectedItem } });
                    detailWindow.Closed += detailWindow_Closed;
                    detailWindow.Show();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        protected override void Add(string name)
        {
            MaintainRecordUnfinishedDetailWindow detailWindow = new MaintainRecordUnfinishedDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name }, { "model", SelectedItem } });
            detailWindow.Closed += detailWindow_Closed;
            detailWindow.Show();
        }

        protected override void ViewDetail(string name)
        {
            MaintainRecordUnfinishedDetailWindow detailWindow = new MaintainRecordUnfinishedDetailWindow(string.Empty, new Dictionary<string, object>() { { "action", name }, { "model", SelectedItem } });
            detailWindow.Show();
        }

        void detailWindow_Closed(object sender, EventArgs e)
        {
            MaintainRecordUnfinishedDetailWindow window = sender as MaintainRecordUnfinishedDetailWindow;
            if (window != null)
            {
                if (window.DialogResult == true)
                {
                    Data.RefreshPage();
                }
            }
        }

        private MaintainRecordUnfinished selecteditem;
        public MaintainRecordUnfinished SelectedItem
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

        protected override void Delete()
        {
            if (SelectedItem != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    MaintainRecordClient client = InitialClient();
                    //client.DeleteMaintainApplicationByIDAsync(SelectedItem.ID);
                }
            }
        }

        private MaintainRecordClient InitialClient()
        {
            MaintainRecordClient _client = ServiceClientFactory.Create<MaintainRecordClient>();
            _client.GetMaintainRecordUnfinishedListCompleted += _client_GetMaintainRecordUnfinishedListCompleted;
            return _client;
        }

        private void _client_GetMaintainRecordUnfinishedListCompleted(object sender, GetMaintainRecordUnfinishedListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {

                        List<MaintainRecordUnfinished> items = new List<MaintainRecordUnfinished>();
                        foreach (var item in e.Result.Result)
                        {
                            int state = (int)item.ApplicationStatus;
                            switch (state)
                            {
                                case 2:
                                    item.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("HasOrder");
                                    items.Add(item);
                                    break;
                                case 3:
                                    item.ShowStatus = ApplicationContext.Instance.StringResourceReader.GetString("Fixing");
                                    items.Add(item);
                                    break;

                            }

                            item.CreateTime = item.CreateTime.ToLocalTime();
                            if (item.ScheduleDate.HasValue)
                                item.ScheduleDate = item.ScheduleDate.Value.ToLocalTime();
                            if (item.StartTime.HasValue)
                                item.StartTime = item.StartTime.Value.ToLocalTime();
                            if (item.EndTime.HasValue)
                            {
                                item.EndTime = item.EndTime.Value.ToLocalTime();
                            }
                        }
                        Data.loader_Finished(new PagedResult<MaintainRecordUnfinished>
                        {
                            Count = items.Count,
                            Items = items,
                            PageIndex = currentIndex
                        });

                        if (items.Any())
                        {
                            SelectedItem = items[0];
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
                Data = new PagedServerCollection<MaintainRecordUnfinished>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    MaintainRecordClient client = InitialClient();
                    //查询数据
                    client.GetMaintainRecordUnfinishedListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByVehicleID, SearchByContact, SearchByWorker, pageIndex, pageSize);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagination()", ex);
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

        private string searchByVehicleID;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByVehicleID
        {
            get
            {
                return searchByVehicleID;
            }
            set
            {
                this.searchByVehicleID = value;

                RaisePropertyChanged(() => this.SearchByVehicleID);
            }
        }

        private string searchByContact;
        /// <summary>
        /// 
        /// </summary>
        public string SearchByContact
        {
            get
            {
                return searchByContact;
            }
            set
            {
                this.searchByContact = value;

                RaisePropertyChanged(() => this.SearchByContact);
            }
        }

        #endregion
    }
}

