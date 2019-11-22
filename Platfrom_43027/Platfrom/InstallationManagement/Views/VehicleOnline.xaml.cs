using Gsafety.PTMS.Constants;
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

namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.VehicleOnlineV, Category = InstallationName.CategoryName,
    ToolTip = "Click to view some text.", Url = "/VehicleOnline")]
    [ExportViewToRegion(InstallationName.VehicleOnlineV, ViewContainer.InstallContainer)]
    public partial class VehicleOnline : UserControl
    {
        public VehicleOnline()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(this.VehicleOnlineGrid);
        }
    }
}
