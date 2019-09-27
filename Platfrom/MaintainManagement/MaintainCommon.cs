/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 02baaa58-e03d-4ab6-afb4-2967ea454685      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Maintain
/////    Project Description:    
/////             Class Name: MaintainCommon
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/11 17:01:35
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/11 17:01:35
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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
    public class MaintainCommon
    {
        public static T Clone<T>(T t)
        {
            var ser = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
            using (var ms = new MemoryStream())
            {
                ser.WriteObject(ms, t);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)ser.ReadObject(ms); ;
            }
        }

        public static List<int> PageSizeList
        {
            get
            {
                List<int> pageSizeList = new List<int>();
                pageSizeList.Add(20);
                pageSizeList.Add(40);
                pageSizeList.Add(80);
                return pageSizeList;
            }
        }
    }
}
