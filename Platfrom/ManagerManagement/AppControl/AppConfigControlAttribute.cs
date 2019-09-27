/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: c178330c-2c6b-476d-8e5d-2fc54dff85af      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-ZHOUDD
/////                 Author: GJSY(zhoudd)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Manager
/////    Project Description:    
/////             Class Name: AddEnumWindow
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/07/24 17:50:20
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/10/13 17:57:28
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

namespace Gsafety.PTMS.Manager
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AppConfigControlAttribute:Attribute 
    {
        public AppConfigControlAttribute(bool isDefault, string desc, string name)
            : base()
        {
            this.IsDefault = isDefault;
            this.Desc = desc;
            this.Name = name;
        }

        public bool IsDefault { get;private  set; }

        public string Desc { get; private set; }

        public string Name { get; private set; }
    }
}
