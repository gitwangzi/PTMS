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
    public partial class MdvrMsgDetailWindow : ChildWindow
    {
        public MdvrMsgDetailWindow(string viewType, IDictionary<string, object> parms)
        {
            InitializeComponent();
            MdvrMsgDetailViewModel viewModel = new MdvrMsgDetailViewModel();
            viewModel.ActivateView(viewType, parms);
            this.DataContext = viewModel;
            viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.MouseRightButtonUp += MdvrMsgDetailWindow_MouseRightButtonUp;

            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;

        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void viewModel_OnSaveResult(object sender, Gsafety.Common.CommMessage.SaveResultArgs e)
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

        void MdvrMsgDetailWindow_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
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

