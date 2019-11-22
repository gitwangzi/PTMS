using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Traffic.ViewModels;
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

namespace Gsafety.PTMS.Traffic.Views
{
    public partial class SpeedRuleParameterSettingDetailWindow : ChildWindow
    {
        private readonly SpeedRuleParameterSettingDetailWindowViewModel viewModel;
        public SpeedRuleParameterSettingDetailWindow(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            this.viewModel = new SpeedRuleParameterSettingDetailWindowViewModel();
            this.viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.DataContext = this.viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        void viewModel_OnSaveResult(object sender, SaveResultArgs e)
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

        private void txtName_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Name = this.txtName.Text;
        }

        private void txtMaxSpeed_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.MaxSpeed = this.txtMaxSpeed.Text;
        }

        private void txtDuration_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            this.viewModel.Duration = this.txtDuration.Text;
        }
    }
}

