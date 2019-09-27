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
    [ExportAsView(BaseInformationName.UserManageV)]
    [ExportViewToRegion(BaseInformationName.UserManageV, BaseInformationName.BaseInfoContainer)]
    public partial class UserManage : UserControl
    {
        public UserManage()
        {
            InitializeComponent();
        }

        private void monitorTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
