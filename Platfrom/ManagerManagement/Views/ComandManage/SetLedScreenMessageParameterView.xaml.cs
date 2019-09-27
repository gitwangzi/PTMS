using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ComandManage
{
    /// <summary>
    /// 设置LED消息参数视图
    /// </summary>
    [ExportAsView(ManagerName.SetLEDScreenMessageParameterV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.SetLEDScreenMessageParameterV, ManagerName.ManagerContainer)]
    public partial class SetLedScreenMessageParameterView : UserControl
    {
        public SetLedScreenMessageParameterView()
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
