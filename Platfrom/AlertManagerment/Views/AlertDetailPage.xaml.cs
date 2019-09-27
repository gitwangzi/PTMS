using Microsoft.Expression.Interactivity.Layout;
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

namespace Gsafety.PTMS.Alert.Views
{
    public partial class AlertDetailPage : UserControl
    {
        public AlertDetailPage()
        {
            InitializeComponent();
        }

        #region prop
        MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
        private bool isDrag;
        /// <summary>
        /// Draggable and right-click
        /// </summary>
        public bool IsDrag
        {
            get { return isDrag; }
            set
            {
                isDrag = value;
                if (isDrag == true)
                {
                    dragBehavior.Attach(this);//This object can join the mouse to drag the behavior of the object
                    dragBehavior.DragFinished += new MouseEventHandler(dragBehavior_DragFinished);
                    //In the object moving after successful load a handle events.
                }
                else if (isDrag == false)
                {
                    try
                    {
                        //Set the control mobile behavior cancel, and cancel the DragFinished processing events
                        dragBehavior.Detach();
                        dragBehavior.DragFinished -= new MouseEventHandler(dragBehavior_DragFinished);
                    }
                    catch
                    {

                    }
                }

            }
        }
        #endregion

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
    }
}
