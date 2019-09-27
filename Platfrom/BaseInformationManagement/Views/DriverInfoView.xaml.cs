using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.DriverInfoV)]
    [ExportViewToRegion(BaseInformationName.DriverInfoV, BaseInformationName.BaseInfoContainer)]
    public partial class DriverInfoView : UserControl
    {
        public DriverInfoView()
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
