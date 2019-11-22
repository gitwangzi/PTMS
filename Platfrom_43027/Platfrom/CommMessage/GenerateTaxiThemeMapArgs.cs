/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4ce1b046-dc6e-4f6f-b27a-e87cc21af34b      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: GenerateTaxiThemeMapArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/10 11:28:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/10 11:28:20
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

namespace Gsafety.Common.CommMessage
{
    public class GenerateTaxiThemeMapArgs
    {
        /// <summary>
        /// start time
        /// </summary>
        public DateTime dtStartTime { get; set; }
        /// <summary>
        /// timespan:minutes
        /// </summary>
        public int dIntVTime { get; set; }
    }
}
