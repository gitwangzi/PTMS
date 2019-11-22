using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace Gsafety.PTMS.Manager.Views.ConfigurationManage
{
    [ExportAsView(ManagerName.VehicleTypeDetailViewV)]
    [ExportViewToRegion(ManagerName.VehicleTypeDetailViewV, ManagerName.ManagerContainer)]
    public partial class VehicleTypeDetailView : UserControl
    {
        public VehicleTypeDetailView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}

