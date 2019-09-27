using BaseLib.ViewModels;
using Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Librarys;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Installation;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using Gsafety.Ant.BaseInformation.Views.Organization;

namespace Gsafety.Ant.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.VehicleManageVm)]
    public class VehicleManageViewModel : ListViewModel<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>
    {
        public ICommand BtnDetailOrganizationCommand { get; set; }
        public ICommand BtnSearchOrganizationCommand { get; set; }
        public ICommand BtnDetailCommand { get; set; }
        public VehicleManageViewModel()
        {
            this.BtnSearchOrganizationCommand = new ActionCommand<object>(q => QueryOrganization());
            this.BtnDetailOrganizationCommand = new ActionCommand<object>(this.DetailVehicleDeparment);
            BtnDetailCommand = new ActionCommand<object>(obj => Update("view"));
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
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {

            if (vehicleDepartmentList.Count == 0)
            {
                QueryOrganization();
            }

            vehicletypelist.Clear();

            vehicletypelist.Add(new Gsafety.PTMS.ServiceReference.VehicleService.VehicleType() { ID = string.Empty, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
            SelectedVehicleType = vehicletypelist[0];

            VehicleTypeClient client = InitVehicleTypeClient();
            client.GetVehicleTypeListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);

            base.ActivateView(viewName, viewParameters);
        }

        protected override void ViewDetail(string actionName)
        {
            Update("view");
        }

        protected void QueryOrganization()
        {
            InlitTree();
        }

        private void InlitTree()
        {
            try
            {
                OrganizationClient client = InitialClient();
                client.GetOrganizationByUserAsync(ApplicationContext.Instance.AuthenticationInfo.UserID);
               // client.GetAllOrganizationAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private OrganizationClient InitialClient()
        {
            OrganizationClient client = ServiceClientFactory.Create<OrganizationClient>();
            ServiceClientFactory.CreateMessageHeader(client.InnerChannel);
            client.GetOrganizationByUserCompleted += Client_GetOrganizationByUserCompleted;
           // client.GetAllOrganizationCompleted += client_GetAllOrganizationCompleted;

            return client;
        }

        private void Client_GetOrganizationByUserCompleted(object sender, GetOrganizationByUserCompletedEventArgs e)
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

        /// <summary>
        /// 初始化车辆类别服务客户端方法
        /// </summary>
        /// <returns></returns>
        private VehicleTypeClient InitVehicleTypeClient()
        {
            VehicleTypeClient vehicleClient = ServiceClientFactory.Create<VehicleTypeClient>();
            vehicleClient.GetBscVehicleListCompleted += vehicleClient_GetBscVehicleListCompleted;
            vehicleClient.GetVehicleTypeListCompleted += vehicleClient_GetVehicleTypeListCompleted;

            return vehicleClient;
        }

        private List<Gsafety.PTMS.ServiceReference.VehicleService.VehicleType> vehicletypelist = new List<Gsafety.PTMS.ServiceReference.VehicleService.VehicleType>();
        List<Gsafety.PTMS.ServiceReference.VehicleService.VehicleType> vehciletypes = new List<Gsafety.PTMS.ServiceReference.VehicleService.VehicleType>();

        Gsafety.PTMS.ServiceReference.VehicleService.VehicleType selectedvehicletype = null;

        public Gsafety.PTMS.ServiceReference.VehicleService.VehicleType SelectedVehicleType
        {
            get { return selectedvehicletype; }
            set
            {
                selectedvehicletype = value;
                RaisePropertyChanged(() => SelectedVehicleType);
            }
        }
        /// <summary>
        /// 所有省份
        /// </summary>
        public List<Gsafety.PTMS.ServiceReference.VehicleService.VehicleType> VehicleTypesList
        {
            get { return vehicletypelist; }
            set
            {
                vehicletypelist = value;
                RaisePropertyChanged(() => VehicleTypesList);
            }
        }

        void vehicleClient_GetVehicleTypeListCompleted(object sender, GetVehicleTypeListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        vehciletypes.Clear();
                        foreach (var item in e.Result.Result)
                        {
                            vehciletypes.Add(item);
                            vehicletypelist.Add(item);
                        }

                        RaisePropertyChanged(() => VehicleTypesList);
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

                ApplicationContext.Instance.Logger.LogException("VehicleDepartmentListViewModel.vehicleClient_GetVehicleTypeListCompleted", ex);
            }
            finally
            {
                VehicleTypeClient vehicleClient = sender as VehicleTypeClient;
                vehicleClient.CloseAsync();
                vehicleClient = null;
            }

        }

        /// <summary>
        /// 获取车辆列表服务完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vehicleClient_GetBscVehicleListCompleted(object sender, GetBscVehicleListCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    return;
                }

                if (e.Error != null)
                {
                    MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.ServerError));
                    ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), e.Error);
                    return;
                }

                var result = e.Result;
                if (result.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(result.ErrorMsg) == false)
                    {
                        MessageBoxHelper.ShowDialog(
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                        ApplicationContext.Instance.StringResourceReader.GetString(LProxy.OperatorServiceError));
                        ApplicationContext.Instance.Logger.LogError(MethodBase.GetCurrentMethod().ToString(), result.ErrorMsg);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(
                       ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption),
                       ApplicationContext.Instance.StringResourceReader.GetString(result.ErrorMsg));
                    }
                }
                else
                {
                    foreach (var item in e.Result.Result)
                    {
                        item.CreateTime = item.CreateTime.ToLocalTime();
                    }
                    Data.loader_Finished(new BaseLib.Model.PagedResult<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>()
                    {
                        Count = e.Result.TotalRecord,
                        Items = e.Result.Result,
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
                VehicleTypeClient vehicleClient = sender as VehicleTypeClient;
                CloseVehicleService(vehicleClient);
            }
        }

        private void CloseVehicleService(VehicleServiceClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        private void CloseVehicleService(VehicleTypeClient client)
        {
            if (client != null)
            {
                client.CloseAsync();
            }
            client = null;
        }

        string VehicleDepartmentId = string.Empty;
        string VehicleDepartmentName = string.Empty;

        public void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if (treeView != null)
            {
                var selectedItem = treeView.SelectedItem;
                var target = selectedItem as TreeNode<PTMS.ServiceReference.OrganizationService.Organization>;
                if (target != null)
                {
                    VehicleDepartmentId = target.Model.ID;
                    VehicleDepartmentName = target.Model.Name;

                    Query();
                }
                else
                {
                    ApplicationContext.Instance.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.EmptyView));
                }
            }
        }

        /// <summary>
        /// 查询车辆
        /// </summary>
        protected override void Query()
        {
            try
            {
                currentIndex = 1;
                Data.MoveToFirstPage();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleDepartmentViewModel()", ex);
            }
        }

        /// <summary>
        /// 添加车辆
        /// </summary>
        /// <returns></returns>
        protected override void Add(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(this.VehicleDepartmentName) || string.IsNullOrEmpty(this.VehicleDepartmentId))
                {
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString("SelectED"));
                }
                else
                {
                    VehicleObj = new Gsafety.PTMS.ServiceReference.VehicleService.Vehicle();
                    VehicleObj.OrgnizationId = this.VehicleDepartmentId;
                    VehicleObj.OrgnizationName = this.VehicleDepartmentName;

                    var addVehicleDepartmentDetailView = new AddVehicleDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { 
                { "action", name }, { "data", VehicleObj},{"OrgnizationId",this.VehicleDepartmentId},{"OrgnizationName",this.VehicleDepartmentName},{"VehicleTypes",vehciletypes}
                });
                    addVehicleDepartmentDetailView.Show();
                    addVehicleDepartmentDetailView.Closed += addVehicleDepartmentDetailView_Closed;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void addVehicleDepartmentDetailView_Closed(object sender, EventArgs e)
        {
            Data.RefreshPage();
        }

        private Gsafety.PTMS.ServiceReference.VehicleService.Vehicle vehicleObj = new Gsafety.PTMS.ServiceReference.VehicleService.Vehicle();
        /// <summary>
        /// 当前列表选中的车辆
        /// </summary>
        public Gsafety.PTMS.ServiceReference.VehicleService.Vehicle VehicleObj
        {
            get { return vehicleObj; }
            set
            {
                vehicleObj = value;
                RaisePropertyChanged(() => VehicleObj);
            }
        }

        /// <summary>
        /// 编辑和查看车辆明细
        ///update,view
        /// </summary>
        /// <param name="name"></param>
        protected override void Update(string name)
        {
            try
            {
                VehicleObj.OrgnizationId = this.VehicleDepartmentId;
                VehicleObj.OrgnizationName = this.VehicleDepartmentName;
                var addVehicleDepartmentDetailView = new AddVehicleDetailWindow("", new System.Collections.Generic.Dictionary<string, object>() { 
                { "action", name }, { "data", VehicleObj} ,{"VehicleTypes",vehciletypes}
                });
                addVehicleDepartmentDetailView.Show();
                addVehicleDepartmentDetailView.Closed += addVehicleDepartmentDetailView_Closed;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        /// <summary>
        /// 删除车辆
        /// </summary>
        protected override void Delete()
        {
            try
            {
                if (VehicleObj == null)
                {
                    string message = ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_PleaseSelect");
                    MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), message);
                    return;
                }
                else
                {
                    var childwindow = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsDelete), MessageDialogButton.OkAndCancel);
                    childwindow.Closed += childwindow_Closed;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        /// <summary>
        /// 初始化客户端的方法
        /// </summary>
        private VehicleServiceClient InitVehicleServiceClient()
        {
            VehicleServiceClient vehicleClient = ServiceClientFactory.Create<VehicleServiceClient>();
            vehicleClient.DeleteVehicleCompleted += vehicleClient_DeleteVehicleCompleted;
            return vehicleClient;
        }

        void childwindow_Closed(object sender, EventArgs e)
        {
            ChildWindow w = sender as ChildWindow;
            if (w.DialogResult == true)
            {
                VehicleServiceClient vehicleClient = InitVehicleServiceClient();
                vehicleClient.DeleteVehicleAsync(VehicleObj);
            }
        }

        /// <summary>
        /// 删除车辆服务完成方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vehicleClient_DeleteVehicleCompleted(object sender, DeleteVehicleCompletedEventArgs e)
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
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
            finally
            {
                VehicleServiceClient vehicleClient = sender as VehicleServiceClient;
                CloseVehicleService(vehicleClient);
            }
        }

        private string searchVehicleId;
        /// <summary>
        /// 查询条件 车牌号
        /// </summary>
        public string SearchVehicleId
        {
            get { return searchVehicleId; }
            set
            {
                searchVehicleId = value;
                RaisePropertyChanged(() => SearchVehicleId);
            }
        }

        private string searchOwner;
        /// <summary>
        /// 查询条件 车主
        /// </summary>
        public string SearchOwner
        {
            get { return searchOwner; }
            set
            {
                searchOwner = value;
                RaisePropertyChanged(() => SearchOwner);
            }
        }

        /// <summary>
        /// 初始化页面需加载的数据
        /// </summary>
        protected override void InitPagination()
        {

            try
            {
                Data = new BaseLib.Model.PagedServerCollection<Gsafety.PTMS.ServiceReference.VehicleService.Vehicle>((pageIndex, pageSize) =>
                {
                    if (SelectedVehicleType != null)
                    {
                        pageSize = PageSizeValue;
                        System.Threading.Interlocked.Exchange(ref currentIndex, pageIndex);
                        VehicleTypeClient vehicleClient = InitVehicleTypeClient();
                        vehicleClient.GetBscVehicleListAsync(currentIndex, PageSizeValue, SearchVehicleId, SearchOwner, string.Empty, this.VehicleDepartmentId, SelectedVehicleType.ID);
                    }
                });

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("VehicleDepartmentViewModel().InitPagination", ex);
            }
        }
    }
}
