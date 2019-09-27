/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 357a51b4-21eb-4b68-8816-7082519377b6      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm
/////    Project Description:    
/////             Class Name: AlarmBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 7/24/2013 5:23:18 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 7/24/2013 5:23:18 PM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
using System.Windows.Shapes;
using Jounce.Core.ViewModel;


namespace Gsafety.PTMS.Alarm
{
    public class AlarmBinding
    {
        [Export]
        public ViewModelRoute BindingAlarmMain
        {
            get { return ViewModelRoute.Create(AlarmName.AlarmMainViewModel, AlarmName.AlarmMainView); }
        }

        [Export]
        public ViewModelRoute BindingAlarmMenu
        {
            get { return ViewModelRoute.Create(AlarmName.AlarmMenuViewModel, AlarmName.AlarmMenuView); }
        }
        
       

        [Export]
        public ViewModelRoute BindingALarmInfo
        {
            get { return ViewModelRoute.Create(AlarmName.AlarmInfoViewModle, AlarmName.AlarmInfoView); }
        }  
    }
}
