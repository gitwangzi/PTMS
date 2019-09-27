using Gsafety.PTMS.ServiceReference.VehicleAlarmService;
using Gsafety.PTMS.Share;
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
using Gsafety.PTMS.BasicPage.VideoDisplay;
using System.Reflection;
using Microsoft.Expression.Interactivity.Layout;

namespace Gsafety.Ant.Monitor.Views
{
    public partial class AlertHandlePage : UserControl
    {
        public AlertHandlePage()
        {
            InitializeComponent();
            IsDrag = true;
        }
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
    }
}

