using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.SystemLog
{
    /// <summary>
    /// 安装日志View
    /// </summary>
    [ExportAsView(ManagerName.InstallLogV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.InstallLogV, ManagerName.ManagerContainer)]
    public partial class InstallLogView : UserControl
    {
        public InstallLogView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
