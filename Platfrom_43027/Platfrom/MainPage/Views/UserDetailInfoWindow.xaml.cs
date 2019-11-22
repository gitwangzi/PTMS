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
using Gsafety.Ant.MainPage.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;

namespace Gsafety.Ant.MainPage.Views
{
    public partial class UserDetailInfoWindow : ChildWindow
    {
        private readonly UserDetailInfoWindowVm viewModel;
        public UserDetailInfoWindow()
        {
            InitializeComponent();
            this.viewModel = new UserDetailInfoWindowVm();
            this.DataContext = this.viewModel;
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            
            this.viewModel.ActivateView();
            this.MouseRightButtonDown += UserDetailInfoWindow_MouseRightButtonDown;
        }

        void UserDetailInfoWindow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
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
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

