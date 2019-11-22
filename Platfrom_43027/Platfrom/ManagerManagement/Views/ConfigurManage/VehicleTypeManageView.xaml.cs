using Gsafety.PTMS.Manager;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.Manager.Views.ConfigurationManage
{
    [ExportAsView(ManagerName.VehicleTypeManageViewV, Category = ManagerName.CategoryName,
        MenuName = ManagerName.UserMangeMenuName, MenuTitle = "VehicleType",
        ToolTip = "Click to view some text.", Url = "/VehicleTypeManageViewV", Order = 0)]
    [ExportViewToRegion(ManagerName.VehicleTypeManageViewV, ManagerName.ManagerContainer)]
    public partial class VehicleTypeManageView : UserControl
    {
        public VehicleTypeManageView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}

