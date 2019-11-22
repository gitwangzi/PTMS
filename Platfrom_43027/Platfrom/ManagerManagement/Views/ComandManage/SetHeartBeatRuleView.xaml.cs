using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ComandManage
{
    /// <summary>
    /// 设置心跳参数规则视图
    /// </summary>
    [ExportAsView(ManagerName.SetHeartBeatRuleV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.SetHeartBeatRuleV, ManagerName.ManagerContainer)]
    public partial class SetHeartBeatRuleView : UserControl
    {
        public SetHeartBeatRuleView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid2);
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;
        }

        private void ChildWindow_MouseRightButtonDown(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
