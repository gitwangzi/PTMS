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
    public partial class SendSpeedRuleDetailView : ChildWindow
    {
        SendSpeedRuleDetailViewModel viewModel;
        public SendSpeedRuleDetailView(string viewName, System.Collections.Generic.IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            viewModel = new SendSpeedRuleDetailViewModel();
            this.DataContext = viewModel;
            this.viewModel.ActivateView(viewName, viewParameters);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid2);
            this.MouseRightButtonDown += SendSpeedRuleDetailView_MouseRightButtonDown;
        }

        void SendSpeedRuleDetailView_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}

