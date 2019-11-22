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
using Jounce.Core.ViewModel;
using Jounce.Core.Command;
using Jounce.Core.Event;
using System.Threading;
using Jounce.Framework.Command;
using Jounce.Core.View;
using Jounce.Framework.ViewModel;
using Gsafety.PTMS.ServiceReference.DeviceInstallService;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using Gsafety.PTMS.ServiceReference.VehicleService;
using System.Reflection;
using Gsafety.PTMS.Enums;
using Gsafety.PTMS.ServiceReference.InstallStationService;

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.InstallVehicleCheckVm)]
    public class InstallVehicleCheckViewModel : InstallViewModelBase
    {
        VehicleServiceClient vehicleServiceClient = null;
        public InstallVehicleCheckViewModel()
        {
            try
            {
                vehicleServiceClient = ServiceClientFactory.Create<VehicleServiceClient>();
                step = 1;
                SetupTime = DateTime.Now;
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
            ResetData();
            Stations = ApplicationContext.Instance.AuthenticationInfo.Stations;
            if (Stations.Count != 0)
            {
                SelectedStation = Stations[0];
                RaisePropertyChanged(() => SelectedStation);
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

                        if (e.Result.Result.InvalidCode == 1)
                        {
                            CarType = e.Result.Result.Type.ToString();
                            IsEnabled = true;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
                        }
                        else
                        {
                            MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_VehicleAlreadyInstalled"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                        }

                    }
                    else
                    {
                        CarType = string.Empty;
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ValidateNotRight"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarType));
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel CheckInstallVehicleCompleted", ex);
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
                ValidateSetupStaffIsEmpty();
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
                }
            }
        }

        //Vehicle Type
        public string CarType { get; set; }

        private bool radioIsChecked = false;
        public bool RadioIsChecked
        {
            get
            {
                return radioIsChecked;
            }
            set
            {
                radioIsChecked = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RadioIsChecked));
                if (value)
                {
                    IsNoteEnabled = false;
                    IsFinished = true;
                    Note = string.Empty;
                }

            }
        }

        private bool isChecked = true;
        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked));
                if (value)
                {
                    IsNoteEnabled = true;
                    IsFinished = false;
                }
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

        private bool? isEnabled = false;
        public bool? IsEnabled
        {
            get
            {
                return this.isEnabled;
            }
            set
            {
                this.isEnabled = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
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
                package.CreateTime = SetupTime;
                /// The current installation steps
                package.Step = 1;

                package.Note = Note;

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
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString(e.Result.Result), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_ServiceError"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel SubmitForStepOneCompleted", ex);
            }
        }

        protected override void Quit()
        {
            var result = MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL_IfCancelInstall"), ApplicationContext.Instance.StringResourceReader.GetString("Tip"), MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                if (!RadioIsChecked)
                {
                    vehicleServiceClient.UpdateVehicleStatusByVehicleIdAsync(CarNumber, 0, Note);
                }
                ResetData();
                EventAggregator.Publish(new ViewNavigationArgs(InstallationName.InstallVehicleCheckV));
            }
        }

        protected override void ResetData()
        {
            try
            {
                Note = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
                SetupTime = DateTime.Now;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SetupTime));
                CarNumber = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarNumber));
                CarType = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CarType));
                SetupStaff = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SetupStaff));
                IsEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEnabled));
                RadioIsChecked = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RadioIsChecked));
                IsFinished = false;
                IsChecked = true;
                IsNoteEnabled = true;
                IsGetMessage = false;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("DeviceInstallViewModel ResetData", ex);
            }
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
        #endregion
    }
}
