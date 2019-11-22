/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 24912bff-d40d-4710-89e1-26acf45d6485      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: GisManagement.Views
/////    Project Description:    
/////             Class Name: GpsCarHisDataView
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/5 16:39:21
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/5 16:39:21
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
using System.Windows.Interactivity;
using Microsoft.Expression.Interactivity;
using Microsoft.Expression.Interactivity.Layout;
using System.Windows.Controls.Primitives;
using Gsafety.Common.CommMessage;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Jounce.Regions.Core;

namespace GisManagement.Views
{
    //[ExportAsView(GisName.GpsCarHisDataView,
    //ToolTip = "Click to view some text.", Url = "/GpsCarHisDataView")]
    //[ExportViewToRegion(GisName.GpsCarHisDataView, GisName.GisContainer)]
    public partial class GpsCarHisDataView : UserControl
    {
       
        public GpsCarHisDataView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(hisCarDataGrid);
            this.img_close.AddHandler(Image.MouseLeftButtonDownEvent, new MouseButtonEventHandler(img_close_MouseLeftButtonUp), true);
        }


        MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
        private bool isDrag;

        public bool IsDrag
        {
            get { return isDrag; }
            set
            {
                isDrag = value;
                if (isDrag == true)
                {
                    dragBehavior.Attach(this);
                    dragBehavior.DragFinished += new MouseEventHandler(dragBehavior_DragFinished);
                }
                else if (isDrag == false)
                {
                    try
                    {
                        dragBehavior.Detach();
                        dragBehavior.DragFinished -= new MouseEventHandler(dragBehavior_DragFinished);
                    }
                    catch
                    {

                    }
                }

            }
        }

        void dragBehavior_DragFinished(object sender, MouseEventArgs e)
        {
            MouseDragElementBehavior dragBehavior = sender as MouseDragElementBehavior;
        }

        private void HisCarDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //HisCarDataGrid.ScrollIntoView((sender as DataGrid).SelectedItem, null);
        }

        private void Add_Clicked(object sender, RoutedEventArgs e)
        {
            HistoricalRoute window = new HistoricalRoute("", false);
            window.Closed += new EventHandler(HistoricalRoute_Closed);
            window.Show();
        }

        private void Modify_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void HistoricalRoute_Closed(object sender, EventArgs e)
        {
            HistoricalRoute window = sender as HistoricalRoute;
            window.Closed -= HistoricalRoute_Closed;
            if (window != null && window.DialogResult == true)
            {
                //EventAggregator.Publish<HisTraceArgs>(window.HistraceArgs);
                GisView root = null;
                var temp = this.Parent;
                while (temp!=null)
                {
                    if (temp is GisView)
                    {
                        root = temp as GisView;
                        break;
                    }
                    temp = ((FrameworkElement)temp).Parent;
                   
                }

                if (root != null)
                {
                   
                }
            }
        }

         

        private void img_close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var  ctrl =(FrameworkElement)this.Parent;
            ToggleButton btn = null;
            do
            {
                btn = ctrl.FindName("OpenHistoryLine") as ToggleButton;
                ctrl = (FrameworkElement)ctrl.Parent;
            } while (btn == null);
            if (btn != null)
            {
                btn.IsChecked = false;
            }
             
        }

    }
    
}
