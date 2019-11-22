using Gsafety.PTMS.Constants;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Installation.Views
{
    /// <summary>
    /// 安全套件管理界面
    /// </summary>
    [ExportAsView(InstallationName.DevSuiteManageV)]
    [ExportViewToRegion(InstallationName.DevSuiteManageV, ViewContainer.InstallContainer)]
    public partial class DevSuiteManageView : UserControl
    {
        public DevSuiteManageView()
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
