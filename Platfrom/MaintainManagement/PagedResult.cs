/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f1fb7a42-b627-4b6e-b37c-2088a69ab7e2      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.BaseInformation
/////    Project Description:    
/////             Class Name: PagedResult
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/20 10:44:14
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/20 10:44:14
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Maintain
{
    [DataContract]
    public class PagedResult<T>
    {
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public int PageIndex { get; set; }
        [DataMember]
        public IEnumerable<T> Items { get; set; }
    }
}
