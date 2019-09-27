using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ComandManage
{
    /// <summary>
    /// 查询规则配置视图
    /// </summary>
    [ExportAsView(ManagerName.SearchRoleConfigurationV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.SearchRoleConfigurationV, ManagerName.ManagerContainer)]
    public partial class SearchRoleConfigurationView : UserControl
    {
        public SearchRoleConfigurationView()
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
