using Gsafety.PTMS.Constants;
using Gsafety.PTMS.Installation;
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

namespace Gsafety.Ant.Installation.Views
{
    [ExportAsView(InstallationName.DevGPSManageV)]
    [ExportViewToRegion(InstallationName.DevGPSManageV, ViewContainer.InstallContainer)]
    public partial class DevGPSManageView : UserControl
    {
        public DevGPSManageView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
