/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e73be928-54bf-4750-85a9-12b506dcecbd      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-PENGGL
/////                 Author: TEST(penggl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share.Model
/////    Project Description:    
/////             Class Name: EnumModel
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/16 13:44:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/16 13:44:21
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

namespace Gsafety.PTMS.Bases.Models
{
    public class EnumModel
    {
        /// <summary>
        /// EnumName 
        /// </summary>
        public string EnumName { get; set; }
        /// <summary>
        /// ShowName 
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// EnumValue
        /// </summary>
        public int EnumValue { get; set; }
    }
}
