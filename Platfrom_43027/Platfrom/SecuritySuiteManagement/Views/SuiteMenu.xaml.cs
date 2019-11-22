using Gsafety.Common.Utilities;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 34869d5d-3a16-4236-9503-4ebbe4b8c831      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.SecuritySuite.Views
/////    Project Description:    
/////             Class Name: SuiteMenu
/////          Class Version: v1.0.0.0
/////            Create Time: 8/7/2013 9:10:00 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/7/2013 9:10:00 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Gsafety.Common.Controls;

namespace Gsafety.PTMS.SecuritySuite.Views
{
    public partial class SuiteMenu : UserControl
    {
        List<HyperlinkButton> _NavigationButton;

        public List<HyperlinkButton> NavigationButtons
        {
            get 
            {
                if (_NavigationButton == null || _NavigationButton.Count == 0)
                {
                    VisualTreeExtedHelper vtHelper = new VisualTreeExtedHelper();
                    _NavigationButton = vtHelper.GetChildObjects<HyperlinkButton>(this.LayoutRoot, "");
                }
                return _NavigationButton; 
            }
        }


        public SuiteMenu()
        {
            InitializeComponent();
            acc_menuContainer.SelectedIndex = 0;
        }

        private void AccordionControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((MenuItemWidth)((FrameworkElement)sender).Tag).PanelWidth = e.NewSize.Width;
        }

    }
}
