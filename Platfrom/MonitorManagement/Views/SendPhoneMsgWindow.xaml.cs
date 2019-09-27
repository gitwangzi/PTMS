using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Monitor.ViewModels;
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

namespace Gsafety.PTMS.Monitor.Views
{
    public partial class SendPhoneMsgWindow : ChildWindow
    {
        SendPhoneMsgViewModel viewModel;
        public SendPhoneMsgWindow(string sendVehicleId)
        {
            InitializeComponent();
            viewModel = new SendPhoneMsgViewModel(sendVehicleId);
            viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = viewModel;
            this.MouseRightButtonUp += SendPhoneMsgWindow_MouseRightButtonUp;
        }

        void SendPhoneMsgWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
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

