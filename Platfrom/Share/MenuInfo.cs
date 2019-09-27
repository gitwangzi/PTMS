using Jounce.Core.Command;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 7ae91a61-2619-4f32-bc64-0144cb398f5d      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Share
/////    Project Description:    
/////             Class Name: MenuInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 1:58:22 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 1:58:22 PM
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

namespace Gsafety.PTMS.Share
{
    public class MenuInfo
    {
        #region Fields

        private string _Category; 
        private string _SubMenuNeme;
        private string _MenuKey;
        private string _MenuTitle;
        private string _Uri;
        private string _ToolTip;
        private int _Order;
        private bool _IsVisible;
        private IActionCommand _Command;

        #endregion

        #region Attributes

        public string Category
        {
            get { return _Category; }
            set { _Category = value; }
        }

        public string SubMenuType
        {
            get { return _SubMenuNeme; }
            set { _SubMenuNeme = value; }
        }

        public string MenuKey
        {
            get { return _MenuKey; }
            set { _MenuKey = value; }
        }

        public string MenuTitle
        {
            get { return _MenuTitle; }
            set { _MenuTitle = value; }
        }

        public string Uri
        {
            get { return _Uri; }
            set { _Uri = value; }
        }

        public string ToolTip
        {
            get { return _ToolTip; }
            set { _ToolTip = value; }
        }


        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }

        public int Order
        {
            get { return _Order; }
            set { _Order = value; }
        }

        public IActionCommand Command
        {
            get { return _Command; }
            set { _Command = value; }
        }

        #endregion

        public MenuInfo(string menuName, string menuTitle, string uri)
        {
            _MenuKey = menuName;
            _MenuTitle = menuTitle;
            _Uri = uri;
            _Order = 0;
            _IsVisible = true;
        }
    }
}
