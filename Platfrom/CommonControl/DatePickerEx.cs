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
    public class DatePickerEx : DatePicker
    {
        #region private member

        TextBox currentTextBox;
        TimePicker timePick;

        public string InputText
        {
            get { return (string)GetValue(InputTextProperty); }
            set { SetValue(InputTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InputText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InputTextProperty =
            DependencyProperty.Register("InputText", typeof(string), typeof(DatePickerEx), new PropertyMetadata("", InputTextChanged));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(DatePickerEx), new PropertyMetadata(false));

        #endregion

        #region Override method

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.currentTextBox = GetTemplateChild("TextBox") as TextBox;
            this.currentTextBox.IsReadOnly = IsReadOnly;
            this.CalendarClosed += DatePickerEx_CalendarClosed;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (!this.IsReadOnly)
                this.InputText = this.currentTextBox.Text;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (this.IsReadOnly)
                e.Handled = this.IsReadOnly;
        }

        void DatePickerEx_CalendarClosed(object sender, RoutedEventArgs e)
        {
            this.InputText = this.Text;
        }

        protected static void InputTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DatePickerEx sender = d as DatePickerEx;
            sender.currentTextBox.Text = e.NewValue.ToString();
        }

        #endregion
    }
}
