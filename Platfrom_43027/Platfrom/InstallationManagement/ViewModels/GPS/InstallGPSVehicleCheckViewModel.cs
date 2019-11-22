using Gsafety.Ant.Installation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.ServiceReference.InstallStationService;
using Gsafety.PTMS.ServiceReference.VehicleService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallGPSVehicleCheckVm)]
    public class InstallGPSVehicleCheckViewModel : InstallGPSViewModelBase
    {
        int resultcode = -1;

        public InstallGPSVehicleCheckViewModel()
        {
            try
            {
                step = 1;
                SetupTime = DateTime.Now;
                ImageSource = "GpsSetp1.png";
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel()", ex);
            }
        }

        private VehicleServiceClient InitialVehicleClient()
        {
            VehicleServiceClient vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
            vehicleServiceClient.CheckInstallVehicleForGPSCompleted += vehicleServiceClient_CheckInstallVehicleForGPSCompleted;
            deviceInstallServiceClient.SubmitGPSForStep1Completed += deviceInstallServiceClient_SubmitGPSForStep1Completed;

            return vehicleServiceClient;
        }

        List<InstallStation> _stations = null;

        public List<InstallStation> Stations
        {
            get { return _stations; }
            set
            {
                _stations = value;
                RaisePropertyChanged(() => Stations);
            }
        }

        public InstallStation SelectedStation { get; set; }

        protected override void ActivateView(string viewName, IDictionary<string, object> viewParameters)
        {
            try
            {
                ResetData();
                Stations = ApplicationContext.Instance.AuthenticationInfo.Stations;
                if (Stations.Count != 0)
                {
                    SelectedStation = Stations[0];
                    RaisePropertyChanged(() => SelectedStation);
                }
                else
                {
                    SetError("SelectedStation", ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_NotAssignSetupStation"));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        //Methods
        protected override void Get()
        {
            try
            {
                VehicleServiceClient vehicleServiceClient = InitialVehicleClient();
                vehicleServiceClient.CheckInstallVehicleForGPSAsync(carNumber, ApplicationContext.Instance.AuthenticationInfo.ClientID);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void vehicleServiceClient_CheckInstallVehicleForGPSCompleted(object sender, CheckInstallVehicleForGPSCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    if (e.Result.Result.Result != 0)
                    {
                        OrgnizationName = e.Result.Result.OrgnizationName;
                        VehicleSn = e.Result.Result.VehicleSn;
                        EngineId = e.Result.Result.EngineId;
                        OperationLicense = e.Result.Result.OperationLicense;
                        ContactPhone = e.Result.Result.ContactPhone;
                        Owner = e.Result.Result.Owner;
                        CarType = e.Result.Result.Type.ToString();
                        resultcode = e.Result.Result.InvalidCode;
                        if (e.Result.Result.InvalidCode == 1)
                        {
                            IsNoteEnabled = true;
                        }
                        else
                        {
                            //此车已安装过
                            MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_VehicleAlreadyInstalled"), MessageDialogButton.Ok);
                        }

                        CheckNextBtnIsEnable();
                    }
                    else
                    {
                        ClearCarInfo();
                        //车牌号不存在
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ValidateNotRight"), MessageDialogButton.Ok);
                    }
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarType));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel CheckInstallVehicleCompleted", ex);
            }
            finally
            {
                VehicleServiceClient client = sender as VehicleServiceClient;
                client.CloseAsync();
            }
        }


        //Installers
        private string setupStaff;
        public string SetupStaff
        {
            get { return setupStaff; }
            set
            {
                setupStaff = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SetupStaff));
                ValidateSetupStaffIsEmpty();
                CheckNextBtnIsEnable();
            }
        }

        //Installation time
        private DateTime _SetupTime = DateTime.Now;
        public DateTime SetupTime
        {
            get
            {
                return _SetupTime;
            }
            set
            {
                _SetupTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SetupTime));
            }
        }

        //License plate number
        private string carNumber;
        public string CarNumber
        {
            get { return carNumber; }
            set
            {
                string tempValue = value == null ? null : value.Trim();
                if (tempValue != carNumber)
                {
                    carNumber = tempValue;
                    ValidateCarNumberIsEmpty();
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarNumber));
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarNumber));
            }
        }

        //Vehicle Type
        private string _carType = string.Empty;
        public string CarType
        {
            get
            {
                return _carType;
            }
            set
            {
                _carType = value;
                RaisePropertyChanged(() => CarType);
            }
        }

        private bool isNoteEnabled = true;
        public bool IsNoteEnabled
        {
            get
            {
                return isNoteEnabled;
            }
            set
            {
                isNoteEnabled = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsNoteEnabled));
            }
        }

        //Remark
        private string _Note;
        public string Note
        {
            get
            {
                return _Note;
            }
            set
            {
                _Note = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
            }
        }

        private string _orgnizationname;
        public string OrgnizationName
        {
            get
            {
                return _orgnizationname;
            }
            set
            {
                _orgnizationname = value;
                RaisePropertyChanged("OrgnizationName");
            }
        }

        private string _vehiclesn;
        public string VehicleSn
        {
            get
            {
                return _vehiclesn;
            }
            set
            {
                _vehiclesn = value;
                RaisePropertyChanged("VehicleSn");
            }
        }

        private string _engineid;
        public string EngineId
        {
            get
            {
                return _engineid;
            }
            set
            {
                _engineid = value;
                RaisePropertyChanged("EngineId");
            }
        }

        private string _operationlicense;
        public string OperationLicense
        {
            get
            {
                return _operationlicense;
            }
            set
            {
                _operationlicense = value;
                RaisePropertyChanged("OperationLicense");
            }
        }

        private string _contactphone;
        public string ContactPhone
        {
            get
            {
                return _contactphone;
            }
            set
            {
                _contactphone = value;
                RaisePropertyChanged("ContactPhone");
            }
        }

        private string _owner;
        public string Owner
        {
            get
            {
                return _owner;
            }
            set
            {
                _owner = value;
                RaisePropertyChanged("Owner");
            }
        }

        Step1Package package;

        protected override void NextPage()
        {
            try
            {
                IsFinished = false;
                package = new Step1Package();
                /// ID
                package.ID = System.Guid.NewGuid().ToString();
                /// License plate number
                package.VehicleId = CarNumber;
                /// Installation point number
                package.InstallationStationId = SelectedStation.ID;
                /// Installers
                package.InstallationStaff = SetupStaff;
                /// Record staff
                package.RecordStaff = ApplicationContext.Instance.AuthenticationInfo.Account;
                /// Start the installation time
                package.CreateTime = SetupTime.ToUniversalTime();
                /// The current installation steps
                package.Step = 1;

                package.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;

                package.Note = Note;

                deviceInstallServiceClient.SubmitGPSForStep1Async(package);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel NextPage", ex);
            }
        }

        void deviceInstallServiceClient_SubmitGPSForStep1Completed(object sender, SubmitGPSForStep1CompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    ResetData();
                    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallGPSCheckV, new Dictionary<string, object>() { { "ID", package.ID } }));
                }
                else
                {
                    if (e.Result.Result != string.Empty)
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString(e.Result.Result), MessageDialogButton.Ok);
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), MessageDialogButton.Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel SubmitForStepOneCompleted", ex);
            }
        }

        protected override void ResetData()
        {
            try
            {
                SetupTime = DateTime.Now;
                SetupStaff = string.Empty;

                CarNumber = string.Empty;

                ClearCarInfo();

                Note = string.Empty;

                IsFinished = false;
                IsNoteEnabled = true;
                IsGetMessage = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel ResetData", ex);
            }
        }

        private void ClearCarInfo()
        {
            OrgnizationName = string.Empty;
            VehicleSn = string.Empty;
            EngineId = string.Empty;
            OperationLicense = string.Empty;
            Owner = string.Empty;
            ContactPhone = string.Empty;
            CarType = string.Empty;
        }


        #region Validation Func
        private void ValidateSetupStaffIsEmpty()
        {
            var prop = ExtractPropertyName(() => SetupStaff);
            ClearErrors(prop);

            if (string.IsNullOrEmpty(setupStaff))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_CanNotEmpty"));
            }
        }

        private void ValidateCarNumberIsEmpty()
        {
            var prop = ExtractPropertyName(() => CarNumber);
            ClearErrors(prop);

            if (string.IsNullOrEmpty(carNumber))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_CanNotEmpty"));
                IsGetMessage = false;
            }
            else
            {
                IsGetMessage = true;
            }
        }

        private void CheckNextBtnIsEnable()
        {
            if (!string.IsNullOrEmpty(SetupStaff) && resultcode == 1)
            {
                IsFinished = true;
            }
            else
            {
                IsFinished = false;
            }
        }
        #endregion
    }
}
