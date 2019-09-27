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
    [ExportAsView(SecuritySuiteName.SwitchingStatusV, Category = SecuritySuiteName.CategoryName,
    MenuName = SecuritySuiteName.VehicleEquipmentMenuName, MenuTitle = "MAINTAIN_SwitchDeviceStatus",
    ToolTip = "Click to view some text.", Url = "/SwitchingStatus", Order = 2)]
    [ExportViewToRegion(SecuritySuiteName.SwitchingStatusV, SecuritySuiteName.SuiteContainer)]
    public partial class SwitchingStatus : UserControl
    {
        public SwitchingStatus()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(this.SwitchingStatusDataGrid);
        }
    }
}
