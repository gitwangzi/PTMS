/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 073ed638-2e82-4c3c-8999-572e757e027a      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: TrafficMenu
/////          Class Version: v1.0.0.0
/////            Create Time: 8/13/2013 3:22:21 PM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 8/13/2013 3:22:21 PM
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
using Gsafety.Common.Utilities;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;

namespace Gsafety.PTMS.Traffic.Views
{
    public partial class TrafficMenu : UserControl
    {
        List<HyperlinkButton> _NavigationButtons;
        public List<HyperlinkButton> NavigationButtons
        {
            get
            {
                if (_NavigationButtons == null || _NavigationButtons.Count == 0)
                {
                    VisualTreeExtedHelper vtHelper = new VisualTreeExtedHelper();
                    _NavigationButtons = vtHelper.GetChildObjects<HyperlinkButton>(this.LayoutRoot, "");
                }
                return _NavigationButtons;
            }
        }
        public TrafficMenu()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(DataGrid_Fence);
            DataGrid_Fence.DoubleClickHook(DataGrid_Fence_Callback);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(DataGrid_Route);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(DataGrid_SpeedRule);
        }

        bool is0 = false;
        private void AccordionControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((MenuItemWidth)((FrameworkElement)sender).Tag).PanelWidth = e.NewSize.Width;

            Size s = e.PreviousSize;
            if (s.Width == 0)
            {
                return;
            }
            //Transform t = btnDetail.RenderTransform;
            double d = e.NewSize.Width - e.PreviousSize.Width;
            double width = d / e.PreviousSize.Width;
            if (width > 0)
            {
                //if (titleGrid.Width + Math.Abs(width) > 450)
                //{
                //    titleGrid.Width = 450;
                //    titleGrid.UpdateLayout();

                //    return;
                //}
                titleGrid.Width += Math.Abs(width) * titleGrid.Width;

            }
            else
            {
                //if (titleGrid.Width - Math.Abs(width) < 150)
                //{
                //    titleGrid.Width = 150;
                //    titleGrid.UpdateLayout();
                //    return;
                //}
                //if (titleGrid.Width <= 0)
                //{
                //    return;
                //}

                titleGrid.Width -= Math.Abs(width) * titleGrid.Width;
                //if (titleGrid.Width == 0)
                //{
                //    titleGrid.Width = 150;
                //}

            }
            //col.UpdateLayout();

        }

        private void DataGrid_Fence_Callback(object source)
        {
            GridDbClickOper(DataGrid_Fence);
        }

        private void GridDbClickOper(DataGrid dg)
        {
            if (dg.SelectedItem != null)
            {
                ((Gsafety.PTMS.Traffic.ViewModels.TrafficMenuVm)this.LayoutRoot.DataContext).OpenDetailViewClick_Event(null);
            }
        }
    }
}
