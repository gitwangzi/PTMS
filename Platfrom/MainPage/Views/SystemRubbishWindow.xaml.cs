using Gsafety.Ant.MainPage.ViewModels;
using Gsafety.PTMS.MainPage.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Gsafety.Ant.MainPage.Views
{
    public partial class SystemRubbishWindow : ChildWindow
    {
        SystemRubbishWindowVm viewModel;
        public SystemRubbishWindow()
        {
            InitializeComponent();
            viewModel = new SystemRubbishWindowVm();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(VehicleListDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SafeDeviceListDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(UserListDataGrid);
            this.DataContext = viewModel;
        }

    }
}

