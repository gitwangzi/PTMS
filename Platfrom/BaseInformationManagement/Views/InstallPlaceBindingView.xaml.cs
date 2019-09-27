using Gsafety.PTMS.BaseInformation;
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

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.InstallPlaceBindingV, Category = BaseInformationName.CategoryName,
        MenuName = BaseInformationName.MenuName, MenuTitle = "BASEINFO_SetupStationView",
        ToolTip = "Click to view some text.", Url = "/InstallPlaceBindingV", Order = 0)]
    [ExportViewToRegion(BaseInformationName.InstallPlaceBindingV, BaseInformationName.BaseInfoContainer)]
    public partial class InstallPlaceBindingView : UserControl
    {
        public InstallPlaceBindingView()
        {
            InitializeComponent();
        }
    }
}
