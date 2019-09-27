using Gsafety.PTMS.Installation.Views;
using Gsafety.PTMS.ServiceReference.MaitenanceRecordService;
using Gsafety.PTMS.ServiceReference.SecuritySuiteService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: ddc8211b-d9e4-42a3-a5c1-f2c470bff9e5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: LIN-20130409ZRS
/////                 Author: TEST(zhujf)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Installation.ViewModels
/////    Project Description:    
/////             Class Name: SubstitutionMaintenanceVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/3 17:54:49
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/3 17:54:49
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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

namespace Gsafety.PTMS.Installation.ViewModels
{
    [ExportAsViewModel(InstallationName.SubstitutionMaintenanceVm)]
    public class SubstitutionMaintenanceVm : BaseEntityViewModel
    {
        private MaintenanceRecordServiceClient maintenaceClient;
        private SecuritySuiteServiceClient securitySuiteClient;

        public Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuite CurrentDeviceSuite;
        public Gsafety.PTMS.ServiceReference.MaitenanceRecordService.DeviceSuite NewDeviceSuite;

        private bool IsSubmited = false;

        public ICommand ReturnCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
   
        public bool IsEnable1 { get; private set; }

        public SuiteMaintenance CurrentMaintainingRecord { get; private set; }

        #region IsChecked
        private bool _IsChecked1 = false;
        public bool IsChecked1
        {
            get { return _IsChecked1; }
            set 
            {
                _IsChecked1 = value;
                if (_IsChecked1) { IsReadOnly1 = false; } else { IsReadOnly1 = true; if (CameraAfter1 != null) CameraAfter1 = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked1));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CameraAfter1));
            }
        }

        private bool _IsChecked2 = false;
        public bool IsChecked2
        {
            get { return _IsChecked2; }
            set 
            {
                _IsChecked2 = value;
                if (_IsChecked2) { IsReadOnly2 = false; } else { IsReadOnly2 = true; if (CameraAfter2 != null) CameraAfter2 = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked2));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CameraAfter2));
            }
        }

        private bool _IsChecked3 = false;
        public bool IsChecked3
        {
            get { return _IsChecked3; }
            set
            {
                _IsChecked3 = value;
                if (_IsChecked3) { IsReadOnly3 = false; } else { IsReadOnly3 = true; if (AlarmButtonAfter1 != null) AlarmButtonAfter1 = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked3));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmButtonAfter1));
            }
        }

        private bool _IsChecked4 = false;
        public bool IsChecked4
        {
            get { return _IsChecked4; }
            set
            {
                _IsChecked4 = value;
                if (_IsChecked4) { IsReadOnly4 = false; } else { IsReadOnly4 = true; if (AlarmButtonAfter2 != null) AlarmButtonAfter2 = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked4));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmButtonAfter2));
            }
        }

        private bool _IsChecked5 = false;
        public bool IsChecked5
        {
            get { return _IsChecked5; }
            set
            {
                _IsChecked5 = value;
                if (_IsChecked5) { IsReadOnly5 = false; } else { IsReadOnly5 = true; if (AlarmButtonAfter3 != null) AlarmButtonAfter3 = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked5));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmButtonAfter3));
            }
        }

        private bool _IsChecked6 = false;
        public bool IsChecked6
        {
            get { return _IsChecked6; }
            set
            {
                _IsChecked6 = value;
                if (_IsChecked6) { IsReadOnly6 = false; } else { IsReadOnly6 = true; if (DoorSwitchSensorSNAfter != null) DoorSwitchSensorSNAfter = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked6));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DoorSwitchSensorSNAfter));
            }
        }

        private bool _IsChecked7 = false;
        public bool IsChecked7
        {
            get { return _IsChecked7; }
            set
            {
                _IsChecked7 = value;
                if (_IsChecked7) { IsReadOnly7 = false; } else { IsReadOnly7 = true; if (UPSAfter != null) UPSAfter = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked7));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UPSAfter));
            }
        }

        private bool _IsChecked8 = false;
        public bool IsChecked8
        {
            get { return _IsChecked8; }
            set
            {
                _IsChecked8 = value;
                if (_IsChecked8) { IsReadOnly8 = false; } else { IsReadOnly8 = true; if (SdCardAfter != null) SdCardAfter = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked8));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SdCardAfter));
            }
        }

        private bool _IsChecked9 = false;
        public bool IsChecked9
        {
            get { return _IsChecked9; }
            set
            {
                _IsChecked9 = value;
                if (_IsChecked9) { IsReadOnly9 = false; } else { IsReadOnly9 = true; if (MDVR_SNAfter != null) MDVR_SNAfter = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked9));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MDVR_SNAfter));
            }
        }

        private bool _IsChecked10 = false;
        public bool IsChecked10
        {
            get { return _IsChecked10; }
            set
            {
                _IsChecked10 = value;
                if (_IsChecked10) { IsReadOnly10 = false; } else { IsReadOnly10 = true; if (MDVR_CoreSNAfter != null) MDVR_CoreSNAfter = null; }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsChecked10));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MDVR_CoreSNAfter));
            }
        }

        #endregion
        private string _VehicleId;

        public string VehicleId
        {
            get { return _VehicleId; }
            set
            {
                _VehicleId = value;
            }
        }

        private string _Maintainer;

        public string Maintainer
        {
            get { return _Maintainer; }
            set
            {
                _Maintainer = value;
            }
        }

        private string _SuiteId;

        public string SuiteId
        {
            get { return _SuiteId; }
            set
            {
                _SuiteId = value;
            }
        }


        private string _Note;
        public string Note
        {
            get { return _Note; }
            set
            {
                _Note = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Note));
            }
        }
        #region Before modification equipment
        private string _CameraBefore1;
        public string CameraBefore1
        {
            get { return _CameraBefore1; }
            set
            {
                _CameraBefore1 = value;
            }
        }

        private string _CameraBefore2;
        public string CameraBefore2
        {
            get { return _CameraBefore2; }
            set
            {
                _CameraBefore2 = value;
            }
        }

        private string _AlarmButtonBefore1;
        public string AlarmButtonBefore1
        {
            get { return _AlarmButtonBefore1; }
            set
            {
                _AlarmButtonBefore1 = value;
            }
        }

        private string _AlarmButtonBefore2;
        public string AlarmButtonBefore2
        {
            get { return _AlarmButtonBefore2; }
            set
            {
                _AlarmButtonBefore2 = value;
            }
        }

        private string _AlarmButtonBefore3;
        public string AlarmButtonBefore3
        {
            get { return _AlarmButtonBefore3; }
            set
            {
                _AlarmButtonBefore3 = value;
            }
        }

        private string _InfraredSensorBefore;
        public string DoorSwitchSensorSNBefore
        {
            get { return _InfraredSensorBefore; }
            set
            {
                _InfraredSensorBefore = value;
            }
        }

        private string _UPSBefore;
        public string UPSBefore
        {
            get { return _UPSBefore; }
            set
            {
                _UPSBefore = value;
            }
        }

        private string _SdCardBefore;
        public string SdCardBefore
        {
            get { return _SdCardBefore; }
            set
            {
                _SdCardBefore = value;
            }
        }

        private string _MDVR_SNBefore;
        public string MDVR_SNBefore
        {
            get { return _MDVR_SNBefore; }
            set
            {
                _MDVR_SNBefore = value;
            }
        }

        private string _MDVR_CoreSNBefore;
        public string MDVR_CoreSNBefore
        {
            get { return _MDVR_CoreSNBefore; }
            set
            {
                _MDVR_CoreSNBefore = value;
            }
        }
        #endregion

        #region The modified device
        private string _CameraAfter1;
        public string CameraAfter1
        {
            get { return _CameraAfter1; }
            set
            {
                _CameraAfter1 = value;
            }
        }

        private string _CameraAfter2;
        public string CameraAfter2
        {
            get { return _CameraAfter2; }
            set
            {
                _CameraAfter2 = value;
            }
        }

        private string _AlarmButtonAfter1;
        public string AlarmButtonAfter1
        {
            get { return _AlarmButtonAfter1; }
            set
            {
                _AlarmButtonAfter1 = value;
            }
        }

        private string _AlarmButtonAfter2;
        public string AlarmButtonAfter2
        {
            get { return _AlarmButtonAfter2; }
            set
            {
                _AlarmButtonAfter2 = value;
            }
        }

        private string _AlarmButtonAfter3;
        public string AlarmButtonAfter3
        {
            get { return _AlarmButtonAfter3; }
            set
            {
                _AlarmButtonAfter3 = value;
            }
        }

        private string _ANTInfraredSensorAfter;
        public string DoorSwitchSensorSNAfter
        {
            get { return _ANTInfraredSensorAfter; }
            set
            {
                _ANTInfraredSensorAfter = value;
            }
        }

        private string _UPSAfter;
        public string UPSAfter
        {
            get { return _UPSAfter; }
            set
            {
                _UPSAfter = value;
            }
        }

        private string _SdCardAfter;
        public string SdCardAfter
        {
            get { return _SdCardAfter; }
            set
            {
                _SdCardAfter = value;
            }
        }

        private string _MDVR_SNAfter;
        public string MDVR_SNAfter
        {
            get { return _MDVR_SNAfter; }
            set
            {
                _MDVR_SNAfter = value;
            }
        }

        private string _MDVR_CoreSNAfter;
        public string MDVR_CoreSNAfter
        {
            get { return _MDVR_CoreSNAfter; }
            set
            {
                _MDVR_CoreSNAfter = value;
            }
        }

        #endregion


         #region IsReadonly
         private bool _IsReadOnly1 = true;
         public bool IsReadOnly1
         {
             get { return _IsReadOnly1; }
             set
             {
                 _IsReadOnly1 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly1));
             }
         }
         private bool _IsReadOnly2 = true;
         public bool IsReadOnly2
         {
             get { return _IsReadOnly2; }
             set
             {
                 _IsReadOnly2 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly2));
             }
         }
         private bool _IsReadOnly3 = true;
         public bool IsReadOnly3
         {
             get { return _IsReadOnly3; }
             set
             {
                 _IsReadOnly3 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly3));
             }
         }
         private bool _IsReadOnly4 = true;
         public bool IsReadOnly4
         {
             get { return _IsReadOnly4; }
             set
             {
                 _IsReadOnly4 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly4));
             }
         }
         private bool _IsReadOnly5 = true;
         public bool IsReadOnly5
         {
             get { return _IsReadOnly5; }
             set
             {
                 _IsReadOnly5 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly5));
             }
         }
         private bool _IsReadOnly6 = true;
         public bool IsReadOnly6
         {
             get { return _IsReadOnly6; }
             set
             {
                 _IsReadOnly6 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly6));
             }
         }
         private bool _IsReadOnly7 = true;
         public bool IsReadOnly7
         {
             get { return _IsReadOnly7; }
             set
             {
                 _IsReadOnly7 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly7));
             }
         }
         private bool _IsReadOnly8 = true;
         public bool IsReadOnly8
         {
             get { return _IsReadOnly8; }
             set
             {
                 _IsReadOnly8 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly8));
             }
         }
         private bool _IsReadOnly9 = true;
         public bool IsReadOnly9
         {
             get { return _IsReadOnly9; }
             set
             {
                 _IsReadOnly9 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly9));
             }
         }
         private bool _IsReadOnly10 = true;
         public bool IsReadOnly10
         {
             get { return _IsReadOnly10; }
             set
             {
                 _IsReadOnly10 = value;
                 Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsReadOnly10));
             }
         }

         #endregion
         protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            base.ActivateView(viewName, viewParameters);
            CurrentMaintainingRecord = viewParameters["key"] as Gsafety.PTMS.ServiceReference.MaitenanceRecordService.SuiteMaintenance;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentMaintainingRecord));
            VehicleId = CurrentMaintainingRecord.VehicleID;
            Maintainer = CurrentMaintainingRecord.Maintainer;
            SuiteId = CurrentMaintainingRecord.SuiteId;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => VehicleId));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Maintainer));
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SuiteId));
            Note = null;
            IsChecked1 = false;
            IsChecked2 = false;
            IsChecked3 = false;
            IsChecked4 = false;
            IsChecked5 = false;
            IsChecked6 = false;
            IsChecked7 = false;
            IsChecked8 = false;
            IsChecked9 = false;
            IsChecked10 = false;

            securitySuiteClient.GetSecuritySuiteByIDAsync(CurrentMaintainingRecord.OldSuite_Id);
        }


        public SubstitutionMaintenanceVm()
        {
            try
            {
                maintenaceClient = ServiceClientFactory.Create<MaintenanceRecordServiceClient>();
                securitySuiteClient = ServiceClientFactory.Create<SecuritySuiteServiceClient>();
                securitySuiteClient.GetSecuritySuiteByIDCompleted += securitySuiteClient_GetSecuritySuiteByIDCompleted;
                maintenaceClient.SubstitutionMaintenanceCompleted += maintenaceClient_SubstitutionMaintenanceCompleted;
              
                ReturnCommand = new ActionCommand<object>(obj => Return());
                SubmitCommand = new ActionCommand<object>(obj => Submit());
            }
            catch
            {

            }
        }

        void maintenaceClient_SubstitutionMaintenanceCompleted(object sender, SubstitutionMaintenanceCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Installation_SubstitutionMaintanceFailed"));
                return;
            }
            if (e.Result.Result)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__SubstutionSucess"));
            }
            else
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__SubstutionFailed"));
            }

            Return();

            ApplicationContext.Instance.MessageManager.SendDeviceMaintainMessage(new Gsafety.PTMS.ServiceReference.MessageService.DeviceMaintain() { MaintainTime = DateTime.Now, MdvrCoreId = CurrentDeviceSuite.MdvrCoreId });
        }
    
        private void Return()
        {
            EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SuiteMaintainingV));
        }

        private void Submit()
        {
            CurrentMaintainingRecord.Note = Note;
            CurrentMaintainingRecord.Status = RepairStatusType.Complete;
            CurrentMaintainingRecord.RepairType = RepairType.Substitution;
            CurrentMaintainingRecord.Station_Id = ApplicationContext.Instance.AuthenticationInfo.OrgCode;

            IsSubmited = true;
            securitySuiteClient.GetSecuritySuiteByIDAsync(CurrentMaintainingRecord.OldSuite_Id);
           

        }

        void securitySuiteClient_GetSecuritySuiteByIDCompleted(object sender, GetSecuritySuiteByIDCompletedEventArgs e)
        {
            CurrentDeviceSuite = e.Result.Result;

            if (IsSubmited)
            {
                bool flag = false;
                if (CameraAfter1 != null) { CurrentDeviceSuite.Camera1Id = CameraAfter1; flag = true; }
                if (CameraAfter2 != null) { CurrentDeviceSuite.Camera2Id = CameraAfter2; flag = true; }                
                if (AlarmButtonAfter1 != null) { CurrentDeviceSuite.AlarmButton1Id = AlarmButtonAfter1; flag = true; }
                if (AlarmButtonAfter2 != null) { CurrentDeviceSuite.AlarmButton2Id = AlarmButtonAfter2; flag = true; }
                if (AlarmButtonAfter3 != null) { CurrentDeviceSuite.AlarmButton3Id = AlarmButtonAfter3; flag = true; }
                if (DoorSwitchSensorSNAfter != null) { CurrentDeviceSuite.DoorSensorId = DoorSwitchSensorSNAfter; flag = true; }
                if (UPSAfter != null) { CurrentDeviceSuite.UpsId = UPSAfter; flag = true; }
                if (SdCardAfter != null) { CurrentDeviceSuite.SdCardId = SdCardAfter; flag = true; }
                if (MDVR_SNAfter != null) { CurrentDeviceSuite.MdvrId = MDVR_SNAfter; flag = true; }
                if (MDVR_CoreSNAfter != null) { CurrentDeviceSuite.MdvrCoreId = MDVR_CoreSNAfter; flag = true; }

                bool flag1 = true;
                if ((IsChecked1 && CameraAfter1 == null) || (IsChecked2 && CameraAfter2 == null) || (IsChecked3 && AlarmButtonAfter1 == null) ||
                   (IsChecked4 && AlarmButtonAfter2 == null) || (IsChecked4 && AlarmButtonAfter3 == null) || (IsChecked6 && DoorSwitchSensorSNAfter == null) ||
                   (IsChecked7 && UPSAfter == null) || (IsChecked8 && SdCardAfter == null) ||(IsChecked9 && MDVR_SNAfter==null) ||(IsChecked10 && MDVR_CoreSNAfter==null))
                    flag1 = false;

                if (flag&&flag1)
                {
                    NewDeviceSuite = new Gsafety.PTMS.ServiceReference.MaitenanceRecordService.DeviceSuite();
                    CopyDeviceSuite(CurrentDeviceSuite, NewDeviceSuite);
                    maintenaceClient.SubstitutionMaintenanceAsync(CurrentMaintainingRecord, VehicleId, 99, NewDeviceSuite);
                    EventAggregator.Publish(new ViewNavigationArgs(InstallationName.SuiteMaintainingV));
                }
                else
                {

                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("ID_INSTALL__SubstutionFailedInputAgain"));
                }
               
                IsSubmited = false;
            }
            else
            {
                CameraBefore1 = CurrentDeviceSuite.Camera1Id;
                CameraBefore2 = CurrentDeviceSuite.Camera2Id;
                AlarmButtonBefore1 = CurrentDeviceSuite.AlarmButton1Id;
                AlarmButtonBefore2 = CurrentDeviceSuite.AlarmButton2Id;
                AlarmButtonBefore3 = CurrentDeviceSuite.AlarmButton3Id;
                DoorSwitchSensorSNBefore = CurrentDeviceSuite.DoorSensorId;
                UPSBefore = CurrentDeviceSuite.UpsId;
                SdCardBefore = CurrentDeviceSuite.SdCardId;
                MDVR_SNBefore = CurrentDeviceSuite.MdvrId;
                MDVR_CoreSNBefore = CurrentDeviceSuite.MdvrCoreId;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CameraBefore1));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CameraBefore2));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmButtonBefore1));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmButtonBefore2));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlarmButtonBefore3));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => DoorSwitchSensorSNBefore));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UPSBefore));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SdCardBefore));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MDVR_SNBefore));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => MDVR_CoreSNBefore));
            }
        }

        private void CopyDeviceSuite(Gsafety.PTMS.ServiceReference.SecuritySuiteService.DeviceSuite CurrentDeviceSuite,
                                 Gsafety.PTMS.ServiceReference.MaitenanceRecordService.DeviceSuite NewDeviceSuite)
        {        
            NewDeviceSuite.AlarmButton1Id = CurrentDeviceSuite.AlarmButton1Id;
            NewDeviceSuite.AlarmButton2Id = CurrentDeviceSuite.AlarmButton2Id;
            NewDeviceSuite.AlarmButton3Id = CurrentDeviceSuite.AlarmButton3Id;

            NewDeviceSuite.Camera1Id = CurrentDeviceSuite.Camera1Id;
            NewDeviceSuite.Camera2Id = CurrentDeviceSuite.Camera2Id;
            NewDeviceSuite.DeviceType = (Gsafety.PTMS.ServiceReference.MaitenanceRecordService.VehicleType)CurrentDeviceSuite.DeviceType;
            NewDeviceSuite.DoorSensorId = CurrentDeviceSuite.DoorSensorId;

            NewDeviceSuite.MdvrCoreId = CurrentDeviceSuite.MdvrCoreId;
            NewDeviceSuite.MdvrId = CurrentDeviceSuite.MdvrId;
            NewDeviceSuite.MdvrSimId = CurrentDeviceSuite.MdvrSimId;
            NewDeviceSuite.MdvrSimPhoneNumber = CurrentDeviceSuite.MdvrSimPhoneNumber;
            NewDeviceSuite.Note = CurrentDeviceSuite.Note;
            NewDeviceSuite.SdCardId = CurrentDeviceSuite.SdCardId;
            NewDeviceSuite.SoftwareVersion = CurrentDeviceSuite.SoftwareVersion;
            NewDeviceSuite.status = (Gsafety.PTMS.ServiceReference.MaitenanceRecordService.DeviceSuiteStatus)CurrentDeviceSuite.status;
            NewDeviceSuite.SuiteId = CurrentDeviceSuite.SuiteId;
            NewDeviceSuite.UpsId = CurrentDeviceSuite.UpsId;
            NewDeviceSuite.InstallStatus = Gsafety.PTMS.ServiceReference.MaitenanceRecordService.InstallStatusType.Installed;
            
        }
    }
}
