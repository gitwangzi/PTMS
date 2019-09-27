﻿using Gsafety.PTMS.Share;
using Microsoft.Expression.Interactivity.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Traffic.ViewModels;

namespace Gsafety.PTMS.Traffic.Views
{
    public partial class SendVehicleDetailView : ChildWindow
    {
        SendVehicleDetailViewModel viewModel = null;
        public SendVehicleDetailView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            viewModel = new SendVehicleDetailViewModel();
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.MouseRightButtonDown += SendVehicleDetailView_MouseRightButtonDown;
        }

        void SendVehicleDetailView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
