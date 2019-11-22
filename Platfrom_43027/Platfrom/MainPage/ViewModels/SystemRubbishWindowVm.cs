using BaseLib.Model;
using BaseLib.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.MainPage;
using Gsafety.PTMS.ServiceReference.SystemRubbishService;
using Gsafety.PTMS.Share;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Vehicle = Gsafety.PTMS.ServiceReference.SystemRubbishService.Vehicle;
using DevSuite = Gsafety.PTMS.ServiceReference.SystemRubbishService.DevSuite;
using GUser = Gsafety.PTMS.ServiceReference.SystemRubbishService.GUser;
using Gsafety.PTMS.ServiceReference.VehicleService;

namespace Gsafety.PTMS.MainPage.ViewModels
{
    public class SystemRubbishWindowVm: BaseViewModel
    {
        private SystemRubbishServiceClient systemRubbishClient;

        #region 
        public ICommand BtnVehicleRecoverCommand { get; set; }

        public ICommand BtnSuiteRecoverCommand { get; set; }

        public ICommand BtnGUserRecoverCommand { get; set; }

        public ICommand BtnVehicleSearchCommand { get; set; }

        public ICommand BtnSuiteSearchCommand { get; set; }

        public ICommand BtnGUserSearchCommand { get; set; }

        private string searchByName;

        public string SearchByName
        {
            get
            {
                return searchByName;
            }
            set
            {
                this.searchByName = value;
                RaisePropertyChanged(() => this.SearchByName);
            }
        }

        private GUser currentGUserModel = new GUser();
    
        public GUser CurrentGUserModel
        {
            get { return currentGUserModel; }
            set
            {
                currentGUserModel = value;
                RaisePropertyChanged(() => CurrentGUserModel);
            }
        }


        protected List<GUser> _guserItems = new List<GUser>();

        PagedCollectionView _guserList = null;

        public PagedCollectionView GUserList
        {
            get
            {
                return _guserList;
            }
            set
            {
                if (_guserList != value)
                {
                    _guserList = value;
                    RaisePropertyChanged("GUserList");
                }
            }
        }

        protected List<Vehicle> _vehicleItems = new List<Vehicle>();

        PagedCollectionView _vehicleList = null;

        public PagedCollectionView VehicleList
        {
            get
            {
                return _vehicleList;
            }
            set
            {
                if (_vehicleList != value)
                {
                    _vehicleList = value;
                    RaisePropertyChanged("VehicleList");
                }
            }
        }


        private Vehicle vehicleObj = new Vehicle();
        /// <summary>
        /// 当前列表选中的车辆
        /// </summary>
        public Vehicle VehicleObj
        {
            get { return vehicleObj; }
            set
            {
                vehicleObj = value;
                RaisePropertyChanged(() => VehicleObj);
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

        private ServiceReference.VehicleService.VehicleType vehicletype = null;
        /// <summary>
        /// 查询条件 车类型
        /// </summary>
        public ServiceReference.VehicleService.VehicleType VehicleType
        {
            get { return vehicletype; }
            set { vehicletype = value; }
        }

        private DevSuite currentSuiteModel = new DevSuite();
        /// <summary>
        /// 当前列表选中的套件
        /// </summary>
        public DevSuite CurrentSuiteModel
        {
            get { return currentSuiteModel; }
            set
            {
                currentSuiteModel = value;
                RaisePropertyChanged(() => CurrentSuiteModel);
            }
        }


        protected List<DevSuite> _devSuiteItems = new List<DevSuite>();

        PagedCollectionView _devSuiteList = null;

        public PagedCollectionView DevSuiteList
        {
            get
            {
                return _devSuiteList;
            }
            set
            {
                if (_devSuiteList != value)
                {
                    _devSuiteList = value;
                    RaisePropertyChanged("DevSuiteList");
                }
            }
        }


        private string _suiteID = string.Empty;
        public string SuitID
        {
            get { return _suiteID; }
            set
            {
                _suiteID = value;
                RaisePropertyChanged(() => SuitID);
            }
        }

        private string _mdvrCoreSn = string.Empty;
        public string MdvrCoreSn
        {
            get { return _mdvrCoreSn; }
            set
            {
                _mdvrCoreSn = value;
                RaisePropertyChanged(() => MdvrCoreSn);
            }
        }

        private string _mdvrSn = string.Empty;
        public string MdvrSn
        {
            get { return _mdvrSn; }
            set
            {
                _mdvrSn = value;
                RaisePropertyChanged(() => MdvrSn);
            }
        }

        private string _mdvrSim = string.Empty;
        public string MdvrSim
        {
            get { return _mdvrSim; }
            set
            {
                _mdvrSim = value;
                RaisePropertyChanged(() => MdvrSim);
            }
        }

        private List<ServiceReference.VehicleService.VehicleType> bindvehicletypes = new List<ServiceReference.VehicleService.VehicleType>();
        public List<ServiceReference.VehicleService.VehicleType> VehicleTypes
        {
            get { return bindvehicletypes; }
            set
            {
                bindvehicletypes = value;
                RaisePropertyChanged(() => VehicleTypes);
            }
        }

        private int _currentSelectedIndex = 0;
        public int CurrentSelectedIndex
        {
            get { return _currentSelectedIndex; }
            set
            {
                if (_currentSelectedIndex != value)
                {
                    _currentSelectedIndex = value;
                    GetDataList();
                }
            }
        }

        #endregion

        public SystemRubbishWindowVm()
        {
            BtnVehicleRecoverCommand = new ActionCommand<object>(obj => RecoverVehicle());
            BtnSuiteRecoverCommand = new ActionCommand<object>(obj => RecoverSuite());
            BtnGUserRecoverCommand = new ActionCommand<object>(obj => RecoverGUser());
            BtnVehicleSearchCommand = new ActionCommand<object>(obj => GetAbandonVehicleList());
            BtnSuiteSearchCommand = new ActionCommand<object>(obj => GetAbandonSuiteList());
            BtnGUserSearchCommand = new ActionCommand<object>(obj => GetAbandonGUserList());
          
            systemRubbishClient = ServiceClientFactory.Create<SystemRubbishServiceClient>();
            systemRubbishClient.GetAbandonVehicleListCompleted += SystemRubbishClient_GetAbandonVehicleListCompleted;
            systemRubbishClient.RecoverVehicleCompleted += SystemRubbishClient_RecoverVehicleCompleted;
            systemRubbishClient.GetAbandonDevSuiteListCompleted += SystemRubbishClient_GetAbandonDevSuiteListCompleted;
            systemRubbishClient.RecoverDevSuiteCompleted += SystemRubbishClient_RecoverDevSuiteCompleted;
            systemRubbishClient.GetAbandonGUserListCompleted += SystemRubbishClient_GetAbandonGUserListCompleted;
            systemRubbishClient.RecoverGUserCompleted += SystemRubbishClient_RecoverGUserCompleted;

            GetVehicleTypeList();            
        }

        private void SystemRubbishClient_RecoverGUserCompleted(object sender, RecoverGUserCompletedEventArgs e)
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
                        GetAbandonGUserList();
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void SystemRubbishClient_GetAbandonGUserListCompleted(object sender, GetAbandonGUserListCompletedEventArgs e)
        {
            try
            {
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
                     _guserItems= new List<GUser>(e.Result.Result);
                     GUserList = new PagedCollectionView(_guserItems);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void SystemRubbishClient_RecoverDevSuiteCompleted(object sender, RecoverDevSuiteCompletedEventArgs e)
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
                        GetAbandonSuiteList();
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void SystemRubbishClient_GetAbandonDevSuiteListCompleted(object sender, GetAbandonDevSuiteListCompletedEventArgs e)
        {
            try
            {
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
                    _devSuiteItems = new List<DevSuite>(e.Result.Result);
                    DevSuiteList = new PagedCollectionView(_devSuiteItems);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private VehicleTypeClient GetVehicleTypeList()
        {
            VehicleTypeClient vehicleClient = ServiceClientFactory.Create<VehicleTypeClient>();
            vehicleClient.GetVehicleTypeListCompleted += vehicleClient_GetVehicleTypeListCompleted;
            vehicleClient.GetVehicleTypeListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID);

            return vehicleClient;
        }

        private void RecoverVehicle()
        {
            if (VehicleObj != null)
            {
                var recovervehicledialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsRecover), MessageDialogButton.OkAndCancel);
                recovervehicledialogResult.Closed += RecovervehicledialogResult_Closed;
            }
             
        }

        private void RecoverSuite()
        {
            if (CurrentSuiteModel != null)
            {
                var recoverSuitedialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsRecover), MessageDialogButton.OkAndCancel);
                recoverSuitedialogResult.Closed += RecoverSuitedialogResult_Closed; ;
            }
        }

        private void RecoverSuitedialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    systemRubbishClient.RecoverDevSuiteAsync(CurrentSuiteModel);
                }
            }
        }

        private void RecoverGUser()
        {
            if (CurrentGUserModel != null)
            {
                var recoverGUseredialogResult = (SelfMessageBox)MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(LProxy.IsRecover), MessageDialogButton.OkAndCancel);
                recoverGUseredialogResult.Closed += RecoverGUseredialogResult_Closed; 
            }
        }

        private void RecoverGUseredialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    systemRubbishClient.RecoverGUserAsync(CurrentGUserModel);
                }
            }
        }

        private void RecovervehicledialogResult_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    systemRubbishClient.RecoverVehicleAsync(VehicleObj);
                }
            }
        }
  

        private void SystemRubbishClient_RecoverVehicleCompleted(object sender, RecoverVehicleCompletedEventArgs e)
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
                        GetAbandonVehicleList();
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void SystemRubbishClient_GetAbandonVehicleListCompleted(object sender, GetAbandonVehicleListCompletedEventArgs e)
        {
            try
            {
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
                    _vehicleItems = new List<Vehicle>( e.Result.Result);
                    VehicleList = new PagedCollectionView(_vehicleItems);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }


        private void GetDataList()
        {
            switch (_currentSelectedIndex)
            {
                case 0:
                    GetVehicleTypeList();
                    GetAbandonVehicleList();
                    break;
                case 1:
                    GetAbandonSuiteList();
                    break;
                case 2:
                    GetAbandonGUserList();
                    break;
                default:
                    break;
            }
        }

        private void GetAbandonGUserList()
        {
            systemRubbishClient.GetAbandonGUserListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SearchByName);
        }

        private void GetAbandonVehicleList()
        {
            if (VehicleType == null)
            {
                VehicleType = VehicleTypes[0];
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleType));
            }

            systemRubbishClient.GetAbandonVehicleListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID,SearchVehicleId, SearchOwner, string.Empty, VehicleType.ID);
        }

        private void GetAbandonSuiteList()
        {
            systemRubbishClient.GetAbandonDevSuiteListAsync(ApplicationContext.Instance.AuthenticationInfo.ClientID, SuitID, MdvrCoreSn, MdvrSn, MdvrSim);
        }

       
       
        void vehicleClient_GetVehicleTypeListCompleted(object sender, GetVehicleTypeListCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result.IsSuccess)
                    {
                        bindvehicletypes = new List<ServiceReference.VehicleService.VehicleType>();
                        bindvehicletypes.Add(new ServiceReference.VehicleService.VehicleType() { ID = string.Empty, Name = ApplicationContext.Instance.StringResourceReader.GetString("All") });
                      
                        foreach (var item in e.Result.Result)
                        {
                            bindvehicletypes.Add(item);
                        }

                       
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleTypes));
                        GetAbandonVehicleList();
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

    }

    
}
