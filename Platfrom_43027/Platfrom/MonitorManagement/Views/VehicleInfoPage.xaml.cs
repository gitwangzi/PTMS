using Gsafety.PTMS.Share;
using Microsoft.Expression.Interactivity.Layout;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace Gsafety.PTMS.Monitor.Views
{
    public partial class VehicleInfoPage : UserControl
    {
        public VehicleInfoPage()
        {
            InitializeComponent();//
            //ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dgChauffeurlist);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        MouseDragElementBehavior dragBehavior = new MouseDragElementBehavior();
        private bool isDrag;
        /// <summary>
        /// Are allowed to be dragged and Right
        /// </summary>
        public bool IsDrag
        {
            get { return isDrag; }
            set
            {
                isDrag = value;
                if (isDrag == true)
                {
                    dragBehavior.Attach(this);//The objects can be added to the behavior of the mouse to drag the object to
                    dragBehavior.DragFinished += new MouseEventHandler(dragBehavior_DragFinished);
                    //After loading a processing object moves successful event。
                }
                else if (isDrag == false)
                {
                    try
                    {
                        //Set the controls to move the behavior to cancel and cancel DragFinished handle events
                        //dragBehavior.Detach();
                        //dragBehavior.DragFinished -= new MouseEventHandler(dragBehavior_DragFinished);
                    }
                    catch (Exception ex)
                    {
                        ApplicationContext.Instance.Logger.LogException(MethodBase.GetCurrentMethod().ToString(), ex);
                    }
                }

            }
        }

        /// <summary>
        /// After the completion of the event was triggered by dragging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dragBehavior_DragFinished(object sender, MouseEventArgs e)
        {
            MouseDragElementBehavior dragBehavior = sender as MouseDragElementBehavior;
            //this.Tag = dragBehavior.X + "|" + dragBehavior.Y; // this.Tag Set to the corresponding value
            //this.label1.Content = "X：" + dragBehavior.X + "---Y：" + dragBehavior.Y;
            //After setting the mouse to drag the control to display the coordinate position in the current control in label1
        }

        /// <summary>
        /// 收缩按钮方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperatorGridImage_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExpendCollaspeInfoGrid.Visibility = Visibility.Collapsed;
            DriverDataGridGrid.Visibility = Visibility.Collapsed;
            this.ShowInfoScrollViewer.Visibility = Visibility.Collapsed;
            this.OperatorCollapseGridImage.Visibility = Visibility.Collapsed;
            this.OperatorExpendGridImage.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 伸展按钮方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OperatorExpendGridImage_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ExpendCollaspeInfoGrid.Visibility = Visibility.Visible;
            DriverDataGridGrid.Visibility = Visibility.Visible;
            this.ShowInfoScrollViewer.Visibility = Visibility.Visible;
            this.OperatorCollapseGridImage.Visibility = Visibility.Visible;
            this.OperatorExpendGridImage.Visibility = Visibility.Collapsed;
        }
    }
}
