﻿using Microsoft.Expression.Interactivity.Layout;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 756fdee5-69b8-4b1f-92f1-c73361caf316      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGZS
/////                 Author: TEST(wangzs)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alert.Views
/////    Project Description:    
/////             Class Name: AlertDetailPage
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/16 11:54:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/16 11:54:17
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class AlertDetailPage : UserControl
    {
        public AlertDetailPage()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// After being dragged to complete the trigger event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dragBehavior_DragFinished(object sender, MouseEventArgs e)
        {
            MouseDragElementBehavior dragBehavior = sender as MouseDragElementBehavior;
            //this.Tag = dragBehavior.X + "|" + dragBehavior.Y; // this.Tag Set to the corresponding values
            //this.label1.Content = "X：" + dragBehavior.X + "---Y：" + dragBehavior.Y;
            //After setting the mouse to drag the controls, displays the current in the label1 controls the coordinates of the location
        }

        private void OperatorCollapseGridImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OperatorCollapseGridImage.Visibility = Visibility.Collapsed;
            OperatorExpendGridImage.Visibility = Visibility.Visible;
            ShowInfoScrollViewer2.Visibility = Visibility.Collapsed;
        }

        private void OperatorExpendGridImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OperatorCollapseGridImage.Visibility = Visibility.Visible;
            OperatorExpendGridImage.Visibility = Visibility.Collapsed;
            ShowInfoScrollViewer2.Visibility = Visibility.Visible;
        }
    }
}
