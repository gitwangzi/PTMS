using BaseLib.ViewModels;
using Gsafety.PTMS.ServiceReference.AccountService;
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
using System.Collections.ObjectModel;
using Gsafety.PTMS.Share;
using System.Reflection;
using Jounce.Framework.Command;
using System.Linq;
using BaseLib.SystemFunction;
using Gsafety.PTMS.Bases.Enums;
using System.Collections.Generic;
using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Gsafety.Common.Controls;
using Gsafety.Common.CommMessage;
using System.Linq;
using System.Text;

namespace Gsafety.PTMS.Manager.ViewModels.RoleManage
{
    //[ExportAsViewModel(ManagerName.AntProductRoleSelectVm)]
    public class AntProductRoleSelectViewModel : PTMSBaseViewModel
    {
        // private RoleServiceClient roleClient;
        private string _currentRoleID;
        private RoleCategory _currentRoleCategory;
        public event EventHandler<SaveResultArgs> OnSaveResult;

        public Visibility EditVisibility { get; set; }
        public bool EditEnable { get; set; }

        private ObservableCollection<FunctionItem> _functionCheckList = new ObservableCollection<FunctionItem>();
        public ObservableCollection<FunctionItem> FunctionCheckList
        {
            get
            {

                return _functionCheckList;
            }
            set
            {
                _functionCheckList = value;
                RaisePropertyChanged(() => FunctionCheckList);
            }
        }

        public IActionCommand ReturnCommand { get; protected set; }
        public IActionCommand SaveCommand { get; protected set; }
        List<string> originalfunction = new List<string>();

        public AntProductRoleSelectViewModel()
        {
            try
            {
                //roleClient = ServiceClientFactory.Create<RoleServiceClient>();
                //ServiceClientFactory.CreateMessageHeader(roleClient.InnerChannel);

                //roleClient.GetFunItemsByRoleIDCompleted += roleClient_GetFunItemsByRoleIDCompleted;
                //roleClient.UpdateRoleFunItemsCompleted += roleClient_UpdateRoleFunItemsCompleted;

                ReturnCommand = new ActionCommand<object>(t => Return());
                SaveCommand = new ActionCommand<object>(t => SaveAction());
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private RoleServiceClient InitServiceClient()
        {
            RoleServiceClient roleClient = ServiceClientFactory.Create<RoleServiceClient>();
            ServiceClientFactory.CreateMessageHeader(roleClient.InnerChannel);

            roleClient.GetFunItemsByRoleIDCompleted += roleClient_GetFunItemsByRoleIDCompleted;
            roleClient.UpdateRoleFunItemsCompleted += roleClient_UpdateRoleFunItemsCompleted;
            return roleClient;
        }

        private void Return()
        {

        }

        void roleClient_GetFunItemsByRoleIDCompleted(object sender, GetFunItemsByRoleIDCompletedEventArgs e)
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
                    originalfunction.Clear();
                    var roleFunc = e.Result.Result.ToList();
                    originalfunction.AddRange(roleFunc);
                    foreach (var item in FunctionCheckList)
                    {
                        InitTree(roleFunc, item);
                    }
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
            RoleServiceClient roleClient = sender as RoleServiceClient;
            if (roleClient != null)
            {
                roleClient.CloseAsync();
                roleClient = null;
            }
        }

        void InitTree(List<string> roleFunc, FunctionItem item)
        {
            if (roleFunc.Any(t => t == item.ID))
            {
                item.IsChecked = true;
            }
            else
            {
                item.IsChecked = false;
            }

            if (item.HasChildren)
            {
                foreach (var itemChildren in item.Children)
                {
                    InitTree(roleFunc, itemChildren);
                }
            }
        }

        #region Save
        private void SaveAction()
        {
            var addFuns = new ObservableCollection<string>();
            AnalysisFuncItems(addFuns, FunctionCheckList);
            List<string> addfunction = new List<string>();
            foreach (var item in addFuns)
            {
                if (!originalfunction.Contains(item))
                {
                    addfunction.Add(item);
                }
            }

            StringBuilder builder = new StringBuilder();
            if (addfunction.Count != 0)
            {
                builder.Append(ApplicationContext.Instance.StringResourceReader.GetString("AddFunction") + ":");
                foreach (var item in addfunction)
                {
                    builder.Append(GetFunctionName(item, FunctionCheckList) + ";");
                }
            }

            List<string> deletefunction = new List<string>();
            foreach (var item in originalfunction)
            {
                if (!addFuns.Contains(item))
                {
                    deletefunction.Add(item);
                }
            }

            if (deletefunction.Count != 0)
            {
                builder.Append(ApplicationContext.Instance.StringResourceReader.GetString("DeleteFunction") + ":");
                foreach (var item in deletefunction)
                {
                    builder.Append(GetFunctionName(item, FunctionCheckList) + ";");
                }
            }

            LogOperate log = new LogOperate();
            log.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;
            log.ID = Guid.NewGuid().ToString();
            log.OperateTime = DateTime.Now.ToUniversalTime();
            log.OperatorID = ApplicationContext.Instance.AuthenticationInfo.UserID;
            log.OperatorName = ApplicationContext.Instance.AuthenticationInfo.Account;
            log.OperateContent = builder.ToString();
            RoleServiceClient roleClient = InitServiceClient();
            roleClient.UpdateRoleFunItemsAsync(_currentRoleID, addFuns, log);
        }

        private string GetFunctionName(string item, ObservableCollection<FunctionItem> FunctionCheckList)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var functionname in FunctionCheckList)
            {
                if (item == functionname.ID)
                {
                    return functionname.Name;
                }
                else if (item.Contains(functionname.ID))
                {
                    return functionname.Name + "." + GetFunctionName(item, functionname.Children);

                }
                else
                {
                    continue;
                }
            }

            return string.Empty;
        }

        void roleClient_UpdateRoleFunItemsCompleted(object sender, UpdateRoleFunItemsCompletedEventArgs e)
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
                        SaveResultArgs args = new SaveResultArgs();
                        args.Result = true;

                        if (OnSaveResult != null)
                        {
                            OnSaveResult(this, args);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("UpdateRole()", ex);
            }
            finally
            {
                CloseClient(sender);
            }
        }

        private void AnalysisFuncItems(ObservableCollection<string> addFuns, ObservableCollection<FunctionItem> func)
        {
            foreach (var item in func)
            {
                if (item.IsChecked == false)
                {
                    continue;
                }

                addFuns.Add(item.ID);

                if (item.HasChildren)
                {
                    AnalysisFuncItems(addFuns, item.Children);
                }
            }
        }
        #endregion

        public void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);

            try
            {
                _currentRoleID = (string)viewParameters["roleID"];
                _currentRoleCategory = (RoleCategory)viewParameters["roleCategory"];
                var mode = viewParameters["Mode"] as string;
                if (mode == "Update")
                {
                    EditEnable = true;
                    EditVisibility = Visibility.Visible;
                }
                else
                {
                    EditVisibility = Visibility.Collapsed;
                    EditEnable = false;
                }

                GetAllFunItemsByRoleCategory(_currentRoleCategory);
                RoleServiceClient roleClient = InitServiceClient();
                roleClient.GetFunItemsByRoleIDAsync(_currentRoleID);
            }
            catch (System.Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }

        private void GetAllFunItemsByRoleCategory(RoleCategory roleCategory)
        {
            try
            {
                var allFuncItems = FuncItemHelper.GetFuncItemByRoleType(roleCategory);
                if (allFuncItems == null || false == allFuncItems.HasChildren)
                {
                    return;
                }
                try
                {
                    //SecurityAdmin
                    if (string.Equals(_currentRoleID, "E5713BCC-A8A7-4CB3-A8AE-03292B67D52D"))
                    {
                        if (FuncItemHelper.MANAGER_UserMangerFunc.Children.Contains(FuncItemHelper.CloudUserManageFunc) == false)
                        {
                            FuncItemHelper.MANAGER_UserMangerFunc.Children.Insert(0, FuncItemHelper.CloudUserManageFunc);
                        }
                        if (FuncItemHelper.MANAGER_UserMangerFunc.Children.Contains(FuncItemHelper.RoleManageFunc) == false)
                        {
                            FuncItemHelper.MANAGER_UserMangerFunc.Children.Insert(0, FuncItemHelper.RoleManageFunc);
                        }
                    }
                    else
                    {
                        if (FuncItemHelper.MANAGER_UserMangerFunc.Children.Contains(FuncItemHelper.CloudUserManageFunc))
                        {
                            FuncItemHelper.MANAGER_UserMangerFunc.Children.Remove(FuncItemHelper.CloudUserManageFunc);
                        }
                        if (FuncItemHelper.MANAGER_UserMangerFunc.Children.Contains(FuncItemHelper.RoleManageFunc))
                        {
                            FuncItemHelper.MANAGER_UserMangerFunc.Children.Remove(FuncItemHelper.RoleManageFunc);
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }

                FunctionCheckList = allFuncItems.Children;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
    }
}
