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
    [ExportAsView(ReportName.HistoryTraceV, Category = ReportName.CategoryName,
      MenuName = ReportName.VehicleReportMenu, MenuTitle = "视频流量", Url = "/HistoryTraceView", Order = 2)]
    [ExportViewToRegion(ReportName.HistoryTraceV, ReportName.ReportContainer)]
    public partial class HistoryTraceView : UserControl
    {
        public HistoryTraceView()
        {
            InitializeComponent();

            combProvince.DropDownOpened += PopupHandler.OnDropDown;
            combProvince.DropDownClosed += PopupHandler.OnDropDown;

            combCity.DropDownOpened += PopupHandler.OnDropDown;
            combCity.DropDownClosed += PopupHandler.OnDropDown;

            combVehicleType.DropDownOpened += PopupHandler.OnDropDown;
            combVehicleType.DropDownClosed += PopupHandler.OnDropDown;

            combVehicle.DropDownOpened += PopupHandler.OnDropDown;
            combVehicle.DropDownClosed += PopupHandler.OnDropDown;
        }
    }
}
