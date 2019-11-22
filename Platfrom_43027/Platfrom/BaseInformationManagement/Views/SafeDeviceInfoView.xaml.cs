using Gsafety.Common.Controls;
using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{
    /// <summary>
    /// 安全套件管理界面
    /// </summary>
    [ExportAsView(BaseInformationName.SafeDeviceInfoV)]
    [ExportViewToRegion(BaseInformationName.SafeDeviceInfoV, BaseInformationName.BaseInfoContainer)]
    public partial class SafeDeviceInfoView : UserControl
    {
        public SafeDeviceInfoView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(PartListDataGrid);
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
