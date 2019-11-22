using Gsafety.Common.Controls;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.Organization
{
    [ExportAsView(ManagerName.AntProductUserDetailV, Category = ManagerName.CategoryName, MenuName = ManagerName.AntProductUserDetailV)]
    [ExportViewToRegion(ManagerName.AntProductUserDetailV, ManagerName.AntProductDepartmentContainer)]
    public partial class AntProductUserDetailView : UserControl
    {
        public AntProductUserDetailView()
        {
            InitializeComponent();
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;

            comboStatus2.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus2.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
