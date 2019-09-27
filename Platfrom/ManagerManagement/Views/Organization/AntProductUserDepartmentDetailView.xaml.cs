using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    /// <summary>
    /// 人员部门详细视图
    /// </summary>
    [ExportAsView(ManagerName.AntProductUserDepartmentDetailV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductUserDepartmentDetailV, ManagerName.ManagerContainer)]
    public partial class AntProductUserDepartmentDetailView : UserControl
    {
        public AntProductUserDepartmentDetailView()
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
