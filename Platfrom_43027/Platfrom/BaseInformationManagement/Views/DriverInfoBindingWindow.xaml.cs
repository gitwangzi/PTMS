using Gsafety.Ant.BaseInformation.ViewModels;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{
    public partial class DriverInfoBindingWindow : ChildWindow
    {
        private readonly DriverInfoBindingViewModel viewModel;
        public DriverInfoBindingWindow(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new DriverInfoBindingViewModel();
            this.DataContext = viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        void viewModel_OnSaveResult(object sender, Common.CommMessage.SaveResultArgs e)
        {
            if(e.Result)
            {
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString(LProxy.Caption), ApplicationContext.Instance.StringResourceReader.GetString(e.Message));

            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}

