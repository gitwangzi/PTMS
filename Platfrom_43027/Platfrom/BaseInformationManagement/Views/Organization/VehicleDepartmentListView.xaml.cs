using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Manager;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views.Organization
{

    [ExportAsView(BaseInformationName.VehicleDepartmentListV, Category = BaseInformationName.CategoryName, MenuName = ManagerName.VehicleDepartmentListV)]
    [ExportViewToRegion(BaseInformationName.VehicleDepartmentListV, "VehicleDepartmentContainer")]
    public partial class VehicleDepartmentListView : UserControl
    {
        public VehicleDepartmentListView()
        {
            InitializeComponent();
            Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
        }
    }
}
