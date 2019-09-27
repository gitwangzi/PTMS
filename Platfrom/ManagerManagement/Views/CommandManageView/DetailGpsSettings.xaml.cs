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

namespace Gsafety.PTMS.Manager.Views.CommandManageView
{
    [ExportAsView(ManagerName.DetailGpsSettingsView)]
    [ExportViewToRegion(ManagerName.DetailGpsSettingsView, ManagerName.ManagerContainer)]
    public partial class DetailGpsSettings : UserControl
    {
        public DetailGpsSettings()
        {
            InitializeComponent();
        }
    }
}
