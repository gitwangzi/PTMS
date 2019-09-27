/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 156d82d6-b1b4-42a8-beac-7852f82a0bc5      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Control
/////    Project Description:    
/////             Class Name: MenuButton
/////          Class Version: v1.0.0.0
/////            Create Time: 8/15/2013 2:39:05 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/15/2013 2:39:05 PM
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
    /// <summary>
    /// Implements a "menu button" for Silverlight and WPF.
    /// </summary>
    public class MenuButton : SplitButton
    {
        /// <summary>
        /// Initializes a new instance of the MenuButton class.
        /// </summary>
        public MenuButton()
        {
            DefaultStyleKey = typeof(MenuButton);
        }

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        protected override void OnClick()
        {
            OpenButtonMenu();
        }
    }
}
