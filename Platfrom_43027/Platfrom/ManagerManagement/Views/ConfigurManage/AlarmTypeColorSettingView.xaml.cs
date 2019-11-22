using Gsafety.Common.Controls;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ConfigurManage
{
    /// <summary>
    /// 告警类型颜色设置视图
    /// </summary>
    [ExportAsView(ManagerName.AntProductAlarmTypeColorSettingV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.AntProductAlarmTypeColorSettingV, ManagerName.ManagerContainer)]
    public partial class AlarmTypeColorSettingView : UserControl
    {
        public AlarmTypeColorSettingView()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

      System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
