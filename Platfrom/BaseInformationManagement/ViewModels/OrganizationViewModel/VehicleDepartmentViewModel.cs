using BaseLib.ViewModels;
using Gsafety.Ant.BaseInformation.Views.Organization;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel
{
    /// <summary>
    /// 车辆机构管理
    /// </summary>
    [ExportAsViewModel(BaseInformationName.AntProductVehicleDepartmentVm)]
    public class VehicleDepartmentViewModel : ListViewModel<Organization>
    {
        #region 属性

        /// <summary>
        /// 查看组织机构明细按钮命令
        /// </summary>
        public ICommand BtnDetailCommand { get; set; }

        private ObservableCollection<Organization> vehicleDepartmentList = new ObservableCollection<Organization>();

        public ObservableCollection<Organization> VehicleDepartmentList
        {
            get { return this.vehicleDepartmentList; }
            set
            {
                this.vehicleDepartmentList = value;
                RaisePropertyChanged(() => this.VehicleDepartmentList);
            }
        }


        private ObservableCollection<TreeNode<Organization>> _vehicleDepartmentNodes = new ObservableCollection<TreeNode<Organization>>();
        /// <summary>
        /// 前台UI显示的车辆树形节点列表
        /// </summary>
        public ObservableCollection<TreeNode<Organization>> VehicleDepartmentNodes
        {
            get { return this._vehicleDepartmentNodes; }
            set
            {
                this._vehicleDepartmentNodes = value;
                RaisePropertyChanged(() => this.VehicleDepartmentNodes);
            }
        }

        private string id;
        /// <summary>
        /// 要删除的部门的主键
        /// </summary>
        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
                RaisePropertyChanged(() => this.Id);
            }
        }

        #endregion

        #region 构造函数

        public VehicleDepartmentViewModel()
        {
            this.BtnAddCommand = new ActionCommand<object>(this.AddVehicleDepartment);
            this.BtnEditCommand = new ActionCommand<object>(this.UpdateVehicleDepartment);
            this.BtnSearchCommand = new ActionCommand<object>(q => Query());
            this.BtnDetailCommand = new ActionCommand<object>(this.DetailVehicleDeparment);
            this.BtnDeleteCommand = new ActionCommand<object>(this.DeleteVehicleDepartment);
            this.InlitizeViewModel();
        }


        #endregion

        #region 方法

        /// <summary>
        /// 初始化构造函数
        /// </summary>
        private void InlitizeViewModel()
        {
            this.id = "";
            this.InlitTree();
        }

        private OrganizationClient InitialClient()
        {
            OrganizationClient client = ServiceClientFactory.Create<OrganizationClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetOrganizationByUserCompleted += client_GetOrganizationByUserCompleted;
            client.DeleteOrganizationCompleted += client_DeleteOrganizationCompleted;
            client.GetAllOrganizationCompleted += client_GetAllOrganizationCompleted;

            return client;
        }

        void client_GetAllOrganizationCompleted(object sender, GetAllOrganizationCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        var result = e.Result.Result;
                        this.VehicleDepartmentNodes.Clear();
                        this.VehicleDepartmentList.Clear();
                        if (result.Any())
                        {
                            this.VehicleDepartmentList = result;
                            ApplicationContext.Instance.VehicleDepartmentList = result;
                            var rootNode = TreeNodeFactory.CreateTreeForestFromModelListGeneric(this.VehicleDepartmentList.ToList(),
                                ve => ve.ID, ve => ve.ParentID, ve => this.VehicleDepartmentList.Any(t => t.ID == ve.ParentID) == false);
                            this.VehicleDepartmentNodes = rootNode;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(e.Result.ErrorMsg))
                        {
                            MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        }
                        else
                        {
                            MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(e.Result.ErrorMsg));
                        }
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                OrganizationClient client = sender as OrganizationClient;
                this.Close(client);
            }
        }

        private void InlitTree()
        {
            try
            {
                OrganizationClient client = InitialClient();

                if (ApplicationContext.Instance.AuthenticationInfo.IsClientCreate)
                {
                    client.GetAllOrganizationAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
                }
                else
                {
                    client.GetOrganizationByUserAsync(ApplicationContext.Instance.AuthenticationInfo.UserID);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void client_DeleteOrganizationCompleted(object sender, DeleteOrganizationCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
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
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(),
                               e.Result.ExceptionMessage);
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                    }
                }
                else
                {
                    this.Query();
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                OrganizationClient client = sender as OrganizationClient;
                Close(client);
            }
        }

        void client_GetOrganizationByUserCompleted(object sender, GetOrganizationByUserCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    foreach (var item in e.Result.Result)
                    {
                        item.CreateTime = item.CreateTime.ToLocalTime();
                    }
                    var result = e.Result.Result;
                    this.VehicleDepartmentNodes.Clear();
                    this.VehicleDepartmentList.Clear();
                    if (result.Any())
                    {
                        this.VehicleDepartmentList = result;
                        ApplicationContext.Instance.VehicleDepartmentList = result;
                        var rootNode = TreeNodeFactory.CreateTreeForestFromModelListGeneric(this.VehicleDepartmentList.ToList(),
                            ve => ve.ID, ve => ve.ParentID, ve => this.VehicleDepartmentList.Any(t => t.ID == ve.ParentID) == false);
                        this.VehicleDepartmentNodes = rootNode;
                    }
                }
                else
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                       ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                OrganizationClient client = sender as OrganizationClient;
                Close(client);
            }
        }

        /// <summary>
        /// 关闭Client
        /// </summary>
        private void Close(OrganizationClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private async void DeleteVehicleDepartment(object obj)
        {
            try
            {
                if (obj == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                    return;
                }
                var r = obj as TreeNode<Organization>;
                if (r == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        message);
                    return;
                }
                else
                {
                    Organization org = r.Model;
                    if (r.Children.Count == 0)
                    {
                        var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                        var result = await dialogResult;

                        if (result == MessageDialogResult.OK)
                        {
                            this.Id = r.Model.ID;
                            OrganizationClient client = InitialClient();
                            client.DeleteOrganizationAsync(this.Id, ApplicationContext.Instance.AuthenticationInfo.UserID);
                        }
                    }
                    else
                    {
                        var dialogResult = MessageBoxHelper.ShowDialogMessageTask(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("HasChild"), MessageDialogButton.Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 查看详细
        /// </summary>
        /// <param name="obj"></param>
        private void DetailVehicleDeparment(object obj)
        {
            try
            {
                if (obj == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                    return;
                }
                var r = obj as TreeNode<Organization>;
                if (r == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        message);
                    return;
                }
                else
                {
                    //当前节点的父级名称
                    string parentName = "";
                    //当前节点的父级编号
                    string parentId = "0";
                    var veParent = r.Parent as TreeNode<Organization>;
                    if (veParent != null)
                    {
                        parentName = veParent.Model.Name;
                        parentId = veParent.Model.ID;
                    }

                    var window = new AddVehicleDepartmentWindow("", new Dictionary<string, object>() { { "action", "view" }, { "ParentName", parentName }, { "ParentId", parentId }, { "data", r.Model } });
                    window.Show();
                    window.Closed += window_Closed;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        private void UpdateVehicleDepartment(object obj)
        {
            try
            {
                if (obj == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                    return;
                }
                var r = obj as TreeNode<Organization>;
                if (r == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        message);
                    return;
                }
                else
                {
                    //当前节点的父级名称
                    string parentName = "";
                    //当前节点的父级编号
                    string parentId = "0";
                    var veParent = r.Parent as TreeNode<Organization>;
                    if (veParent != null)
                    {
                        parentName = veParent.Model.Name;
                        parentId = veParent.Model.ID;
                    }

                    var window = new AddVehicleDepartmentWindow("", new Dictionary<string, object>() { { "action", "Update" }, { "ParentName", parentName }, { "ParentId", parentId }, { "data", r.Model } });
                    window.Show();
                    window.Closed += window_Closed;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void AddVehicleDepartment(object obj)
        {
            try
            {
                TreeNode<Organization> parentorg = null;
                if (obj == null)
                {
                    if (!ApplicationContext.Instance.AuthenticationInfo.IsClientCreate)
                    {
                        string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                        return;
                    }
                }

                parentorg = obj as TreeNode<Organization>;
                if (parentorg == null)
                {
                    if (!ApplicationContext.Instance.AuthenticationInfo.IsClientCreate)
                    {
                        string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                            message);
                        return;
                    }
                    else
                    {
                        if (VehicleDepartmentList.Count != 0)
                        {
                            string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                                message);
                            return;
                        }
                    }
                }

                AddVehicleDepartmentWindow window = null;

                if (parentorg != null)
                {
                    window = new AddVehicleDepartmentWindow("", new Dictionary<string, object>() { { "action", "addOrg" }, { "ParentName", parentorg.Model.Name }, { "ParentId", parentorg.Model.ID } });
                }
                else
                {
                    window = new AddVehicleDepartmentWindow("", new Dictionary<string, object>() { { "action", "addOrg" } });
                }
                window.Show();
                window.Closed += window_Closed;

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void window_Closed(object sender, EventArgs e)
        {
            this.Query();
        }

        protected override void Query()
        {
            this.InlitTree();
        }

        #endregion
    }
}
