/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1f095346-cbbc-4f84-ab29-6b444f6f5009      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Views
/////    Project Description:    
/////             Class Name: UserInformation
/////          Class Version: v1.0.0.0
/////            Create Time: 11/7/2013 10:26:45 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/7/2013 10:26:45 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.MainPage.Views
{
    public partial class UserInformation : ChildWindow
    {
        public UserInformation()
        {
            InitializeComponent();
            txtUserName.Text = ApplicationContext.Instance.AuthenticationInfo.UserShowName;
            txtAddress.Text = ApplicationContext.Instance.AuthenticationInfo.Address;
            txtEmail.Text = ApplicationContext.Instance.AuthenticationInfo.Email;
           // txtGroupName.Text = ApplicationContext.Instance.AuthenticationInfo.OrgName;
            txtPhone.Text = ApplicationContext.Instance.AuthenticationInfo.Phone;
          //  txtPostalCode.Text = ApplicationContext.Instance.AuthenticationInfo.PostalCode;
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

