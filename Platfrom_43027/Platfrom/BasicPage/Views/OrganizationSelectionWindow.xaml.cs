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
    public partial class OrganizationSelectionWindow : ChildWindow
    {
        OrganizationSelectionViewModel _datacontext = null;
        public OrganizationSelectionWindow(string userID)
        {
            InitializeComponent();
            _datacontext = new OrganizationSelectionViewModel(userID);

            this.DataContext = _datacontext;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            _datacontext.SaveVehicleOrg();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

