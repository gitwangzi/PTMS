using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.User
{
    [ExportAsView(ManagerName.SystemUserManageV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.SystemUserManageV, ManagerName.ManagerContainer)]
    public partial class SystemUserManageView : UserControl
    {
        public SystemUserManageView()
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
