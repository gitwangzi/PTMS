using Gsafety.PTMS.Manager.ViewModels.RoleManage;
using Jounce.Core.View;
using Jounce.Regions.Core;
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

namespace Gsafety.PTMS.Manager.Views.RoleManage
{
    //[ExportAsView(ManagerName.AntProductRoleSelectV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    //[ExportViewToRegion(ManagerName.AntProductRoleSelectV, ManagerName.ManagerContainer)]
    public partial class RoleSelectView : ChildWindow
    {
        public readonly AntProductRoleSelectViewModel viewModel;
        public RoleSelectView(string viewName, IDictionary<string, object> viewParameters)
        {
            InitializeComponent();
            viewModel = new AntProductRoleSelectViewModel();
            this.DataContext = viewModel;
            viewModel.ActivateView(viewName, viewParameters);

            viewModel.OnSaveResult += viewModel_OnSaveResult;
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        void viewModel_OnSaveResult(object sender, Gsafety.Common.CommMessage.SaveResultArgs e)
        {
            if (e.Result)
            {
                this.DialogResult = true;
            }
            else
            {
                this.DialogResult = false;
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

