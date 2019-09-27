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
    [ExportAsViewModel(PublicServiceName.LostRegistryManageVm)]
    public class LostRegistryManageViewModel : ListViewModel<LostRegistry>
    {
       // LostRegistryClient client = null;
        /// <summary>
        /// 初始化内容
        /// </summary>
        public LostRegistryManageViewModel()
            : base()
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("LostRegistryMangeViewModel()", ex);
            }
        }

        private LostRegistryClient InitServiceClient()
        {
            LostRegistryClient client = ServiceClientFactory.Create<LostRegistryClient>();
            client.DeleteLostRegistryByIDCompleted += client_DeleteLostRegistryByIDCompleted;
            client.GetLostRegistryListCompleted += client_GetLostRegistryListCompleted;
            client.GetLostRegistryByConditionListCompleted += client_GetLostRegistryByConditionListCompleted;
            return client;
        }

        private void client_GetLostRegistryByConditionListCompleted(object sender, GetLostRegistryByConditionListCompletedEventArgs e)
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
                            item.LostTime = item.LostTime.ToLocalTime();
                        }

                        Data.loader_Finished(new BaseLib.Model.PagedResult<LostRegistry>()
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
                ApplicationContext.Instance.Logger.LogException("LostRegistryManageViewModel", ex);
            }
            finally
            {
                LostRegistryClient client = sender as LostRegistryClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }

        }

        private void client_GetLostRegistryListCompleted(object sender, GetLostRegistryListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        foreach (var item in e.Result.Result)
                        {
                            item.LostTime = item.LostTime.ToLocalTime();
                            item.CreateTime = item.CreateTime.ToLocalTime();
                        }
                        Data.loader_Finished(new BaseLib.Model.PagedResult<LostRegistry>()
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
                ApplicationContext.Instance.Logger.LogException("LostRegistryManageViewModel", ex);
            }
            finally
            {
                LostRegistryClient client = sender as LostRegistryClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        private void client_DeleteLostRegistryByIDCompleted(object sender, DeleteLostRegistryByIDCompletedEventArgs e)
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
                        if (e.Result.Result)
                        {
                            Data.RefreshPage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("MdvrMsgManageViewModel", ex);
            }
            finally
            {
                LostRegistryClient client = sender as LostRegistryClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        protected override void Update(string name)
        {
            LostRegistryDetailWindow window = new LostRegistryDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentLostRegistry } });
            window.Closed += window_Closed;
            window.Show();
        }
        protected override void Add(string name)
        {
            LostRegistryDetailWindow window = new LostRegistryDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentLostRegistry } });
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
                    LostRegistryClient client = InitServiceClient();
                    client.DeleteLostRegistryByIDAsync(CurrentLostRegistry.ID);
                }
            }
        }

        protected override void ViewDetail(string name)
        {
            LostRegistryDetailWindow window = new LostRegistryDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentLostRegistry } });

            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
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
             
                Data = new BaseLib.Model.PagedServerCollection<LostRegistry>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    LostRegistryClient client = InitServiceClient();

                    PagingInfo page = new PagingInfo();
                    page.PageIndex = pageIndex;
                    page.PageSize = PageSizeValue;
                   // client.GetLostRegistryListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID);
                    client.GetLostRegistryByConditionListAsync(page, ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByLostName, SearchByKeyword, SearchByICard);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InitPagedServerCollection()", ex);
            }
        }

        private LostRegistry currentLostRegistry;

        public LostRegistry CurrentLostRegistry
        {
            get { return currentLostRegistry; }
            set
            {
                currentLostRegistry = value;
                RaisePropertyChanged(() => this.CurrentLostRegistry);
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

        private string searchByICard;
        public string SearchByICard
        {
            get
            {
                return searchByICard;
            }
            set
            {
                this.searchByICard = value;
                Validate(ExtractPropertyName(() => SearchByICard), searchByICard, SearchByICard);
                RaisePropertyChanged(() => this.SearchByICard);
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

        private string _dateFormate = ApplicationContext.Instance.StringResourceReader.GetString("DateFormate");
        public string DateFormate
        {
            get
            {
                return _dateFormate;
            }
            set
            {
                this._dateFormate = value;
                RaisePropertyChanged(() => this.DateFormate);
            }
        }

    }
}

