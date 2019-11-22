/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d6ef3596-7c00-2w3e-b9a7-f58a00595cwe     
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: TrafficNavigationContainer
/////          Class Version: v1.0.0.0
/////            Create Time: 8/8/2013 9:21:34 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/8/2013 9:21:34 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Traffic.Views
{
    [Export]
    public partial class TrafficNavigationContainer : UserControl
    {
        public TrafficNavigationContainer()
        {
            InitializeComponent();
        }
    }
}
