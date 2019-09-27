using Gsafety.PTMS.SecuritySuite;
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

namespace Gsafety.PTMS.SecuritySuite.Views
{
    [ExportAsView(SecuritySuiteName.SuiteStatusManagementV, Category = SecuritySuiteName.CategoryName,
       MenuName = SecuritySuiteName.VehicleEquipmentMenuName, MenuTitle = "SUITE_SuiteManage",
       ToolTip = "Click to view some text.", Url = "/SuiteStatusManagement", Order = 1)]
    [ExportViewToRegion(SecuritySuiteName.SuiteStatusManagementV, SecuritySuiteName.SuiteContainer)]
    public partial class SuiteStatusManagement : UserControl
    {
        public SuiteStatusManagement()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(this.SuiteStatusManagementGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
