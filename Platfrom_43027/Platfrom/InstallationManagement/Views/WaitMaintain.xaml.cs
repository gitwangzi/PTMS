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
using System.Windows.Navigation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Constants;

namespace Gsafety.PTMS.Installation.Views
{

    [ExportAsView(InstallationName.WaitMaintainV, Category = InstallationName.CategoryName,
  ToolTip = "Click to view some text.", Url = "/WaitMaintain")]
    [ExportViewToRegion(InstallationName.WaitMaintainV, ViewContainer.InstallContainer)]
    public partial class WaitMaintain : UserControl
    {
        public WaitMaintain()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(SuiteDataGrid1);
        }


    }
}
