/////Copyright (C) Gsafety 2014 .All Rights Reserved.
/////======================================================================
/////                   Guid: 4fb461e0-43eb-4024-8b13-2b25cc831a94      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-LINGL
/////                 Author: TEST(zhangzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Controls
/////    Project Description:    
/////             Class Name: MenuItemWidthDefine
/////          Class Version: v1.0.0.0
/////            Create Time: 2014/1/2 11:06:25
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2014/1/2 11:06:25
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

using System.ComponentModel;

namespace Gsafety.Common.Controls
{
    public class MenuItemWidth : INotifyPropertyChanged
    {

        private double _panelWidth;
        public double PanelWidth
        {
            get
            {
                return _panelWidth;
            }
            set
            {
                if (_panelWidth != value)
                {
                    _panelWidth = value;
                    OnPropertyChanged("PanelWidth");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null && !string.IsNullOrWhiteSpace(propName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

    }
}
