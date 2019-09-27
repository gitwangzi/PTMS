/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 1dcd6d31-64cc-4e49-9f52-3a0c974274d0      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.CommMessage
/////    Project Description:    
/////             Class Name: MessageNotifitionActiveteParamter
/////          Class Version: v1.0.0.0
/////            Create Time: 11/1/2013 4:15:30 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 11/1/2013 4:15:30 PM
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
    public class MessageNotifitionActiveteParamter
    {
        public bool IsActivete { get; private set; }

        public MessageNotifitionActiveteParamter(bool isActivete)
        {
            IsActivete = isActivete;
        }
    }
}
