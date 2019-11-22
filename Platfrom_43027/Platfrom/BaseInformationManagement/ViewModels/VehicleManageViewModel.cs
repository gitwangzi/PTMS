using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using OrganizationEx = Gsafety.PTMS.Bases.Models.OrganizationEx;
using Vehicle = Gsafety.PTMS.Bases.Models.Vehicle;

namespace Gsafety.Ant.BaseInformation.ViewModels
{
    [ExportAsViewModel(BaseInformationName.VehicleManageVm)]
    public class VehicleManageViewModel : DetailViewModel<Vehicle>
    {
        #region 命令

        public ICommand BtnAddOrganizationCommand { get; set; }

        public ICommand BtnAddVehicleCommand { get; set; }

        public ICommand BtnEditOrganizationCommand { get; set; }

        public ICommand BtnEditVehicleCommand { get; set; }

        public ICommand BtnDeleteOrganizationCommand { get; set; }

        public ICommand BtnDeleteVehicleCommand { get; set; }

        #endregion

        #region 属性

        /// <summary>
        /// 构建树工厂
        /// </summary>
        public VehicleTreeFactory VehicleTreeFactory { get; set; }


        public TreeViewItem CurrentSelectedItem { get; set; }

        /// <summary>
        /// UI选中的项
        /// </summary>
        private MonityEntityBase _currentItem = new MonityEntityBase();
        public MonityEntityBase CurrentItem
        {
            get
            {
                return _currentItem;
            }
            set
            {
                if (_currentItem != value)
                {
                    _currentItem = value;

                    if (_currentItem is VehicleEx)
                    {
                        CurrentVehicle = (_currentItem as VehicleEx);
                        this.CurrentOrganizationEx = this.CurrentVehicle.Parent as OrganizationEx;
                    }
                    if (_currentItem is OrganizationEx)
                    {
                        CurrentOrganizationEx = this._currentItem as OrganizationEx;
                    }

                    RaisePropertyChanged(() => CurrentItem);
                }
            }
        }

        /// <summary>
        /// Currently selected vehicle
        /// </summary>
        public VehicleEx CurrentVehicle
        {
            get;
            set;

            //get { return _CurrentVehicle; }
            //set
            //{
            //    //if (value != null) LocateCar(value.VehicleId);
            //    //if (_CurrentVehicle != value)
            //    //{
            //    //    OnSelectedVehicleChanged(_CurrentVehicle, value);
            //    //    _CurrentVehicle = value;
            //    //    MonitorMenuVm.CurrentVehicleForGroup = _CurrentVehicle.VehicleInfo;
            //    //    if (_CurrentVehicle != null)
            //    //    {
            //    //        EventAggregator.Publish<Vehicle>(_CurrentVehicle.VehicleInfo);
            //    //    }
            //    //}
            //}
        }

        /// <summary>
        /// 当前选中的组织机构
        /// </summary>
        public OrganizationEx CurrentOrganizationEx { get; set; }

        private VehicleServiceClient vehicleServiceClient = null;

        private OrganizationClient organizationClient = null;

        #endregion

        public VehicleManageViewModel()
        {
            this.BtnAddOrganizationCommand = new ActionCommand<object>(method => AddOrganizationCommandExcute());
            this.BtnAddVehicleCommand = new ActionCommand<object>(method => AddVehicleCommandExecute());
            this.BtnEditOrganizationCommand = new ActionCommand<object>(method => EditOrganizationCommandExecute());
            this.BtnEditVehicleCommand = new ActionCommand<object>(method => EditVehicleCommandExecute());
            this.BtnDeleteOrganizationCommand = new ActionCommand<object>(method => DeleteOrganizationCommandExecute());
            this.BtnDeleteVehicleCommand = new ActionCommand<object>(method => DeleteVehicleCommandExecute());
            this.InlitizeViewModel();
            this.InlitizeClient();
            this.VehicleTreeFactory = new VehicleTreeFactory();
        }


        #region 方法

        private void InlitizeViewModel()
        {
            this.CurrentItem = new MonityEntityBase();
            this.CurrentOrganizationEx = new OrganizationEx();
            this.CurrentVehicle = new VehicleEx();
        }

        /// <summary>
        /// 初始化客户端
        /// </summary>
        private void InlitizeClient()
        {
            this.vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
            this.organizationClient = ServiceClientFactory.Create<OrganizationClient>();
            this.vehicleServiceClient.DeleteVehicleCompleted += vehicleServiceClient_DeleteVehicleCompleted;
            this.organizationClient.DeleteOrganizationCompleted += organizationClient_DeleteOrganizationCompleted;
        }

        /// <summary>
        /// 删除车辆组织机构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void organizationClient_DeleteOrganizationCompleted(object sender, DeleteOrganizationCompletedEventArgs e)
        {

        }

        /// <summary>
        /// 删除车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void vehicleServiceClient_DeleteVehicleCompleted(object sender, DeleteVehicleCompletedEventArgs e)
        {

        }

        private void OnSelectedVehicleChanged(VehicleEx oldValue, VehicleEx newValue)
        {
            //if ((oldValue != null) && (newValue != null))
            //{
            //    if (oldValue.VehicleId == newValue.VehicleId)
            //    {
            //        return;
            //    }
            //}
            //if (oldValue != null)
            //{
            //    //如果不是监控列表中则取消订阅
            //    string department = (oldValue.Parent as OrganizationEx).Organization.Name;
            //    if (CanUnbindGPS(oldValue.VehicleId)) MonitorGPS(oldValue.VehicleId, department, false, false, false);
            //}

            //if (newValue != null)
            //{
            //    //订阅
            //    string department = (newValue.Parent as OrganizationEx).Organization.Name;
            //    bool hasAlarm = ApplicationContext.Instance.BufferManager.AlarmManager.HasAlarm(newValue.VehicleId);
            //    MonitorGPS(newValue.VehicleId, department, true, hasAlarm, false);
            //}

        }

        //private void LocateCar(string CarNo)
        //{
        //    EventAggregator.Publish<DisplayCurrentPositionArgs>(new DisplayCurrentPositionArgs()
        //    {
        //        Prov = "",
        //        CarNo = CarNo,
        //        VE = (int)ElementLayerDefine.miVEGpsData
        //    });
        //}

        #endregion

        private void DeleteVehicleCommandExecute()
        {
            if (this.CurrentVehicle == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.PleaseToSelectedItem));
            }
            else
            {

            }
        }

        private void DeleteOrganizationCommandExecute()
        {
            if (this.CurrentOrganizationEx == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.PleaseToSelectedItem));
            }
            else
            {

            }
        }

        private void EditVehicleCommandExecute()
        {
            if (this.CurrentVehicle == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.PleaseToSelectedItem));
            }
            else
            {
                this.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleDetailV, new Dictionary<string, object>() { { "action", "Update" }, { "OrganizationId", "" } }));
            }
        }

        private void EditOrganizationCommandExecute()
        {
            if (this.CurrentOrganizationEx.Organization == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.PleaseToSelectedItem));
            }
            else
            {
                this.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleOrganizationDetailV, new Dictionary<string, object>() { { "action", "Update" }, { "data", this.CurrentOrganizationEx } }));
            }
        }

        private void AddVehicleCommandExecute()
        {
            this.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleDetailV, new Dictionary<string, object>() { { "action", "Add" }, { "OrganizationId", "" } }));
        }

        private void AddOrganizationCommandExcute()
        {
            //this.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleOrganizationDetailV, new Dictionary<string, object>() { { "action", "Add" } }));
            this.EventAggregator.Publish(new ViewNavigationArgs(BaseInformationName.VehicleOrganizationDetailV, new Dictionary<string, object>() { { "action", "Add" }, { "data", this.CurrentOrganizationEx } }));
        }
    }
}
