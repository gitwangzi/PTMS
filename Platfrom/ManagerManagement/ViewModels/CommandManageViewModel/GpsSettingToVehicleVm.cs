
/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 62fbde73-b0a5-424c-a456-9059568e006c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGH
/////                 Author: GJSY(zhangh)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
/////    Project Description:    
/////             Class Name: GpsSettingToVehicleVm
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/21 9:38:59
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/21 9:38:59
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
using Jounce.Core.ViewModel;
using Jounce.Framework.ViewModel;
using Jounce.Core.View;
using System.Collections.Generic;
using Gsafety.PTMS.Share;
using Jounce.Framework.Command;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using System.Collections.ObjectModel;
using Gsafety.PTMS.ServiceReference.MessageService;
using Gsafety.PTMS.BasicPage.VehicleSelect;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.Bases.Enums;
using System.Linq;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.GpsSettingToVechileViewModel)]
    public class GpsSettingToVehicleVm : BaseEntityViewModel
    {
        private CommandManageServiceClient addCommandManageService = null;

         #region property
        public string Title { get; private set; }
        public GpsSettingInfo gpsSettingData { get; set; }
        public string Gps_LoginName { get; set; }
        public string Gps_UpLoadCount { get; set; }
        public string Gps_Distance { get; set; }
        public string Gps_Time { get; set; }
        public string Gps_Comments { get; set; }
        public DateTime? CreateTime { get; set; }
        public string Creator { get; set; }
        public ICommand ReturnCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand SaveAndSettingCommand { get; private set; }
        public List<EnumModel> UploadTypeList { get; set; }
        public List<EnumModel> UploadUsingList { get; set; }
        private bool _Gps_DistanceChecked = false;
        GpsSettingInfo gpsAddInfo = new GpsSettingInfo();
        private VehicleSelectViewModelold _VehicleSelectVM;
        public VehicleSelectViewModelold VehicleSelectVM
        {
            get { return _VehicleSelectVM; }
            set { _VehicleSelectVM = value; }
        }

        ObservableCollection<SelectInfoModel> selectModels;
        public ObservableCollection<SelectInfoModel> SelectModels
        {
            get { return selectModels; }
            set { selectModels = value; }
        }

        private bool _IsSettingToVehicle = false;
        public bool IsSettingToVehcile
        {
            get { return _IsSettingToVehicle; }
            set
            {
                _IsSettingToVehicle = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsSettingToVehcile));
            }
        }

        private string _Gps_IfMonitor;
        public string Gps_IfMonitor
        {
            get
            {
                return _Gps_IfMonitor;
            }
            set
            {
                _Gps_IfMonitor = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_IfMonitor));
            }
        
        }
        private Visibility _UploadVisible;
        public Visibility UploadVisible
        {
            get
            {
                return _UploadVisible;
            }
            set
            {
                _UploadVisible = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UploadVisible));
            }
        }

        private EnumModel _UsingType;
        public EnumModel UsingType
        {
            get
            {
                return _UsingType;
            }
            set
            {
                _UsingType = value;
                if (value.EnumValue == 1)
                {
                    UploadVisible = Visibility.Visible;
                }
                if (value.EnumValue == 0)
                {
                    UploadVisible = Visibility.Collapsed;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UsingType));
            }
        }

        private EnumModel _currentType;
        public EnumModel CurrentType
        {
            get
            {
                return _currentType;
            }
            set
            {
                _currentType = value;              
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
            }
        }

        private bool _RadioButtonyes;
        public bool RadioButtonyes
        {
            get
            {
                return _RadioButtonyes;
            }
            set
            {
                _RadioButtonyes = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RadioButtonyes));
            }
        }

        private bool _RadioButtonno;
        public bool RadioButtonno
        {
            get
            {
                return _RadioButtonno;
            }
            set
            {
                _RadioButtonno = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => RadioButtonno));
            }
        }

        public bool Gps_DistanceChecked
        {
            get { return _Gps_DistanceChecked; }
            set
            {
                _Gps_DistanceChecked = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_DistanceChecked));
            }
        }
        private bool _Gps_TimeChecked = false;
        public bool Gps_TimeChecked
        {
            get { return _Gps_TimeChecked; }
            set
            {
                _Gps_TimeChecked = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_TimeChecked));
            }
        }
       #endregion
        public GpsSettingToVehicleVm()
        {
            try
            {
                addCommandManageService = ServiceClientFactory.Create<CommandManageServiceClient>();
                addCommandManageService.GetAllVehicleRuleRelationCompleted += addCommandManageService_GetAllVehicleRuleRelationCompleted;
                ReturnCommand = new ActionCommand<object>(obj => Return());
                //FinishCommand = new ActionCommand<object>(obj => Finish());
                ResetCommand = new ActionCommand<object>(obj => Reset());
                getUsingUpload();
                getTypeUpload();
                SaveAndSettingCommand = new ActionCommand<object>(obj => SaveAndSetting());
                _VehicleSelectVM = new VehicleSelectViewModelold();
                VehicleSelectVM.InitTree();
                VehicleSelectVM.TreeViewVisible = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void addCommandManageService_GetAllVehicleRuleRelationCompleted(object sender, GetAllVehicleRuleRelationCompletedEventArgs e)
        {
            if (e.Result.IsSuccess)
            {
                if (e.Result.Result != null)
                {
                    VehicleRuleRelation result = e.Result.Result;
                    VehicleSelectVM.InitTree(result.Vehicles);
                }
            }
        }
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                Title = ApplicationContext.Instance.StringResourceReader.GetString("Manager_toVehicle");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                gpsSettingData = viewParameters["ToVehicle"] as Gsafety.PTMS.ServiceReference.CommandManageService.GpsSettingInfo;

                addCommandManageService.GetAllVehicleRuleRelationAsync(gpsSettingData.Gps_RuleID, Gsafety.PTMS.ServiceReference.CommandManageService.RuleType.GpsSend);

                InitialPage(gpsSettingData);
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }

        }
        public void InitialPage(GpsSettingInfo gpsSettingData)
        {
            try
            {
                Gps_LoginName = gpsSettingData.Gps_RuleName;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                Gps_UpLoadCount = gpsSettingData.Gps_UploadSum.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                Gps_Time = gpsSettingData.Gps_Time.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                Gps_Distance = gpsSettingData.Gps_Distance.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                Gps_Comments = gpsSettingData.Gps_Description;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Comments));
                Gps_IfMonitor = gpsSettingData.Gps_IfMonitor.ToString();
                if (gpsSettingData.Gps_UploadType != null)
                {
                    viewType((short)gpsSettingData.Gps_UploadType);
                }
                if (gpsSettingData.Gps_IfMonitor != null)
                {
                    usingType((short)gpsSettingData.Gps_IfMonitor);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
       
        private void Return()
        {
            try
            {
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.GpsSettingView, new Dictionary<string, object>() { { "action", "return" } }));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        //private void Finish()
        //{           
        //    SelectModels = VehicleSelectVM.GetSelectModel();
        //    if (SelectModels.Count != 0)
        //    {               
        //        ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
        //        model.Value = new ObservableCollection<SelectInfoModel>();
        //        model.Value = SelectModels;
        //        model.Setting = new SettingGpsSendUpCMD();
        //        model.Setting.SendTime = DateTime.Now;
        //        model.Setting.RuleName = gpsSettingData.Gps_RuleID;
        //        model.Setting.IsUsing = (int)gpsSettingData.Gps_IfMonitor;
        //        model.Setting.SendNum = string.IsNullOrEmpty(Gps_UpLoadCount) ? model.Setting.SendNum = null : int.Parse(Gps_UpLoadCount);
        //        model.Setting.DistanceValue = string.IsNullOrEmpty(Gps_Distance) ? model.Setting.DistanceValue = null : int.Parse(Gps_Distance);
        //        model.Setting.TimeValue = string.IsNullOrEmpty(Gps_Time) ? model.Setting.DistanceValue = null : int.Parse(Gps_Time);
        //        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
        //        ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
        //        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
        //    }                   
        //}
        private void SaveAndSetting()
        {
            try
            {
                SelectModels = VehicleSelectVM.GetSelectModel();
                if (SelectModels.Count != 0)
                {
                    ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
                    model.Value = new ObservableCollection<SelectInfoModel>();
                    model.Value = SelectModels;
                    model.Setting = new SettingGpsSendUpCMD();
                    model.Setting.SendTime = DateTime.Now;
                    model.Setting.RuleName = gpsSettingData.Gps_RuleID;
                    model.Setting.IsUsing = (int)gpsSettingData.Gps_IfMonitor;
                    model.Setting.SendNum = string.IsNullOrEmpty(Gps_UpLoadCount) ? model.Setting.SendNum = null : int.Parse(Gps_UpLoadCount);
                    model.Setting.DistanceValue = string.IsNullOrEmpty(Gps_Distance) ? model.Setting.DistanceValue = null : int.Parse(Gps_Distance);
                    model.Setting.TimeValue = string.IsNullOrEmpty(Gps_Time) ? model.Setting.DistanceValue = null : int.Parse(Gps_Time);
                    model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                    ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void Reset()
        {
            VehicleSelectVM.InitTree();
            VehicleSelectVM.TreeViewVisible = Visibility.Visible;
        }
        private void getUsingUpload()
        {
            try
            {
                UploadUsingList = new List<EnumModel>();
                foreach (var item in Enum.GetNames(typeof(GpsIfMonitor)))
                {
                    int typeValue = (int)Enum.Parse(typeof(GpsIfMonitor), item, true);
                    UploadUsingList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
                }
                UsingType = UploadUsingList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void getTypeUpload()
        {
            try
            {
                UploadTypeList = new List<EnumModel>();
                foreach (var item in Enum.GetNames(typeof(GpsUploadType)))
                {
                    int typeValue = (int)Enum.Parse(typeof(GpsUploadType), item, true);
                    UploadTypeList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
                }
                CurrentType = UploadTypeList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void viewType(int value)
        {
            CurrentType = UploadTypeList.Where(e => e.EnumValue == value).FirstOrDefault();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
        }
        private void usingType(int value)
        {
            UsingType = UploadUsingList.Where(e => e.EnumValue == value).FirstOrDefault();
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UsingType));
        }
    }
}
