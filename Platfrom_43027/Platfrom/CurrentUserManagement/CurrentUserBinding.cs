using Jounce.Core.ViewModel;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 6a154beb-176a-43c1-ba54-38cb4e398206      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: WANGJINCAI
/////                 Author: TEST(wangjc)
/////======================================================================
/////           Project Name: Gsafety.PTMS.CurrentUserManagement
/////    Project Description:    
/////             Class Name: CurrentUserBinding
/////          Class Version: v1.0.0.0
/////            Create Time: 2013-12-16 11:47:33
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013-12-16 11:47:33
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

namespace Gsafety.PTMS.CurrentUserManagement
{
    public class CurrentUserBinding
    {
        [Export]
        public ViewModelRoute BindingMainPage
        {
            get
            {
                return ViewModelRoute.Create(CurrentUserName.CurrentUserModel, CurrentUserName.CurrentUserView);
            }
        }


        [Export]
        public ViewModelRoute TrafficUserInfo
        {
            get
            {
                return ViewModelRoute.Create(CurrentUserName.TrafficUserInfoModel, CurrentUserName.TrafficUserInfoView);
            }
        }

           [Export]
        public ViewModelRoute CompanyUserInfo
        {
            get
            {
                return ViewModelRoute.Create(CurrentUserName.CompanyUserInfoModel, CurrentUserName.CompanyUserInfoView);
            }
        }
    }
}
