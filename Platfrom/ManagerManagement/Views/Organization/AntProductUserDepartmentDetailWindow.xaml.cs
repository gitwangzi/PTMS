using Gsafety.Common.Controls;
using Gsafety.PTMS.Manager.ViewModels.OrganizationViewModel;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    public partial class AntProductUserDepartmentDetailWindow : ChildWindow
    {
        private readonly AntProductUserDepartmentDetailViewModel viewModel;
        public AntProductUserDepartmentDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new AntProductUserDepartmentDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += AntProductUserDepartmentDetailWindow_MouseRightButtonDown;
        }

        void AntProductUserDepartmentDetailWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void viewModel_OnSaveResult(object sender, Gsafety.Common.CommMessage.SaveResultArgs e)
        {
            if (e.Result)
            {
                DialogResult = true;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Message);
                //messagebox
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Name_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.Name.Text;
        }

        private void Contact_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Contact = this.Contact.Text;
        }

        private void Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Email = this.Email.Text;
        }

        private void Phone_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Phone = this.Phone.Text;
        }
    }
}

