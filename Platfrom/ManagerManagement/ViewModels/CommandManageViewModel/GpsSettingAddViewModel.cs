using Jounce.Core.Command;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Framework;
using Gsafety.PTMS.ServiceReference.MessageService;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Gsafety.PTMS.BasicPage.VehicleSelect;
using Gsafety.PTMS.Bases.Models;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.GpsSettingAddViewModel)]
    public class GpsSettingAddViewModel : BaseEntityViewModel
    {
        CommandManageServiceClient gpsSettingAddClient;
        #region
        public ICommand ReturnCommand { get; private set; }
        public ICommand ResetCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public ICommand DetailCommand { get; private set; }
        public ICommand SaveAndSettingCommand { get; private set; }
        public List<EnumModel> UploadTypeList { get; set; }
        public List<EnumModel> UploadUsingList { get; set; }
        public string Gps_Note { get; set; }
        public string Title { get; private set; }
        GpsSettingInfo gpsAddInfo = new GpsSettingInfo();
        public GpsSettingInfo gpsSettingData { get; set; }
        private bool _IsEditDistance = false;
        public bool IsEditDistance
        {
            get { return _IsEditDistance; }
            set
            {
                _IsEditDistance = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEditDistance));
            }
        }

        private bool _IsEditTime = false;
        public bool IsEditTime
        {
            get { return _IsEditTime; }
            set
            {
                _IsEditTime = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEditTime));
            }
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
                if (value.EnumValue == 0)
                {
                    Gps_TimeEnabled = false;
                    Gps_DistanceEnabled = true;
                    Gps_Time = string.Empty;
                }
                if (value.EnumValue == 1)
                {
                    Gps_DistanceEnabled = false;
                    Gps_TimeEnabled = true;
                    Gps_Distance = string.Empty;
                }
                if (value.EnumValue == 2)
                {
                    Gps_DistanceEnabled = true;
                    Gps_TimeEnabled = true;
                }
              Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
            }
        }

        private EnumModel _UsingType;
        public EnumModel UsingType  // 0:stop  1:start
        {
            get
            {
                return _UsingType;
            }
            set
            {
                _UsingType = value;               
                if (value.EnumValue==1)
                {
                    UploadVisible = Visibility.Visible;                    
                  
                }
                if (value.EnumValue == 0)
                {
                    UploadVisible = Visibility.Collapsed;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => UsingType));
                if (!string.IsNullOrEmpty(Gps_LoginName))
                {
                    if (UsingType.EnumValue == 1)
                    {
                        if (!string.IsNullOrEmpty(Gps_UpLoadCount) && (!string.IsNullOrEmpty(Gps_Distance) || !string.IsNullOrEmpty(Gps_Time)))
                        {
                            finishEnabled = true;
                        }
                        else
                            finishEnabled = false;
                    }
                    if (UsingType.EnumValue == 0)
                    {
                        finishEnabled = true;
                    }
                }
                else
                    finishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
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

        #endregion

        #region   property
        private int index;
        public int TabControlSelectIndex
        {
            get { return index; }
            set { index = value; }
        }

        private string _Gps_LoginName;
        public string Gps_LoginName
        {
            get { return _Gps_LoginName; }
            set
            {
                _Gps_LoginName = value == null ? null : value.Trim();
                ValidateGpsLoginName(ExtractPropertyName(() => Gps_LoginName), _Gps_LoginName);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                if (!string.IsNullOrEmpty(Gps_LoginName))
                {
                    if (UsingType.EnumValue == 1)
                    {
                        if (!string.IsNullOrEmpty(Gps_UpLoadCount) && (!string.IsNullOrEmpty(Gps_Distance) || !string.IsNullOrEmpty(Gps_Time)))
                        {
                            finishEnabled = true;
                        }
                        else
                            finishEnabled = false;
                    }
                    if (UsingType.EnumValue == 0)
                    {
                        finishEnabled = true;
                    }
                }
                else
                    finishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

        private void ValidateGpsLoginName(string prop, string value)
        {
            ClearErrors(prop);
            if (string.IsNullOrEmpty(value))
            {
                SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_MustFilled"));
            }
        }

        ObservableCollection<SelectInfoModel> selectModels;
        public ObservableCollection<SelectInfoModel> SelectModels
        {
            get { return selectModels; }
            set { selectModels = value; }
        }

        private VehicleSelectViewModelold _VehicleSelectVM;
        public VehicleSelectViewModelold VehicleSelectVM
        {
            get { return _VehicleSelectVM; }
            set { _VehicleSelectVM = value; }
        }

        private string _Gps_UpLoadCount;
        public string Gps_UpLoadCount
        {
            get { return _Gps_UpLoadCount; }
            set
            {
                _Gps_UpLoadCount = value;
                ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                if (!string.IsNullOrEmpty(Gps_LoginName))
                {
                    if (UsingType.EnumValue == 1)
                    {
                        if (!string.IsNullOrEmpty(Gps_UpLoadCount) && (!string.IsNullOrEmpty(Gps_Distance) || !string.IsNullOrEmpty(Gps_Time)))
                        {
                            finishEnabled = true;
                        }
                        else
                            finishEnabled = false;
                    }
                    if (UsingType.EnumValue == 0)
                    {
                        finishEnabled = true;
                    }
                }
                else
                    finishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

        private string _Gps_Distance;
        public string Gps_Distance
        {
            get { return _Gps_Distance; }
            set
            {
                _Gps_Distance = value == null ? null : value.Trim();
                if (Gps_DistanceEnabled)
                {
                    ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                if (!string.IsNullOrEmpty(Gps_LoginName))
                {
                    if (UsingType.EnumValue == 1)
                    {
                        if (!string.IsNullOrEmpty(Gps_UpLoadCount) && (!string.IsNullOrEmpty(Gps_Distance) || !string.IsNullOrEmpty(Gps_Time)))
                        {
                            finishEnabled = true;
                        }
                        else
                            finishEnabled = false;
                    }
                    if (UsingType.EnumValue == 0)
                    {
                        finishEnabled = true;
                    }
                }
                else
                    finishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

        private string _Gps_Time;
        public string Gps_Time
        {
            get { return _Gps_Time; }
            set
            {
                _Gps_Time = value == null ? null : value.Trim();
                if (Gps_TimeEnabled)
                {
                    ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                if (!string.IsNullOrEmpty(Gps_LoginName))
                {
                    if (UsingType.EnumValue == 1)
                    {
                        if (!string.IsNullOrEmpty(Gps_UpLoadCount) && (!string.IsNullOrEmpty(Gps_Distance) || !string.IsNullOrEmpty(Gps_Time)))
                        {
                            finishEnabled = true;
                        }
                        else
                            finishEnabled = false;
                    }
                    if (UsingType.EnumValue == 0)
                    {
                        finishEnabled = true;
                    }
                }
                else
                    finishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

        private void ValidateGps(string prop, string value)
        {
            ClearErrors(prop);
            Match match = Regex.Match(value, @"^(0|[1-9]?[0-9]\d*|0)$");
            if (!string.IsNullOrEmpty(value))
            {
                if (!match.Success)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
                }
                if (match.Success)
                if ((int.Parse(match.Value) > 65535 || int.Parse(match.Value) < 0))
                 {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_ValiNumber"));
                 }
           }        
            //ClearErrors(prop);
        }
       

        private bool finishEnabled = false;
        public bool FinishEnabled
        {
            get { return finishEnabled; }
            set
            {
                if (!string.IsNullOrEmpty(Gps_LoginName) )
                {
                    if (UsingType.EnumValue==1)
                    {
                        if (!string.IsNullOrEmpty(Gps_UpLoadCount) && (!string.IsNullOrEmpty(Gps_Distance) || !string.IsNullOrEmpty(Gps_Time)))
                        {
                            finishEnabled = true;
                        }
                        else
                            finishEnabled = false;
                    }
                    if (UsingType.EnumValue == 0)
                    {
                        finishEnabled = true;
                    }
                }
                else
                    finishEnabled = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
        }

        private bool _Gps_DistanceEnabled = false;
        public bool Gps_DistanceEnabled
        {
            get { return _Gps_DistanceEnabled; }
            set
            {
                _Gps_DistanceEnabled = value;
                if (CurrentType.EnumValue == 0)
                {
                    _Gps_DistanceEnabled = true;
                }
              Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_DistanceEnabled));
            }
        }

        private bool _Gps_TimeEnabled = false;
        public bool Gps_TimeEnabled
        {
            get { return _Gps_TimeEnabled; }
            set
            {
                _Gps_TimeEnabled = value;
                if (CurrentType.EnumValue == 1)
                {
                    _Gps_TimeEnabled = true;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_TimeEnabled));
            }
        }     

        public void UpLoadValue()   //0:Distance  1:Time  2:Distance and Time 
        {
            if (!string.IsNullOrEmpty(Gps_Time) && string.IsNullOrEmpty(Gps_Distance))
            {
                gpsAddInfo.Gps_UploadType = 1;
                gpsAddInfo.Gps_Time = short.Parse(Gps_Time);
            }
            if (!string.IsNullOrEmpty(Gps_Distance) && string.IsNullOrEmpty(Gps_Time))
            {
                gpsAddInfo.Gps_UploadType = 0;
                gpsAddInfo.Gps_Distance = short.Parse(Gps_Distance);
            }
            if (!string.IsNullOrEmpty(Gps_Distance) && !string.IsNullOrEmpty(Gps_Time))
            {
                gpsAddInfo.Gps_UploadType = 2;
                gpsAddInfo.Gps_Distance = short.Parse(Gps_Distance);
                gpsAddInfo.Gps_Time = short.Parse(Gps_Time);
            }
        }
        #endregion

        public GpsSettingAddViewModel()
        {
            try
            {
                gpsSettingAddClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                _VehicleSelectVM = new VehicleSelectViewModelold();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                ResetCommand = new ActionCommand<object>(obj => RetSet());
                FinishCommand = new ActionCommand<object>(obj => Finish());
                SaveAndSettingCommand = new ActionCommand<object>(obj => SaveAndSetting());
                IsSettingToVehcile = false;
                getTypeUpload();
                getUsingUpload();
                gpsSettingAddClient.CheckGpsExistCompleted += gpsSettingAddClient_CheckGpsExistCompleted;
                gpsSettingAddClient.GpsSettingAddCompleted += gpsSettingAddClient_GpsSettingAddCompleted;
                ValidateGpsLoginName(ExtractPropertyName(() => Gps_LoginName), _Gps_LoginName);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private string action;
        protected override void ActivateView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            try
            {
                base.ActivateView(viewName, viewParameters);
                action = viewParameters["action"].ToString();
                switch (action)
                {
                    case "Add":
                        Title = ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_GpsSettingAdd");
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_TimeEnabled));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_DistanceEnabled));
                        VehicleSelectVM.InitTree();
                        VehicleSelectVM.TreeViewVisible = Visibility.Visible;
                        IsEditDistance = false;
                        IsEditTime = false;
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEditDistance));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsEditTime));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
                        break;
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        public void InitialPage(GpsSettingInfo rulesInfo)
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
                Gps_Note = gpsSettingData.Gps_Description;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Note));
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
                finishEnabled = false;
                toEmpty();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void RetSet()
        {
            try
            {
                toEmpty();
                VehicleSelectVM.InitTree();
                VehicleSelectVM.FilterText = null;
                VehicleSelectVM.TreeViewVisible = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        bool ValidateNumbers(string prop, string value)
        {
            try
            {
                ClearErrors(prop);
                if (!Regex.Match(value, "^[0-9]*$").Success && !string.IsNullOrEmpty(value))
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private void Finish()
        {
            try
            {
                IsSettingToVehcile = false;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsSettingToVehcile));
                if (action == "Add")
                {
                    if (!string.IsNullOrEmpty(Gps_LoginName))
                    {
                        if (UsingType.EnumValue == 1)
                        {
                            if (CurrentType.EnumValue == 2)
                            {
                                if (ValidateNumbers(ExtractPropertyName(() => Gps_Time), _Gps_Time) && ValidateNumbers(ExtractPropertyName(() => Gps_Distance), _Gps_Distance))
                                {
                                    if (int.Parse(Gps_Time) > 1 && int.Parse(Gps_Time) < 65535 && int.Parse(Gps_Distance) > 1 && int.Parse(Gps_Distance) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount)<65535)
                                    {
                                        gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                                    }
                                    else
                                    {
                                        ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                        ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                        ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            if (CurrentType.EnumValue == 1)
                            {
                                if (ValidateNumbers(ExtractPropertyName(() => Gps_Time), _Gps_Time))
                                {
                                    if (int.Parse(Gps_Time) > 1 && int.Parse(Gps_Time) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                    {
                                        gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                                    }
                                    else
                                    {
                                        ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                        ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                        ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            if (CurrentType.EnumValue == 0)
                            {
                                if (ValidateNumbers(ExtractPropertyName(() => Gps_Distance), _Gps_Distance))
                                {
                                    if (int.Parse(Gps_Distance) > 1 && int.Parse(Gps_Distance) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                    {
                                        gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                                    }
                                    else
                                    {
                                        ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                        ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                        ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                           
                        }
                        if (UsingType.EnumValue == 0)
                        {
                            this.Gps_Time = string.Empty;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                            this.Gps_Distance = string.Empty;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                            this.Gps_UpLoadCount = string.Empty;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));                            
                            gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        private void SaveAndSetting()
        {      
            try
            {              
                IsSettingToVehcile = true;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsSettingToVehcile));
                if (action == "Add")
                {
                    if (!string.IsNullOrEmpty(Gps_LoginName))
                    {
                        if (UsingType.EnumValue == 1)
                        {
                            if (CurrentType.EnumValue == 2)
                            {
                                if (ValidateNumbers(ExtractPropertyName(() => Gps_Time), _Gps_Time) && ValidateNumbers(ExtractPropertyName(() => Gps_Distance), _Gps_Distance))
                                {
                                    if (int.Parse(Gps_Time) > 1 && int.Parse(Gps_Time) < 65535 && int.Parse(Gps_Distance) > 1 && int.Parse(Gps_Distance) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                    {
                                        gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                                    }
                                    else
                                    {
                                        ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                        ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                        ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            if (CurrentType.EnumValue == 1)
                            {
                                if (ValidateNumbers(ExtractPropertyName(() => Gps_Time), _Gps_Time))
                                {
                                    if (int.Parse(Gps_Time) > 1 && int.Parse(Gps_Time) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                    {
                                        gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                                    }
                                    else
                                    {
                                        ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                        ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                        ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }
                            if (CurrentType.EnumValue == 0)
                            {
                                if (ValidateNumbers(ExtractPropertyName(() => Gps_Distance), _Gps_Distance))
                                {
                                    if (int.Parse(Gps_Distance) > 1 && int.Parse(Gps_Distance) < 65535 && int.Parse(Gps_UpLoadCount) > 1 && int.Parse(Gps_UpLoadCount) < 65535)
                                    {
                                        gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                                    }
                                    else
                                    {
                                        ValidateGps(ExtractPropertyName(() => Gps_Time), _Gps_Time);
                                        ValidateGps(ExtractPropertyName(() => Gps_Distance), _Gps_Distance);
                                        ValidateGps(ExtractPropertyName(() => Gps_UpLoadCount), _Gps_UpLoadCount);
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                                        Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("BASEINFO_FormatIllegal"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                                }
                            }

                        }
                        if (UsingType.EnumValue == 0)
                        {
                            this.Gps_Time = string.Empty;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                            this.Gps_Distance = string.Empty;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                            this.Gps_UpLoadCount = string.Empty;
                            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                            gpsSettingAddClient.CheckGpsExistAsync(Gps_LoginName);
                        }
                    }
                    else
                    {
                        MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGER_SettingNameISNULL"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }

        void gpsSettingAddClient_CheckGpsExistCompleted(object sender, CheckGpsExistCompletedEventArgs e)
        {
            try
            {
                if (e.Result.Result==true)
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Rule_Name_IsExist"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
                else
                {
                    if (action == "Add")
                    {
                        if (UsingType.EnumValue==1)
                        {
                            UpLoadValue();
                            gpsAddInfo.Gps_RuleID = Guid.NewGuid().ToString();
                            gpsAddInfo.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                            gpsAddInfo.Gps_RuleName = Gps_LoginName;
                            gpsAddInfo.Gps_UploadSum = int.Parse(Gps_UpLoadCount);
                            gpsAddInfo.Gps_Description = Gps_Note;
                            gpsAddInfo.Gps_IfMonitor = (short)UsingType.EnumValue;
                            gpsSettingAddClient.GpsSettingAddAsync(gpsAddInfo);
                        }
                        else
                        {
                            gpsAddInfo.Gps_RuleID = Guid.NewGuid().ToString();
                            gpsAddInfo.Gps_Creator = ApplicationContext.Instance.AuthenticationInfo.UserName;
                            gpsAddInfo.Gps_RuleName = Gps_LoginName;
                            gpsAddInfo.Gps_IfMonitor = (short)UsingType.EnumValue;
                            gpsAddInfo.Gps_UploadSum = null;
                            gpsAddInfo.Gps_UploadType = null;
                            gpsSettingAddClient.GpsSettingAddAsync(gpsAddInfo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        void gpsSettingAddClient_GpsSettingAddCompleted(object sender, GpsSettingAddCompletedEventArgs e)
        {
            try
            {
                if (e.Result.IsSuccess)
                {
                    this.Gps_DistanceEnabled = false;
                    this.Gps_TimeEnabled = false;
                    SelectModels = VehicleSelectVM.GetSelectModel();
                    if (SelectModels.Count != 0 && IsSettingToVehcile)
                    {
                        ServiceReference.MessageService.GpsSendUpModel model = new ServiceReference.MessageService.GpsSendUpModel();
                        model.Value = new ObservableCollection<SelectInfoModel>();
                        model.Value = SelectModels;
                        model.Setting = new SettingGpsSendUpCMD();
                        model.Setting.SendTime = DateTime.Now;
                        model.OperationType = RuleOperationType.Add;
                        model.Setting.SendNum = string.IsNullOrEmpty(Gps_UpLoadCount) ? model.Setting.SendNum = null : int.Parse(Gps_UpLoadCount);
                        model.Setting.RuleName = gpsAddInfo.Gps_RuleID;
                        model.Setting.IsUsing = (int)gpsAddInfo.Gps_IfMonitor;
                        model.Setting.DistanceValue = string.IsNullOrEmpty(Gps_Distance) ? model.Setting.DistanceValue = null : int.Parse(Gps_Distance);
                        model.Setting.TimeValue = string.IsNullOrEmpty(Gps_Time) ? model.Setting.TimeValue = null : int.Parse(Gps_Time);
                        model.Setting.UserName = ApplicationContext.Instance.AuthenticationInfo.UserName;
                        ApplicationContext.Instance.MessageManager.SendSettingGpsCMD(model);
                        toEmpty();
                    }
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Succeed"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                    finishEnabled = false;
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => FinishEnabled));
                }
                else
                {
                    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("Traffic_Oper_Faild"), ApplicationContext.Instance.StringResourceReader.GetString("TRAFFIC_TIP"), MessageBoxButton.OK);
                }
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }      
        private void toEmpty()
        {
            this.Gps_LoginName = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
            this.Gps_Time = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
            this.Gps_Distance = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
            this.Gps_UpLoadCount = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
            this.Gps_Note = string.Empty;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Note));
        }

        private void getUsingUpload()
        {
            UploadUsingList = new List<EnumModel>();           
            foreach (var item in Enum.GetNames(typeof(GpsIfMonitor)))
            {
                int typeValue = (int)Enum.Parse(typeof(GpsIfMonitor), item, true);
                UploadUsingList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
            }
            UsingType = UploadUsingList.FirstOrDefault();
        }

        private void getTypeUpload()
        {
            UploadTypeList = new List<EnumModel>();            
            foreach (var item in Enum.GetNames(typeof(GpsUploadType)))
            {
                int typeValue = (int)Enum.Parse(typeof(GpsUploadType), item, true);
                UploadTypeList.Add(new EnumModel { EnumName = item, ShowName = ApplicationContext.Instance.StringResourceReader.GetString(item), EnumValue = typeValue });
            }
            CurrentType = UploadTypeList.FirstOrDefault();
        }
    }
}

