using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Manager.Views.Organization;
using Gsafety.PTMS.ServiceReference.AccountService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel
{

    [ExportAsViewModel(ManagerName.AntProductUserDepartmentVm)]
    public class AntProductUserDepartmentViewModel : ListViewModel<UsrDepartment>
    {
        private ObservableCollection<UsrDepartment> usrDepartmentList;
        /// <summary>
        /// 前台用户组织机构树形列表
        /// </summary>
        public ObservableCollection<UsrDepartment> UsrDepartmentList
        {
            get { return this.usrDepartmentList; }
            set
            {
                this.usrDepartmentList = value;
                this.RaisePropertyChanged(() => this.UsrDepartmentList);
            }
        }

        private ObservableCollection<TreeNode<UsrDepartment>> _usrDepartmentNodes = new ObservableCollection<TreeNode<UsrDepartment>>();
        public ObservableCollection<TreeNode<UsrDepartment>> UsrDepartmentNodes
        {
            get { return _usrDepartmentNodes; }
            set
            {
                _usrDepartmentNodes = value;
                RaisePropertyChanged(() => UsrDepartmentNodes);
            }
        }

        private string name;
        /// <summary>
        /// 前台查询条件Name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                RaisePropertyChanged(() => this.Name);
            }
        }

        private string ID { get; set; }

        private TreeNode<UsrDepartment> selectedTreeLevelItem;
        /// <summary>
        /// 获取选择的树结构项
        /// </summary>
        public TreeNode<UsrDepartment> SelectedTreeLevelItem
        {
            get { return this.selectedTreeLevelItem; }
            set
            {
                this.selectedTreeLevelItem = value;
                RaisePropertyChanged(() => this.SelectedTreeLevelItem);
            }
        }

        /// <summary>
        /// 操作名称
        /// 增加
        /// 修改
        /// 查看明细
        /// </summary>
        public string ActionName { get; set; }

        public string ClientId { get; set; }


        /// <summary>
        /// 查看组织机构明细按钮命令
        /// </summary>
        public ICommand BtnDetailCommand { get; set; }

        public ICommand SelectedCommand { get; set; }

        public AntProductUserDepartmentViewModel()
        {
            try
            {
                this.InitlizeViewModel();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AntProductUserDepartmentViewModel()", ex);
            }

        }

        private void InitlizeViewModel()
        {
            try
            {
                this.Name = "";
                this.ActionName = "";
                this.ID = "";
                this.ClientId = ApplicationContext.Instance.AuthenticationInfo.ClientID;
                this.SelectedTreeLevelItem = new TreeNode<UsrDepartment>();
                this.BtnAddCommand = new ActionCommand<object>(this.AddUsrDepartment);
                this.BtnEditCommand = new ActionCommand<object>(this.UpdateDepartment);
                this.BtnSearchCommand = new ActionCommand<object>(q => Query());
                this.BtnDetailCommand = new ActionCommand<object>(this.DetailDeparment);
                this.BtnDeleteCommand = new ActionCommand<object>(this.DeleteDepartment);
                this.UsrDepartmentList = new ObservableCollection<UsrDepartment>();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private UsrDepartmentClient InitialClient()
        {
            UsrDepartmentClient client = ServiceClientFactory.Create<UsrDepartmentClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetUserDepartmentListCompleted += client_GetUserDepartmentListCompleted;

            client.IsCanDeleteUsrDepartmentCompleted += client_IsCanDeleteUsrDepartmentCompleted;
            client.DeleteUsrDepartmentByIDCompleted += client_DeleteUsrDepartmentByIDCompleted;

            return client;
        }



        /// <summary>
        /// 通过编号删除部门
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_DeleteUsrDepartmentByIDCompleted(object sender, DeleteUsrDepartmentByIDCompletedEventArgs e)
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
                        Query();
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 判断此部门是否能够删除
        /// 存在子部门和此部门下有所属用户的不能删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_IsCanDeleteUsrDepartmentCompleted(object sender, IsCanDeleteUsrDepartmentCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                            ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                }
                else
                {
                    if (e.Result.IsSuccess == false)
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(
                                 ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                                 ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), e.Result.ErrorMsg);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog("", ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                            ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Result.ExceptionMessage);
                        }
                    }
                    else
                    {
                        if (e.Result.Result)
                        {
                            UsrDepartmentClient client = InitialClient();
                            client.DeleteUsrDepartmentByIDAsync(this.ID);
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(
                                ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                                ApplicationContext.Instance.StringResourceReader.GetString(LProxy.CannotDeleteUsed));
                            return;
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
                CloseDepartmentClient(sender);
            }
        }

        private static void CloseDepartmentClient(object sender)
        {
            UsrDepartmentClient client = sender as UsrDepartmentClient;
            client.CloseAsync();
            client = null;
        }

        /// <summary>
        /// 自动获取数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetUserDepartmentListCompleted(object sender, GetUserDepartmentListCompletedEventArgs e)
        {
            try
            {
                var result = e.Result.Result;
                this.UsrDepartmentNodes.Clear();
                if (result.Any())
                {
                    UsrDepartmentList = result;

                    Func<UsrDepartment, bool> parentFilter = null;
                    if (ApplicationContext.Instance.AuthenticationInfo.IsClientCreate)
                    {
                        parentFilter = usr => string.IsNullOrWhiteSpace(usr.ParentID);
                    }
                    else
                    {
                        parentFilter = usr => usr.ID == ApplicationContext.Instance.AuthenticationInfo.Department;
                    }

                    var rootNode = TreeNodeFactory.CreateTreeFromModelListGeneric(UsrDepartmentList.ToList(), usr => usr.ID, usr => usr.ParentID, parentFilter);
                    this.UsrDepartmentNodes.Add(rootNode);

                    if (!string.IsNullOrWhiteSpace(this.Name))
                    {
                        var name = this.Name.Trim().ToLower();
                        Func<UsrDepartment, bool> filter = t => t.Name.ToLower().Contains(name);
                        TreeNodeFactory.SearchSingleTreeWithDelete<UsrDepartment>(UsrDepartmentNodes, filter);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                CloseDepartmentClient(sender);
            }
        }

        private void AddUsrDepartment(object r)
        {
            this.ActionName = "add";
            if (UsrDepartmentList.Count == 0)
            {
                if (ApplicationContext.Instance.AuthenticationInfo.IsClientCreate)
                {
                    var window = new AntProductUserDepartmentDetailWindow("", new Dictionary<string, object>() { { "action", this.ActionName } });
                    window.Show();
                    window.Closed += window_Closed;
                    return;
                }
            }

            if (r == null)
            {
                string message = ApplicationContext.Instance.StringResourceReader.GetString("PleaseSelectOrganization");
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                return;
            }
            var obj = r as TreeNode<UsrDepartment>;
            if (obj == null)
            {
                return;
            }
            else
            {
                var windowOperator = new AntProductUserDepartmentDetailWindow("", new Dictionary<string, object>() { { "action", this.ActionName }, { "departmentParentName", obj.Model.Name }, { "departmentParentId", obj.Model.ID } });
                windowOperator.Show();
                windowOperator.Closed += window_Closed;
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            this.Query();
        }

        /// <summary>
        /// 更新部门
        /// </summary>
        /// <param name="r"></param>
        private void UpdateDepartment(object r)
        {
            try
            {
                this.ActionName = "update";
                if (r == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("PleaseSelectOrganization");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        message);
                    return;
                }
                var obj = r as TreeNode<UsrDepartment>;
                if (obj == null)
                {
                    return;
                }

                string parentName = "";
                if (obj.Parent != null)
                {
                    parentName = (obj.Parent as TreeNode<UsrDepartment>).Model.Name;
                }
                var windowOperator = new AntProductUserDepartmentDetailWindow("", new Dictionary<string, object>() { { "action", this.ActionName }, { "departmentParentName", parentName }, { "departmentId", obj.Model.ID } });
                windowOperator.Show();
                windowOperator.Closed += window_Closed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        protected override void Query()
        {
            UsrDepartmentClient client = InitialClient();
            client.GetUserDepartmentListAsync("", ClientId);
        }

        /// <summary>
        /// 查看组织机构明细界面
        /// </summary>
        private void DetailDeparment(object r)
        {
            try
            {
                this.ActionName = "view";
                if (r == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("PleaseSelectOrganization");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        message);
                    return;
                }
                var obj = r as TreeNode<UsrDepartment>;
                if (obj == null)
                {
                    return;
                }
                string parentName = "";
                if (obj.Parent != null)
                {
                    parentName = (obj.Parent as TreeNode<UsrDepartment>).Model.Name;
                }

                var windowOperator = new AntProductUserDepartmentDetailWindow("", new Dictionary<string, object>() { { "action", this.ActionName }, { "departmentParentName", parentName }, { "departmentId", obj.Model.ID } });
                windowOperator.Show();
                windowOperator.Closed += window_Closed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 删除组织机构执行方法
        /// </summary>
        private async void DeleteDepartment(object r)
        {
            try
            {
                if (r == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("PleaseSelectOrganization");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        message);
                    return;
                }
                var obj = r as TreeNode<UsrDepartment>;
                if (obj == null)
                {
                    return;
                }


                var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                var result = await dialogResult;

                if (result == MessageDialogResult.OK)
                {
                    this.ID = obj.Model.ID;
                    UsrDepartmentClient client = InitialClient();
                    client.IsCanDeleteUsrDepartmentAsync(this.ID);
                }
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
                base.ActivateView(viewName, viewParameters);
                this.Query();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

    }
}
