/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: c5857a39-46c8-4ea1-bf31-6ce0f6684ffa      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Views
/////    Project Description:    
/////             Class Name: MessageNotification
/////          Class Version: v1.0.0.0
/////            Create Time: 9/2/2013 9:50:28 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/2/2013 9:50:28 AM
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
    public partial class MessageNotification : UserControl
    {
        public MessageNotification()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(AlarmInfoDataGrid);
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(AlertInfoDataGrid);
        }
    }
}
