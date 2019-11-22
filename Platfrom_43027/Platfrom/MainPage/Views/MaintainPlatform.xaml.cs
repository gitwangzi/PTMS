/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: d4c82149-bcd0-45a7-9024-556ad2e56508      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.MainPage.Views
/////    Project Description:    
/////             Class Name: MaintainPlatform
/////          Class Version: v1.0.0.0
/////            Create Time: 8/14/2013 1:02:57 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/14/2013 1:02:57 PM
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jounce.Core;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Constants;
using Gsafety.Common.Utilities;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.MainPage.Views
{
    [ExportAsView(MainPageName.MaintainPlatformV)]
    [ExportViewToRegion(MainPageName.MaintainPlatformV, ViewContainer.LoginContainer)]
    public partial class MaintainPlatform : UserControl
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

        public MaintainPlatform()
        {
            InitializeComponent();
        }

        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

            bool focusOne = false;
            foreach (var hb in NavigationButtons)
            {
                if (hb.NavigateUri == e.Uri)
                {
                    focusOne = true;
                    VisualStateManager.GoToState(hb, "ActiveLink", true);
                }
                else
                {
                    VisualStateManager.GoToState(hb, "InactiveLink", true);
                }
            }

            if (!focusOne && NavigationButtons != null && NavigationButtons.Count > 0)
            {
                VisualStateManager.GoToState(NavigationButtons[0], "ActiveLink", true);
            }
        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
        }

        private void AccordionControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((MenuItemWidth)((FrameworkElement)sender).Tag).PanelWidth = e.NewSize.Width;
        }
    }
}
