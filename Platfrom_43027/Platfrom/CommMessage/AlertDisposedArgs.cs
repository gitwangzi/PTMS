/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 8a431e27-db22-4aa8-992c-c42fa2744094      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: AlertDisposedArgs
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/7 14:42:52
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/7 14:42:52
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
    /// <summary>
    /// alert message handle
    /// </summary>
    public class AlertDisposedArgs
    {
        /// <summary>
        /// alertID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        ///car no
        /// </summary>
        public string VehicleID { get; set; }
        /// <summary>
        /// the type of operation
        /// Op=0，location
        /// Op=1，alert handle over operation
        /// </summary>
        public int Op { get; set; }

        /// <summary>
        /// display layer
        /// IsDisposed=0，unhandle alert 
        /// IsDisposed=1，handle alert
        /// </summary>
        public int IsDispsed { get; set; }
       
    }
}
