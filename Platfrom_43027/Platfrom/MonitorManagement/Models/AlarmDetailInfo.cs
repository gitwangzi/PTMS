using Gsafety.PTMS.Monitor.Models;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 51ea3114-5356-478e-868e-a6a4e7a86184      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Models
/////    Project Description:    
/////             Class Name: AlarmDetailInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 9/15/2013 10:42:03 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/15/2013 10:42:03 AM
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
using VehicleAlarmService = Gsafety.PTMS.ServiceReference.VehicleAlarmService;

namespace Gsafety.PTMS.Monitor.ViewModels
{
    public class AlarmDetailInfo
    {
        #region Fields

        private VehicleAlarmService.AlarmInfoEx _AlarmInfo;
        private AlarmInfoType _AlarmInfoType;

        #endregion

        #region Attributes

        public VehicleAlarmService.AlarmInfoEx AlarmInfo
        {
            get { return _AlarmInfo; }
            set { _AlarmInfo = value; }
        }

        public AlarmInfoType AlarmInfoType
        {
            get { return _AlarmInfoType; }
            set { _AlarmInfoType = value; }
        }
        #endregion
    }
}
