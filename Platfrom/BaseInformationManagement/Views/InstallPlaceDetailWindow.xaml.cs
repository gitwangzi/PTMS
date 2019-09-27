using Gsafety.Ant.BaseInformation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{

    public partial class InstallPlaceDetailWindow : ChildWindow
    {
        private readonly InstallPlaceDetailViewModel viewModel;
        public InstallPlaceDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new InstallPlaceDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;

            comboStatus2.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus2.DropDownClosed += PopupHandler.OnDropDown;


        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        void viewModel_OnSaveResult(object sender, Common.CommMessage.SaveResultArgs e)
        {
            if (e.Result)
            {
                DialogResult = true;
            }
            else
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), e.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void UserName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.UserName.Text;
        }

        private void Address_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Address = this.Address.Text;
        }

        private void Director_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Director = this.Director.Text;
        }

        private void DirectorPhone_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.DirectorPhone = this.DirectorPhone.Text;
        }

        private void Contact_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Contact = this.Contact.Text;
        }

        private void UserCount_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.ContactPhone = this.UserCount.Text;
        }

        private void Email_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Email = this.Email.Text;
        }

        private void Note_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Note = this.Note.Text;
        }
    }
}

