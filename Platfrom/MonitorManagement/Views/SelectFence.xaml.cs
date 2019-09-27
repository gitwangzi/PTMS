using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Monitor.ViewModels;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using Gsafety.PTMS.ServiceReference.TrafficManageService;
using Jounce.Core.Event;
using Jounce.Core.View;
using Jounce.Core.ViewModel;
using Gsafety.PTMS.Monitor.ViewModels;
using Microsoft.Expression.Interactivity.Layout;

namespace Gsafety.PTMS.Monitor.Views
{
    public partial class SelectFence : UserControl
    {
        public SelectFence()
        {
            InitializeComponent();
            IsDrag = true;

            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(FenceDataGrid);
            //viewModel = new ViewModels.SelectFenceViewModel();
            //this.DataContext = viewModel;
           
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
