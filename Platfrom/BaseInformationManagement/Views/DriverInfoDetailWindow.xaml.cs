using Gsafety.Ant.BaseInformation.ViewModels;
using Gsafety.Common.Controls;
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

namespace Gsafety.Ant.BaseInformation.Views
{
    public partial class DriverInfoDetailWindow : ChildWindow
    {
        DriverInfoDetailViewModel viewModel;
        public DriverInfoDetailWindow(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new DriverInfoDetailViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
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
                //messagebox
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

        private void de_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.ICardID = this.de.Text;
        }

        private void Director_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.DriverLicense = this.Director.Text;
        }

        private void Address_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Address = this.Address.Text;
        }

        private void DirectorPhone_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.CellPhone = this.DirectorPhone.Text;
        }

        private void UserCount_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Phone = this.UserCount.Text;
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

