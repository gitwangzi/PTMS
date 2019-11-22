using Gsafety.Ant.BaseInformation.ViewModels.OrganizationViewModel;
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
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

namespace Gsafety.Ant.BaseInformation.Views.Organization
{
    public partial class VehicleBindingDriverWindow : ChildWindow
    {
        VehicleBindingDriverViewModel viewModel;
        public VehicleBindingDriverWindow(string vehicleId)
        {
            InitializeComponent();
            viewModel = new VehicleBindingDriverViewModel(vehicleId);
            this.DataContext = viewModel;
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.MouseRightButtonDown += VehicleBindingDriverWindow_MouseRightButtonDown;
        }

        void VehicleBindingDriverWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

