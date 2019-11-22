/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 0380c439-7534-4e74-ad0e-0a70d189a230      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: BufferManage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/2/2013 11:55:35 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/2/2013 11:55:35 AM
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

namespace Gsafety.PTMS.Share
{
    public class BufferManage
    {
        public void DataLoading()
        {
            _DistrictManager.DataLoading();

            _AlertConfigManager.DataLoading();
        }

        DistrictManage _DistrictManager = new DistrictManage();
        public DistrictManage DistrictManager
        {
            get
            {
                if (_DistrictManager == null)
                    _DistrictManager = new DistrictManage();
                return _DistrictManager;
            }
        }

        AlarmManage _AlarmManager = new AlarmManage();
        public AlarmManage AlarmManager
        {
            get
            {
                return _AlarmManager;
            }
        }

        VehicleAlertManage _VehicleAlertManager = new VehicleAlertManage();
        public VehicleAlertManage VehicleAlertManager
        {
            get
            {
                return _VehicleAlertManager;
            }
        }

        AlertConfigManage _AlertConfigManager = new AlertConfigManage();
        public AlertConfigManage AlertConfigManager
        {
            get
            {
                return _AlertConfigManager;
            }
        }

        VehicleDeviceAlertManage _VehicleDeviceAlertManager = new Share.VehicleDeviceAlertManage();
        public VehicleDeviceAlertManage VehicleDeviceAlertManage
        {
            get
            {
                return _VehicleDeviceAlertManager;
            }
        }

        private VehicleOrganizationManage _vehicleOrganizationManage = new VehicleOrganizationManage();
        public VehicleOrganizationManage VehicleOrganizationManage
        {
            get
            {
                return _vehicleOrganizationManage;
            }
        }


        private MonitorGroupManager _MonitorGroupManager = new MonitorGroupManager();
        public MonitorGroupManager MonitorGroupManager
        {
            get
            {
                return _MonitorGroupManager;
            }
        }
    }
}
