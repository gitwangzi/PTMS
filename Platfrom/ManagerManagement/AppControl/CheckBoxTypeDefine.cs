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
     [AppConfigControl(isDefault: false, desc: "Checkbox", name: "Checkbox")]
    public class CheckBoxTypeDefine:ItemTypeDefine 
    {
         protected CheckBox _control;

         public CheckBoxTypeDefine()
         {
             _control = new CheckBox();
         }

        public override Control CreateControl(string tag, string value, Action<string> callBack)
        {
            bool resu;
            bool.TryParse(value, out resu);
            _control.IsChecked = resu;
            _control.Checked += (s, e) => { callBack((_control.IsChecked ?? false).ToString()); };
            return _control;
        }
    }
}
