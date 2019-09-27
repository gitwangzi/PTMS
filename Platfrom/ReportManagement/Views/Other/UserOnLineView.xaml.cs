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

namespace Gsafety.PTMS.ReportManager.Views
{
    [ExportAsView(ReportName.UserOnlineV, Category = ReportName.CategoryName,
      MenuName = ReportName.VehicleReportMenu, MenuTitle = "用户在线明细", Url = "/UserOnlineDetailView", Order = 2)]
    [ExportViewToRegion(ReportName.UserOnlineV, ReportName.ReportContainer)]
    public partial class UserOnLineView : UserControl
    {
        public UserOnLineView()
        {
            InitializeComponent();
        }
    }
}
