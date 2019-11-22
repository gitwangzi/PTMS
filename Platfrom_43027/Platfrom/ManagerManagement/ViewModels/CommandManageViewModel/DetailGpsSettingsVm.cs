using Gsafety.PTMS.Bases.Enums;
using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.ServiceReference.CommandManageService;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using Jounce.Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Reflection;

namespace Gsafety.PTMS.Manager.ViewModels.CommandManageViewModel
{
    [ExportAsViewModel(ManagerName.DetailGpsSettingsViewModel)]
    public class DetailGpsSettingsVm : BaseEntityViewModel
    {
        CommandManageServiceClient gpsSettingAddClient;
        #region
        public ICommand ReturnCommand { get; private set; }
        public ICommand FinishCommand { get; private set; }
        public List<EnumModel> UploadTypeList { get; set; }
        public List<EnumModel> UploadUsingList { get; set; }
        public string Gps_Comments { get; set; }
        public string Title { get; private set; }
        public DateTime? CreateTime { get; set; }
        public string Creator { get; set; }
        GpsSettingInfo gpsAddInfo = new GpsSettingInfo();
        public GpsSettingInfo gpsSettingData { get; set; }

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
                }
                if (value.EnumValue == 1)
                {
                    Gps_DistanceEnabled = false;
                    Gps_TimeEnabled = true;
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
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
        private string _Gps_UpLoadCount;
        public string Gps_UpLoadCount
        {
            get { return _Gps_UpLoadCount; }
            set
            {
                _Gps_UpLoadCount = value;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
            }
        }

        private string _Gps_Distance;
        public string Gps_Distance
        {
            get { return _Gps_Distance; }
            set
            {
                _Gps_Distance = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
            }
        }

        private string _Gps_Time;
        public string Gps_Time
        {
            get { return _Gps_Time; }
            set
            {
                _Gps_Time = value == null ? null : value.Trim();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
            }
        }
        void ValidateNumbers(string prop, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                ClearErrors(prop);
                if (!Regex.Match(value, "^[0-9]*$").Success)
                {
                    SetError(prop, ApplicationContext.Instance.StringResourceReader.GetString("MANAGE_Number"));
                }
            }
        }
        private bool _Gps_DistanceEnabled = false;
        public bool Gps_DistanceEnabled
        {
            get { return _Gps_DistanceEnabled; }
            set
            {
                _Gps_DistanceEnabled = value;
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
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_TimeEnabled));
            }
        }

        #endregion

        public DetailGpsSettingsVm()
        {
            try
            {
                gpsSettingAddClient = ServiceClientFactory.Create<CommandManageServiceClient>();
                ReturnCommand = new ActionCommand<object>(obj => Return());
                getTypeUpload();
                getUsingUpload();
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
                Title = ApplicationContext.Instance.StringResourceReader.GetString("Look");
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                gpsSettingData = viewParameters["Detail"] as Gsafety.PTMS.ServiceReference.CommandManageService.GpsSettingInfo;
                InitialPage(gpsSettingData);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CurrentType));
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
                Gps_LoginName = rulesInfo.Gps_RuleName;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                Gps_UpLoadCount = rulesInfo.Gps_UploadSum.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                Gps_Time = rulesInfo.Gps_Time.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                Gps_Distance = rulesInfo.Gps_Distance.ToString();
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                Gps_Comments = rulesInfo.Gps_Description;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Comments));
                CreateTime = rulesInfo.Gps_CreateTime;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => CreateTime));
                Creator = rulesInfo.Gps_Creator;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Creator));
                if (rulesInfo.Gps_UploadType != null)
                {
                    viewType((short)rulesInfo.Gps_UploadType);
                }
                usingType((short)rulesInfo.Gps_IfMonitor);
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
                EventAggregator.Publish(new ViewNavigationArgs(ManagerName.GpsSettingView, new Dictionary<string, object>() { }));
                toEmpty();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private void toEmpty()
        {
            try
            {
                this.Gps_LoginName = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_LoginName));
                this.Gps_Time = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Time));
                this.Gps_Distance = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Distance));
                this.Gps_UpLoadCount = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_UpLoadCount));
                this.Gps_Comments = string.Empty;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Gps_Comments));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
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




