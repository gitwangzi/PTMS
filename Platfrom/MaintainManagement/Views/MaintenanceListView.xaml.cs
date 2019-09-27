using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Share;
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

namespace Gsafety.PTMS.Maintain.Views
{
    [ExportAsView(MaintainName.MaintenanceListView)]
    [ExportViewToRegion(MaintainName.MaintenanceListView, ViewContainer.MaintainContainer)]
    public partial class MaintenanceListView : UserControl
    {
        public MaintenanceListView()
        {
            InitializeComponent();
            //ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(MaintainDataGrid);
            ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(dataGrid);
        }
    }


}
