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

namespace PublicServiceManagement.Views
{
    [ExportAsView(PublicServiceName.PhoneMsgManageV)]
    [ExportViewToRegion(PublicServiceName.PhoneMsgManageV, "PublicServiceContainer")]
    public partial class PhoneMsgManageView : UserControl
    {
        public PhoneMsgManageView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid2);
        }
    }
}
