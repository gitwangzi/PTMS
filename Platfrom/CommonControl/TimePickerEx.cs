/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e0875467-1dd5-473c-b96e-a384c96aa60c      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: KENNY-PC
/////                 Author: TEST(jiaoyx)
/////======================================================================
/////           Project Name: Gsafety.Common.Controls
/////    Project Description:    
/////             Class Name: DatePickerEx
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/8/21 11:38:39
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/8/21 11:38:39
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

namespace Gsafety.Common.Controls
{
    public class TimpePickerEx : TimePicker
    {
        //string strContent;
        //Ctor
        public TimpePickerEx()
        {
            //this.DisplayDateEnd = DateTime.Now;  //last date is today
        }

        #region Override
        protected override void OnKeyUp(KeyEventArgs e)
        {
            //Clipboard.SetText(strContent);
            base.OnKeyUp(e);
            //if (this != null && this.Value != null)
            //    this.Value = this.Value;
            e.Handled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //strContent = Clipboard.GetText();
            Clipboard.SetText("");
            base.OnKeyDown(e);
            e.Handled = true;
        }
        #endregion
    }
}
