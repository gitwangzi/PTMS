/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e34c7bfc-211c-4581-98d3-a5fead54f3df      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.ViewModels
/////    Project Description:    
/////             Class Name: AlertDetailPageViewModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/17 10:43:36
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/17 10:43:36
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Bases.Enums;
using Jounce.Core.Event;
using Jounce.Core.ViewModel;
using Jounce.Framework.Command;
using System;
using System.ComponentModel.Composition;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Printing;
using System.Windows.Shapes;
using VehicleAlertService = Gsafety.PTMS.ServiceReference.VehicleAlertService;
namespace Gsafety.PTMS.Alert.ViewModels
{
    [ExportAsViewModel(AlertName.AlertDetailPageViewModel)]
    public class AlertDetailPageViewModel : BaseViewModel, IEventSink<OpenState>, IEventSink<VehicleAlertService.VehicleAlertDetail>, IEventSink<int>, IEventSink<VehicleAlertService.VehicleAlertEx>, IPartImportsSatisfiedNotification
    {
        #region init
        public AlertDetailPageViewModel()
        {
            HandleresultVisible = Visibility.Collapsed;
            Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HandleresultVisible));
        }

        public void OnImportsSatisfied()
        {
            EventAggregator.SubscribeOnDispatcher<VehicleAlertService.VehicleAlertDetail>(this);
            EventAggregator.SubscribeOnDispatcher<int>(this);
            EventAggregator.SubscribeOnDispatcher<VehicleAlertService.VehicleAlertEx>(this);
            EventAggregator.SubscribeOnDispatcher<OpenState>(this);
        }
        #endregion

        #region prop

        public string Title { get; set; }

        public string PicUrl { get; set; }

        VehicleAlertService.VehicleAlertDetail _AlertInfo;
        public VehicleAlertService.VehicleAlertDetail AlertInfo
        {
            get { return _AlertInfo; }
        }
        
        private int _SelectItemIndex = 0;
        public int SelectItemIndex
        {
            get { return _SelectItemIndex; }
            set { _SelectItemIndex = value; }
        }

        private bool _HandleSelect;
        public bool HandleSelect
        {
            get { return _HandleSelect; }
            set { _HandleSelect = value; }
        }

        private bool _DetailSelect;
        public bool DetailSelect
        {
            get { return _DetailSelect; }
            set { _DetailSelect = value; }
        }

        VehicleAlertService.VehicleAlertEx _AlertHandle;
        public VehicleAlertService.VehicleAlertEx AlertHandle
        {
            get { return _AlertHandle; }
            set { _AlertHandle = value; }
        }

        private Visibility _HandleresultVisible;
        public Visibility HandleresultVisible
        {
            get { return _HandleresultVisible; }
            set { _HandleresultVisible = value; }
        }

        private Visibility _AlertDetailVisible;
        public Visibility AlertDetailVisible
        {
            get { return _AlertDetailVisible; }
            set { _AlertDetailVisible = value; }
        }

        private string _AlertType;
        public string AlertType
        {
            get { return _AlertType; }
            set { _AlertType = value; }
        }

        /// <summary>
        ///  vehicle type
        /// </summary>
        private string _VehicleType;
        public string VehicleType
        {
            get { return _VehicleType; }
            set { _VehicleType = value; }
        }

        private bool m_IsOpen = true;
        public bool IsOpen
        {
            get { return m_IsOpen; }
            set
            {
                m_IsOpen = value;
            }
        }

        private bool _IsVisual = false;
        public bool IsVisual
        {
            get { return _IsVisual; }
            set
            {
                if (IsOpen)
                {
                    IsOpen = _IsVisual = false;
                }
                else
                {
                    IsOpen = _IsVisual = true;
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => IsVisual));
            }
        }

        #endregion 

        #region handleevent
        public void HandleEvent(VehicleAlertService.VehicleAlertDetail alertinfo)
        {
            try
            {
                _AlertInfo = alertinfo;
                if (AlertInfo.VehicleId == null)
                {
                    Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertInfo));
                    return;
                }
                AlertType = ((BusinessAlertType)AlertInfo.AlertType).ToString();
                if (alertinfo.GpsValid.Equals("V"))
                {
                    alertinfo.GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSERROR");
                }
                if (alertinfo.GpsValid.Equals("A"))
                {
                    alertinfo.GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSAvailable");
                }
                if (alertinfo.GpsValid.Equals("N"))
                {
                    alertinfo.GpsValid = ApplicationContext.Instance.StringResourceReader.GetString("ALERT_GPSNone");
                }
                // alertinfo.GpsValid
                AlertType = ApplicationContext.Instance.StringResourceReader.GetString(AlertType);
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertType));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertInfo));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel", ex);
            }
        }

        public void HandleEvent(int publishedEvent)
        {
            try
            {
                if (publishedEvent == 3)
                {
                    HandleresultVisible = Visibility.Visible;
                    AlertDetailVisible = Visibility.Visible;
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + "(" + ApplicationContext.Instance.StringResourceReader.GetString("ALERT_HandledAlert") + ")";
                    PicUrl = "/ExternalResource;component/Images/MainPage_menu_info.png";
                }
                if (publishedEvent == 2)
                {
                    SelectItemIndex = 0;
                    HandleresultVisible = Visibility.Collapsed;
                    AlertDetailVisible = Visibility.Visible;
                    Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + "(" + ApplicationContext.Instance.StringResourceReader.GetString("ALERT_UnHandleAlert") + ")";
                    PicUrl = "/ExternalResource;component/Images/MainPage_menu_info_orange.png";
                }
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => HandleresultVisible));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertDetailVisible));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => SelectItemIndex));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PicUrl));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel", ex);
            }
        }

        public void HandleEvent(VehicleAlertService.VehicleAlertEx publishedEvent)
        {
            try
            {
                AlertHandle = publishedEvent;
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => AlertHandle));
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("AlertDetailPageViewModel", ex);
            }
        }

        public void HandleEvent(OpenState publishedEvent)
        {
            IsVisual = publishedEvent.State;
            if (string.IsNullOrEmpty(Title))
            {
                Title = ApplicationContext.Instance.StringResourceReader.GetString("MONITOR_BaseInfo") + "(" + ApplicationContext.Instance.StringResourceReader.GetString("ALERT_UnHandleAlert") + ")";
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => Title));
                PicUrl = "/ExternalResource;component/Images/MainPage_menu_info_orange.png";
                Jounce.Framework.JounceHelper.ExecuteOnUI(() => RaisePropertyChanged(() => PicUrl));
            }
        }
        #endregion
    }

    /// <summary>
    ///  Window On-State
    /// </summary>
    public class OpenState
    {
        public bool State
        {
            get;
            set;
        }
    }
}
