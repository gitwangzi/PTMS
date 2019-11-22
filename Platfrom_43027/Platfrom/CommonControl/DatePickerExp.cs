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

/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////Guid: 0a68efed-aa84-4309-93e6-0d7cd1749664      
///// clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
///// Machine Name: PC-ShiHS
///// Author: (Shihs)
/////======================================================================
///// Project Name: 
///// Project Description:    
/////Class Name: 
///// Class Version: v1.0.0.0
///// Create Time: 2013/10/09 00:00:00
/////Class Description:  
/////======================================================================
/////Modified Time:
/////Modified by:
/////Modified Description: 
/////======================================================================

namespace Gsafety.Common.Controls
{
    /// <summary>
    /// the prohibition of date selector input
    /// </summary>
    public class DatePickerExp : DatePicker
    {
        //Ctor
        public DatePickerExp()
        {
            this.DisplayDateEnd = DateTime.Now;  //last date is today
        }

        #region Override
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (this != null && this.SelectedDate != null)
                this.Text = this.SelectedDate.Value.ToShortDateString();
            e.Handled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            e.Handled = true;
        }
        #endregion
    }
}
