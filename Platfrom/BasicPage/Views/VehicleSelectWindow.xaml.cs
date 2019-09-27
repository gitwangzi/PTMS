using Gsafety.PTMS.Bases.Models;
using Gsafety.PTMS.BasicPage.ViewModels;
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

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class VehicleSelectWindow : ChildWindow
    {
        VehicleSelectViewModel _datacontext = null;

        public List<VehicleEx> SelectVehicleList
        {
            get
            {
                return _datacontext.VehicleTreeFactory.SelectVehicleList;
            }
        }

        public List<VehicleEx> VehicleList
        {
            get
            {
                return _datacontext.VehicleTreeFactory
                    .VehicleList;
            }
        }

        public VehicleSelectWindow(Func<Vehicle, bool> filter = null)
        {
            InitializeComponent();
            _datacontext = new VehicleSelectViewModel(filter);

            vehicleSelect.DataContext = _datacontext;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

