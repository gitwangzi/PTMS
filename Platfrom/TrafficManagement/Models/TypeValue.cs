/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c0b284c0-3ef9-477b-8c2a-2f1e6d2dc51a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHANGY
/////                 Author: GJSY(zhangy)
/////======================================================================
/////           Project Name: Gsafety.Ant.Traffic.Models
/////    Project Description:    
/////             Class Name: TypeValue
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/10/15 10:01:53
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/15 10:01:53
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

namespace Gsafety.PTMS.Traffic.Models
{
    /// <summary>
    /// Fence type structure
    /// </summary>
    public class TypeValue
    {
        /// <summary>
        /// name
        /// </summary>
        public string strName;
        /// <summary>
        /// value
        /// </summary>
        public int strValue;
        /// <summary>
        /// override ToString，return name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return strName;
        }
    }
}
