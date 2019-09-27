using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using PublicServiceManagement.ViewModel;
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

namespace PublicServiceManagement.Views
{
    public partial class PhoneMsgDetailWindow : ChildWindow
    {
        public PhoneMsgDetailWindow(string viewType, IDictionary<string, object> parms)
        {
            InitializeComponent();
            PhoneMsgDetailViewModel viewModel = new PhoneMsgDetailViewModel();
            viewModel.ActivateView(viewType, parms);
            this.DataContext = viewModel;
            viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.MouseRightButtonUp += PhoneMsgDetailWindow_MouseRightButtonUp;
        }

        void PhoneMsgDetailWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

