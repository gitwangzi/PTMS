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
using System.Windows;
namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallVehicleCheckVm)]
    public class InstallVehicleCheckViewModel : InstallSuiteViewModelBase
    {
        int resultcode = -1;
        //Installers
        private string setupStaff;
        public string SetupStaff
        {
            get { return setupStaff; }
            set
            {
                setupStaff = value == null ? null : value.Trim();
                ValidateSetupStaffIsEmpty();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SetupStaff));
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
                    RaisePropertyChanged("CarNumber");
                }
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

        bool isNoteEnabled = false;
        public bool IsNoteEnabled
        {
            get
            {
                return isNoteEnabled;
            }
            set
            {
                isNoteEnabled = value;
                RaisePropertyChanged("IsNoteEnabled");
            }
        }

        Step1Package package;
        VehicleServiceClient vehicleServiceClient = null;
        public InstallVehicleCheckViewModel()
        {
            try
            {
                vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
                step = 1;
                ImageSource = "Step01.png";
                vehicleServiceClient.CheckInstallVehicleForSuiteCompleted += vehicleServiceClient_CheckInstallVehicleForSuiteCompleted;
                deviceInstallServiceClient.SubmitForStep1Completed += deviceInstallServiceClient_SubmitForStepOneCompleted;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("InstallVehicleCheckViewModel()", ex);
            }
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
            vehicleServiceClient.CheckInstallVehicleForSuiteAsync(carNumber, ApplicationContext.Instance.AuthenticationInfo.ClientID);
        }

        void vehicleServiceClient_CheckInstallVehicleForSuiteCompleted(object sender, CheckInstallVehicleForSuiteCompletedEventArgs e)
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
                        if (e.Result.Result.InvalidCode != 1)
                        {
                            var result = MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_VehicleAlreadyInstalled"), MessageDialogButton.Ok);
                            result.Closed += result_Closed;
                        }
                        else
                        {
                            IsNoteEnabled = true;
                        }

                        CheckNextBtnIsEnable();
                    }
                    else
                    {
                        MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Tip"), ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ValidateNotRight"), MessageDialogButton.Ok);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel CheckInstallVehicleCompleted", ex);
            }
        }

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

                package.Note = Note;

                package.ClientID = ApplicationContext.Instance.AuthenticationInfo.ClientID;

                deviceInstallServiceClient.SubmitForStep1Async(package);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel NextPage", ex);
            }
        }

        void deviceInstallServiceClient_SubmitForStepOneCompleted(object sender, SubmitForStep1CompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    ResetData();
                    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallSuiteCheckV, new Dictionary<string, object>() { { "ID", package.ID } }));
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

                Note = string.Empty;
                CarNumber = string.Empty;
                CarType = string.Empty;
                SetupStaff = string.Empty;
                OrgnizationName = string.Empty;
                VehicleSn = string.Empty;
                EngineId = string.Empty;
                OperationLicense = string.Empty;
                Owner = string.Empty;
                ContactPhone = string.Empty;
                SetupStaff = string.Empty;

                IsFinished = false;
                IsGetMessage = false;
                IsNoteEnabled = false;
                resultcode = -1;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel ResetData", ex);
            }
        }

        private void result_Closed(object sender, EventArgs e)
        {
            SelfMessageBox dialog = sender as SelfMessageBox;
            if (dialog != null)
            {
                if (dialog.DialogResult == true)
                {
                    ResetData();
                }
            }
        }


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
    }
}
