using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BasicPage.Views;
using Gsafety.PTMS.Manager.Views.Organization;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.Command;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Microsoft.Windows;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{
    [ExportAsViewModel(ManagerName.AntProductUserManageVm)]
    public class AntProductUserManageViewModel : ListViewModel<GUser>
    {
        private string _confirmDeleteUser = ApplicationContext.Instance.StringResourceReader.GetString("SureDeleUser");
        private string _confirmResetPwd = ApplicationContext.Instance.StringResourceReader.GetString("SureSetKey");
        private string _deleteUserResult = ApplicationContext.Instance.StringResourceReader.GetString("DeleteSuccess");
        private string _userManager = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_UserManger");
        private string _currentUserDepartmentID;

        //private UserServiceClient client;
        public IActionCommand BtnResetPasswordCommand { get; set; }
        public IActionCommand BtnOrganizationRight { get; set; }

        public AntProductUserManageViewModel()
        {
            try
            {
                BtnResetPasswordCommand = new ActionCommand<object>(obj => ResetPassword());
                BtnOrganizationRight = new ActionCommand<object>(obj => OrganizationRight());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private UserServiceClient InitClient()
        {
            UserServiceClient client = ServiceClientFactory.Create<UserServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetGUserListCompleted += client_GetGUserListCompleted;
            client.DeleteGUserCompleted += client_DeleteGUserCompleted;
            return client;
        }

        // 添加车辆组织机构
        private void OrganizationRight()
        {
            OrganizationSelectionWindow window = new OrganizationSelectionWindow(CurrentModel.ID);
            window.Show();
        }

        private async void ResetPassword()
        {
            try
            {
                var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsResert), MessageDialogButton.OkAndCancel);
                var result = await dialogResult;

                if (result == MessageDialogResult.OK)
                {
                    new ResetPasswordView(CurrentModel.UserName, CurrentModel.ID, CurrentModel.Account).Show();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected async override void Delete()
        {
            var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
            var result = await dialogResult;

            if (result == MessageDialogResult.OK)
            {
                LogOperate log = new LogOperate();
                log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                log.ID = Guid.NewGuid().ToString();
                log.OperateTime = DateTime.Now.ToUniversalTime();
                log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
                log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("DeleteUser") + ":" + CurrentModel.Account;
                UserServiceClient client = InitClient();
                client.DeleteGUserAsync(CurrentModel.ID, log);
            }
        }

        void client_DeleteGUserCompleted(object sender, DeleteGUserCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }

        }

        protected override void ViewDetail(string actionName)
        {
            var window = new AntProductUserDetailWindow("", new Dictionary<string, object>() { { "action", actionName }, { "model", CurrentModel }, { "userDepartmentID", _currentUserDepartmentID } });
            window.Show();
        }

        protected override void Update(string name)
        {
            var window = new AntProductUserDetailWindow("", new Dictionary<string, object>() { { "action", name }, { "model", CurrentModel }, { "userDepartmentID", _currentUserDepartmentID } });
            window.Show();
            window.Closed += window_Closed;
        }

        protected override void Add(string name)
        {
            var window = new AntProductUserDetailWindow("", new Dictionary<string, object>() { { "action", name }, { "userDepartmentID", _currentUserDepartmentID } });
            window.Show();
            window.Closed += window_Closed;
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

        void client_GetGUserListCompleted(object sender, GetGUserListCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    var dialog = MessageBoxHelper.ShowDialog(ServerError);
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        var dialog = MessageBoxHelper.ShowDialog(result.ErrorMsg);
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }

                    if (result.ExceptionMessage != null)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), result.ExceptionMessage);
                    }
                }
                else
                {
                    Data.loader_Finished(new BaseLib.Model.PagedResult<GUser>
                    {
                        Count = result.TotalRecord,
                        Items = result.Result,
                        PageIndex = currentIndex
                    });
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                UserServiceClient client = sender as UserServiceClient;
                if (client != null)
                {
                    client.CloseAsync();
                    client = null;
                }
            }
        }

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<GUser>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    UserServiceClient client = InitClient();
                    client.GetGUserListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, _currentUserDepartmentID, SearchInfo, pageIndex, pageSize);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                _currentUserDepartmentID = (string)viewParameters["userDepartmentID"];

                base.ActivateView(viewName, viewParameters);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private GUser _currentModel;
        public GUser CurrentModel
        {
            get { return _currentModel; }
            set
            {
                _currentModel = value;
                RaisePropertyChanged(() => CurrentModel);
            }
        }

        private string _searchInfo = string.Empty;
        public string SearchInfo
        {
            get { return _searchInfo; }
            set
            {
                if (_searchInfo != value)
                {
                    _searchInfo = value;
                    RaisePropertyChanged(() => SearchInfo);
                }
            }
        }
    }
}
