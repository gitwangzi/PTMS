using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.PTMS.Traffic.Views
{
    /// <summary>
    /// 设置超速规则参数视图
    /// </summary>
    [ExportAsView(TrafficName.SpeedRuleParameterSettingV, Category = TrafficName.CategoryName, MenuName = TrafficName.SpeedMenuName)]
    [ExportViewToRegion(TrafficName.SpeedRuleParameterSettingV, TrafficName.TrafficContainer)]
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
