using Gsafety.Common.Controls;
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
    [ExportAsView(ManagerName.AntProductRoleMangeDetailV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductRoleMangeDetailV, ManagerName.ManagerContainer)]
    public partial class AntProductRoleMangeDetailView : UserControl
    {
        public AntProductRoleMangeDetailView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
