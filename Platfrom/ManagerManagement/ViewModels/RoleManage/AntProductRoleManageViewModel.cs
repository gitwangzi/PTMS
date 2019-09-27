using BaseLib.ViewModels;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using BaseLib.SystemFunction;
using Gsafety.PTMS.Bases.Enums;
using System.Reflection;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Manager.Views.RoleManage;

namespace Gsafety.PTMS.Manager.ViewModels.RoleManage
{
    [ExportAsViewModel(ManagerName.AntProductRoleManageVm)]
    public class AntProductRoleManageViewModel : ListViewModel<Role>
    {
        public ICommand BtnPermissionCommand { get; set; }

        public AntProductRoleManageViewModel()
            : base()
        {
            try
            {
                BtnPermissionCommand = new ActionCommand<string>(r => Permission(CurrentModel.ID, (RoleCategory)CurrentModel.RoleCategory, CurrentModel.Editable ? "Update" : "View"));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("RoleMangeViewModel()", ex);
            }
        }

        private RoleServiceClient InitServiceClient()
        {
            RoleServiceClient client = ServiceClientFactory.Create<RoleServiceClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);

            client.GetRoleListCompleted += client_GetRoleListCompleted;
            client.DeleteRoleByIDCompleted += client_DeleteRoleByIDCompleted;
            return client;
        }

        private void Permission(string roleID, RoleCategory roleCategory, string mode)
        {
            RoleSelectView view = new RoleSelectView(string.Empty, new System.Collections.Generic.Dictionary<string, object>() { { "roleID", roleID }, { "roleCategory", roleCategory }, { "Mode", mode } });
            view.Show();
        }

        #region Delete

        protected override void Delete()
        {
            if (CurrentModel != null)
            {
                var dialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                dialogResult.Closed += dialogResult_Closed;
            }
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
                        log.OperateTime = DateTime.Now.ToUniversalTime();
                        log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
                        log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
                        log.OperateContent = ApplicationContext.Instance.StringResourceReader.GetString("DeleteRole") + ":" + CurrentModel.Name;
                        RoleServiceClient client = InitServiceClient();
                        client.DeleteRoleByIDAsync(CurrentModel.ID, log);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        void client_DeleteRoleByIDCompleted(object sender, DeleteRoleByIDCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException("OrderClientMangeViewModel()", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }
        #endregion

        protected override void ViewDetail(string actionName)
        {
            AntProductRoleManageDetailWindow window = new AntProductRoleManageDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", actionName }, { "model", CurrentModel } });
            window.Show();
        }

        protected override void Update(string name)
        {
            AntProductRoleManageDetailWindow window = new AntProductRoleManageDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentModel } });
            window.Closed += window_Closed;
            window.Show();
        }

        protected override void Add(string name)
        {
            AntProductRoleManageDetailWindow window = new AntProductRoleManageDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { { "action", name }, { "model", CurrentModel } });
            window.Closed += window_Closed;
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }

        #region Query
        /// <summary>
        /// 查询
        /// </summary>
        protected override void Query()
        {
            currentIndex = 1;
            Data.MoveToFirstPage();
        }

        void client_GetRoleListCompleted(object sender, GetRoleListCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled == true)
                {
                    return;
                }

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
                    foreach (var item in result.Result)
                    {
                        item.Editable = !item.IsPredefined;
                        item.CreateTime = item.CreateTime.ToLocalTime();
                    }

                    Data.loader_Finished(new BaseLib.Model.PagedResult<Role>
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
                CloseClient(sender);
            }
        }

        private static void CloseClient(object sender)
        {
            RoleServiceClient client = sender as RoleServiceClient;
            if (client != null)
            {
                client.CloseAsync();
                client = null;
            }
        }
        #endregion

        /// <summary>
        /// 初始化分页数据
        /// </summary>
        protected override void InitPagination()
        {
            try
            {
                Data = new BaseLib.Model.PagedServerCollection<Role>((pageIndex, pageSize) =>
                {
                    pageSize = PageSizeValue;
                    System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                    RoleServiceClient client = InitServiceClient();
                    client.GetRoleListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchInfo, pageIndex, pageSize);
                });
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        #region Property

        public Role CurrentModel { get; set; }

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

        #endregion
    }
}
