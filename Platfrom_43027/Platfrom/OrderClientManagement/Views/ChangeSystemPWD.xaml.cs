using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using OrderClientManagement.ViewModels;
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

namespace OrderClientManagement.Views
{
    public partial class ChangeSystemPWD : ChildWindow
    {
        ChangeSystemPWDViewModel viewModel;
        public ChangeSystemPWD(string userId, string account)
        {
            InitializeComponent();
            viewModel = new ChangeSystemPWDViewModel(userId, account);
            this.DataContext = viewModel;
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            IsValidation();
        }       

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void LayoutRoot_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (IsValidation())
            {
                OKButton.IsEnabled = true;
            }
            else
            {
                OKButton.IsEnabled = false;
            }
        }

        private bool IsValidation()
        {
            //if (Validation.GetErrors(currentPasswordBox).Count > 0)
            //{
            //    return false;
            //}

            if (Validation.GetErrors(newPasswordBox).Count > 0)
            {
                return false;
            }

            if (Validation.GetErrors(confirmPasswordBox).Count > 0)
            {
                return false;
            }

            return true;
        }

        private void confirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string newpwd = newPasswordBox.Password;
            string conpwd = confirmPasswordBox.Password;
            if (conpwd.Equals(newpwd))
            {
                viewModel.ClearError();
            }
        }

        private void confirmPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            viewModel.ValidataPasswordContrast();
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
            }
        }
    }
}

