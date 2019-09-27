/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c81d0840-e242-4ca5-898f-da2b34caa247      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage
/////    Project Description:    
/////             Class Name: MainPageBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 8/5/2013 1:23:47 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/5/2013 1:23:47 PM
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
using System.ComponentModel.Composition;
using Jounce.Core.ViewModel;

namespace Gsafety.PTMS.OrderClientManagement
{
    public class OrderClientBinding
    {

        [Export]
        public ViewModelRoute OrderClientInfo
        {
            get
            {
                return ViewModelRoute.Create(OrderClientName.OrderClientInfoVm, OrderClientName.OrderClientInfoV);
            }
        }
    }
}
