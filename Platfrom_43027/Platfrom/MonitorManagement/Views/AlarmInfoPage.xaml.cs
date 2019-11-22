using Gsafety.PTMS.Share;
using Microsoft.Expression.Interactivity.Layout;
/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: f6ffb64e-1d02-46d0-812a-9e805ea1185e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-DENGZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Alarm.Views
/////    Project Description:    
/////             Class Name: AlarmInfoPage
/////          Class Version: v1.0.0.0
/////            Create Time: 9/15/2013 10:06:51 AM
/////      Class Description:  
/////======================================================================
/////          Modified Time: 9/15/2013 10:06:51 AM
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Printing;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class AlarmInfoPage : UserControl
    {
        PrintDocument pd;
        MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
        private bool isDrag;
        /// <summary>
        /// whether allow mouse drag
        /// </summary>
        public bool IsDrag
        {
            get { return isDrag; }
            set
            {
                isDrag = value;
                if (isDrag == true)
                {
                    dragBehavior.Attach(this);//add this object insert into the mouse drag object;
                    //drag finish add a event
                }
                else if (isDrag == false)
                {
                    try
                    {
                        //cancel the control move，cancel the binding event
                        // dragBehavior.Detach();
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException("Property IsDrag", ex);
                    }
                }

            }
        }

        public AlarmInfoPage()
        {
            try
            {
                InitializeComponent();

                pd = new PrintDocument();
                pd.PrintPage += new EventHandler<PrintPageEventArgs>(pd_PrintPage);
            }
            catch (Exception)
            {


            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pd.Print(ApplicationContext.Instance.StringResourceReader.GetString("ALARM_BeginPrint"));
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.PageVisual = DetailInfo;
        }

        private void OperatorCollapseGridImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInfoScrollViewer.Visibility = Visibility.Collapsed;
            OperatorCollapseGridImage.Visibility = Visibility.Collapsed;
            OperatorExpendGridImage.Visibility = Visibility.Visible;
        }

        private void OperatorExpendGridImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowInfoScrollViewer.Visibility = Visibility.Visible;
            OperatorCollapseGridImage.Visibility = Visibility.Visible;
            OperatorExpendGridImage.Visibility = Visibility.Collapsed;
        }
    }
}
