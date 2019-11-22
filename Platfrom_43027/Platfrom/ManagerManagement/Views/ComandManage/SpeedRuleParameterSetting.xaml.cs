using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Manager.Views.ComandManage
{
    /// <summary>
    /// 设置超速规则参数视图
    /// </summary>
    [ExportAsView(ManagerName.SpeedRuleParameterSettingV, Category = ManagerName.CategoryName, MenuName = ManagerName.UserMangeMenuName)]
    [ExportViewToRegion(ManagerName.SpeedRuleParameterSettingV, ManagerName.ManagerContainer)]
    public partial class SpeedRuleParameterSetting : UserControl
    {
        public SpeedRuleParameterSetting()
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
