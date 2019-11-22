using Gsafety.PTMS.ServiceReference.AccountService;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    [ExportAsView(ManagerName.AntProductUserManageV, Category = ManagerName.CategoryName, MenuName = ManagerName.AntProductUserManageV)]
    [ExportViewToRegion(ManagerName.AntProductUserManageV, ManagerName.AntProductDepartmentContainer)]
    public partial class AntProductUserManageView : UserControl
    {
        public AntProductUserManageView()
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
