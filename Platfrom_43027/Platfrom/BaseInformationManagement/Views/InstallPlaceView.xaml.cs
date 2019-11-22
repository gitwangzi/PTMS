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
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions;
using Jounce.Regions.Core;

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.InstallPlaceV,Category = BaseInformationName.CategoryName,
        MenuName = BaseInformationName.MenuName, MenuTitle = "BASEINFO_SetupStationView",
        ToolTip = "Click to view some text.", Url = "/InstallPlaceV", Order = 0)]
    [ExportViewToRegion(BaseInformationName.InstallPlaceV, BaseInformationName.BaseInfoContainer)]
    public partial class InstallPlaceView : UserControl
    {
        public InstallPlaceView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
        }
    }
}
