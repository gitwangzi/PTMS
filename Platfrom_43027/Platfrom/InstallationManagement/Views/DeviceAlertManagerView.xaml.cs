using Gsafety.Common.Controls;
using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Installation;
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

namespace Gsafety.Ant.Installation.Views
{
    [ExportAsView(InstallationName.DeviceAlertManageV)]
    [ExportViewToRegion(InstallationName.DeviceAlertManageV, ViewContainer.InstallContainer)]
    public partial class DeviceAlertManagerView : UserControl
    {
        public DeviceAlertManagerView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
