using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Jounce.Core;
using Gsafety.PTMS.Constants;
namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.MaintainApplcationManagementV)]
    [ExportViewToRegion(InstallationName.MaintainApplcationManagementV, ViewContainer.InstallContainer)]
    public partial class MaintainApplicationManageView : UserControl
    {
        public MaintainApplicationManageView()
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

