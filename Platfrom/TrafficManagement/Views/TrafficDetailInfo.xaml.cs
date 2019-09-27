/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 30c92e31-ced4-44bf-bac6-19098bc67a6e      
/////             clrversion: 4.0.30319.17929
/////Registered organization: Microsoft
/////           Machine Name: PC-WANGYL
/////                 Author: TEST(wangyl)
/////======================================================================
/////           Project Name: Gsafety.PTMS.Traffic.Views
/////    Project Description:    
/////             Class Name: TrafficDetailInfo
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/10/25 9:29:27
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/10/25 9:29:27
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
using Gsafety.PTMS.Share;
using Microsoft.Expression.Interactivity.Layout;
using System.Reflection;
using System.Text.RegularExpressions;
using Gsafety.Common.Controls;

namespace Gsafety.PTMS.Traffic.Views
{
    public partial class TrafficDetailInfo : UserControl
    {
        public TrafficDetailInfo()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
            }
        }
        private string pattern = @"^[0-9]*$";


        MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
        private bool isDrag;
        /// <summary>
        /// 
        /// </summary>
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
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                    }
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dragBehavior_DragFinished(object sender, MouseEventArgs e)
        {
            MouseDragElementBehavior dragBehavior = sender as MouseDragElementBehavior;
            //this.Tag = dragBehavior.X + "|" + dragBehavior.Y; // this.Tag
            //this.label1.Content = "X：" + dragBehavior.X + "---Y：" + dragBehavior.Y;
            //
        }

        private void FenceSt_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Ctrl_CanNotUsed"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageDialogButton.Ok);
            }
            base.OnKeyDown(e);
            e.Handled = true;
        }

        private void FenceEt_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("Ctrl_CanNotUsed"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), MessageDialogButton.Ok);
            }
            base.OnKeyDown(e);
            e.Handled = true;
        }

        private void tbMaxSpeed_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
