using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.PublicService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using PublicServiceManagement;
using PublicServiceManagement.Views;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
namespace Gsafety.PTMS.PublicServiceManagement.Views.ViewModels
{
    [ExportAsViewModel(PublicServiceName.FoundRegistryManageVm)]
    public class FoundRegistryManageViewModel : ListViewModel<FoundRegistry>
    {

       // FoundRegistryClient client = null;
        /// <summary>
        /// 初始化内容
        /// </summary>
        public FoundRegistryManageViewModel()
            : base()
        {
            try
            {

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("FoundRegistryMangeViewModel()", ex);
            }
        }

        private FoundRegistryClient InitServiceClient()
        {
            FoundRegistryClient client = ServiceClientFactory.Create<FoundRegistryClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

          
            client.GetFoundRegistryByConditionListCompleted += client_GetFoundRegistryByConditionListCompleted;
            client.DeleteFoundRegistryByIDCompleted += client_DeleteFoundRegistryByIDCompleted;
            return client;
        }

        private void client_DeleteFoundRegistryByIDCompleted(object sender, DeleteFoundRegistryByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (!string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ErrorMsg);
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                                e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        Data.RefreshPage();
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("client_DeleteInstallStationCompleted", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private void client_GetFoundRegistryByConditionListCompleted(object sender, GetFoundRegistryByConditionListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.CreateTime = item.CreateTime.ToLocalTime();
                            item.FoundTime = item.FoundTime.ToLocalTime();
                            if (item.ClaimTime.HasValue)
                                item.ClaimTime = item.ClaimTime.Value.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<FoundRegistry>()
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
                ApplicationContext.Instance.Logger.LogException("FoundRegistryManageViewModel", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            FoundRegistryClient client = sender as FoundRegistryClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            FoundRegistryDetailWindow window = new FoundRegistryDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentFoundRegistry } });
            window.Closed += window_Closed;
            window.Show();
        }
        protected override void Add(string name)
        {
            FoundRegistryDetailWindow window = new FoundRegistryDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentFoundRegistry } });
            window.Closed += window_Closed;
            window.Show();
        }

        protected override void Delete()
        {
            var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
            dialogResult.Closed += dialogResult_Closed;
        }

        private void dialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    FoundRegistryClient client = InitServiceClient();
                    client.DeleteFoundRegistryByIDAsync(CurrentFoundRegistry.ID);
                }
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }


        protected override void ViewDetail(string name)
        {
            FoundRegistryDetailWindow window = new FoundRegistryDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentFoundRegistry } });
   
            window.Show();
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
                Data = new PagedServerCollection<FoundRegistry>((pageIndex, pageSize) =>
                {
                    FoundRegistryClient client= InitServiceClient();
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = PageSizeValue;
                    client.GetFoundRegistryByConditionListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByFounder, SearchByKeyword, SearchByLostName);
                   // client.GetFoundRegistryListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID);

                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private FoundRegistry currentFoundRegistry;
        public FoundRegistry CurrentFoundRegistry
        {
            get { return currentFoundRegistry; }
            set
            {
                currentFoundRegistry = value;
                RaisePropertyChanged(() => this.CurrentFoundRegistry);
            }
        }

        private string searchByFounder;
        public string SearchByFounder
        {
            get
            {
                return searchByFounder;
            }
            set
            {
                this.searchByFounder = value;
                Validate(ExtractPropertyName(() => SearchByFounder), searchByFounder, SearchByFounder);
                RaisePropertyChanged(() => this.SearchByFounder);
            }
        }

        private string searchByKeyword;
        public string SearchByKeyword
        {
            get
            {
                return searchByKeyword;
            }
            set
            {
                this.searchByKeyword = value;
                Validate(ExtractPropertyName(() => SearchByKeyword), searchByKeyword, SearchByKeyword);
                RaisePropertyChanged(() => this.SearchByKeyword);
            }
        }

        private string searchByLostName;
        public string SearchByLostName
        {
            get
            {
                return searchByLostName;
            }
            set
            {
                this.searchByLostName = value;
                Validate(ExtractPropertyName(() => SearchByLostName), searchByLostName, SearchByLostName);
                RaisePropertyChanged(() => this.SearchByLostName);
            }
        }

        private void Validate(string prop, string value, string valid)
        {
            ClearErrors(prop);
            if (valid.Length > 20)
            {
                base.SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString(PTMSBaseViewModel.wrongformat));
            }
        }

      

    }
}

