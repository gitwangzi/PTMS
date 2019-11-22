using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ComandManage
{
    /// <summary>
    /// IP参数设置视图
    /// </summary>
    [ExportAsView(ManagerName.IpParameterSettingV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.IpParameterSettingV, ManagerName.ManagerContainer)]
    public partial class IpParameterSettingView : UserControl
    {
        public IpParameterSettingView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid2);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
           // comboStatus.DropDownOpened += PopupHandler.OnDropDown;
           // comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
