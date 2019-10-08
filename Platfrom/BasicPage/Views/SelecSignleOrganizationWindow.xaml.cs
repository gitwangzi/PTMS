using Gsafety.PTMS.BasicPage.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.PTMS.BasicPage.Views
{
    public partial class SelecSignleOrganizationWindow : ChildWindow
    {
        public SelecSignleOrganizationWindowViewModel _viewModel = null;
        public SelecSignleOrganizationWindow(string userId)
        {
            InitializeComponent();
            this._viewModel = new SelecSignleOrganizationWindowViewModel(userId);
            this.DataContext = this._viewModel;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void monitorTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if(treeView != null)
            {
                var selectedItem = treeView.SelectedItem;
                if(selectedItem != null)
                {
                    Gsafety.PTMS.Bases.Models.OrganizationEx organizationEx = selectedItem as Gsafety.PTMS.Bases.Models.OrganizationEx;
                    if(organizationEx != null)
                    {
                        this._viewModel.SelectedOrganizationItem = organizationEx;
                        //this._viewModel.SelectedOrganizationItem.Name = organizationEx.Organization.Name;
                        //this._viewModel.SelectedOrganizationItem.ParentID = organizationEx.Organization.ParentID;
                        this._viewModel.SaveVehicleOrg();
                    }

                }
            }
        }
    }
}

