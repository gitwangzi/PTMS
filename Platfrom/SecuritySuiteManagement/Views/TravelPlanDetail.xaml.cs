using Gsafety.PTMS.Share;
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
    [ExportAsView(SecuritySuiteName.TravelPlanDetailV, 
      MenuName = SecuritySuiteName.VehicleTrafficMenuName, MenuTitle = "TravelPlanDetail",
      ToolTip = "Click to view some text.", Url = "/TravelPlanDetail", Order = 5)]
    [ExportViewToRegion(SecuritySuiteName.TravelPlanDetailV, SecuritySuiteName.SuiteContainer)]
    public partial class TravelPlanDetail : UserControl
    {
        public TravelPlanDetail()
        {
            InitializeComponent();
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(OnlineSuiteGrid);
        }

    }
}
