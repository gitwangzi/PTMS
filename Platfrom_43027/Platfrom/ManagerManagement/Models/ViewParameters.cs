/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4361eaf4-9a55-46b7-92b8-d5d0a6dfc8e7      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager.Models
/////    Project Description:    
/////             Class Name: ViewParms
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/11/4 14:59:54
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/11/4 14:59:54
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

namespace Gsafety.PTMS.Manager.Models
{
    public class ViewParameters
    {
        public ActionType Action { get; set; }
        public object Parameters { get; set; }
        public bool IsRefresh { get; set; }
    }
    public enum ActionType
    {
        Add,
        Edit,
        Delete,
        Detail,
        Back,
        SettingToVehicle
    }
}
