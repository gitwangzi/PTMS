/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: a7b2c988-85ff-4447-99a1-642acceef4f9      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: ApplicationContext
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:33:48 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:33:48 PM
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
using Gsafety.Common.Localization;
using Jounce.Core.Model;
using ESRI.ArcGIS.Client;
using System.ComponentModel.Composition;
using Jounce.Core.Event;
using Gsafety.PTMS.Share.MessageManage;
using Gsafety.PTMS.ServiceReference.OrganizationService;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gsafety.PTMS.Share
{
    public class ApplicationContext : BaseNotify
    {
        #region Fields

        private static ApplicationContext instance;
        private NavigateManage _NavigateManager;
        private MenuManager _MenuManager;
        private StringResourceReader _StringResourceReader;
        private ServerConfigInfo _ServerConfig;
        private AuthenticationInfo _AuthenticationInfo = new AuthenticationInfo();
        private ObservableCollection<Organization> _vehicleDepartmentList = new ObservableCollection<Organization>();
        private BufferManage _BufferManager;
        private AntLogger _AntLogger = new AntLogger();
        private BusyInfo _BusyInfo;

        #endregion

        #region Attributes

        public static ApplicationContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ApplicationContext();
                }
                return instance;
            }
        }

        public BusyInfo BusyInfo
        {
            get
            {
                if (_BusyInfo == null)
                    _BusyInfo = new BusyInfo();
                return _BusyInfo;
            }
            set
            {
                _BusyInfo = value;
                RaisePropertyChanged(() => BusyInfo);
            }
        }

        [Import]
        public IEventAggregator EventAggregator { get; set; }


        public string LoginLogId { get; set; }

        private int _currentView;
        public int CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                if (DeActiveViewCallback != null)
                {
                    DeActiveViewCallback(_currentView);
                }
                _currentView = value;
            }
        }


        public Action<int> DeActiveViewCallback { get; set; }

        private string _currentGISViewName = "";
        public string CurrentGISName
        {
            get { return _currentGISViewName; }
            set { _currentGISViewName = value; }
        }

        private ESRI.ArcGIS.Client.Geometry.Geometry _currentDrawArgs;
        public ESRI.ArcGIS.Client.Geometry.Geometry CurrentDrawArgs
        {
            get { return _currentDrawArgs; }
            set { _currentDrawArgs = value; }
        }

        public StringResourceReader StringResourceReader
        {
            get
            {
                if (_StringResourceReader == null)
                    _StringResourceReader = new StringResourceReader();
                return _StringResourceReader;
            }
        }

        public MenuManager MenuManager
        {
            get
            {
                if (_MenuManager == null)
                    _MenuManager = new MenuManager();
                return _MenuManager;
            }
        }


        public NavigateManage NavigateManager
        {
            get
            {
                if (_NavigateManager == null)
                    _NavigateManager = new NavigateManage();
                return _NavigateManager;
            }
        }

        public AuthenticationInfo AuthenticationInfo
        {
            get { return _AuthenticationInfo; }
            set { _AuthenticationInfo = value; }
        }



        public ObservableCollection<Organization> VehicleDepartmentList
        {
            get { return _vehicleDepartmentList; }
            set
            {
                _vehicleDepartmentList = value;             
            }
        }
        public ServerConfigInfo ServerConfig
        {
            get
            {
                if (_ServerConfig == null)
                    _ServerConfig = new ServerConfigInfo();
                return _ServerConfig;
            }
        }

        MessageCenterClient _messageclient;
        public MessageCenterClient MessageClient
        {
            get
            {
                if (_messageclient == null)
                {
                    _messageclient = new MessageCenterClient();
                }
                return _messageclient;
            }
        }

        public BufferManage BufferManager
        {
            get
            {
                if (_BufferManager == null)
                    _BufferManager = new BufferManage();
                return _BufferManager;
            }
        }

        public AntLogger Logger
        {
            get { return _AntLogger; }
        }

        public System.Globalization.CultureInfo SystemCutureInfo
        {
            get { return System.Threading.Thread.CurrentThread.CurrentUICulture; }
        }


        #endregion

        public ApplicationContext()
        {
            CompositionInitializer.SatisfyImports(this);

            Gsafety.PTMS.Media.Common.Loggers.LoggerInstance.Logger = new MediaPlayerLogger();
        }
    }
}
