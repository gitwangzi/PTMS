using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views.Organization
{
    [ExportAsView(BaseInformationName.AntProductAddVehicleDepartmentV, Category = BaseInformationName.CategoryName, MenuName = BaseInformationName.AntProductAddVehicleDepartmentV)]
    [ExportViewToRegion(BaseInformationName.AntProductAddVehicleDepartmentV, "VehicleDepartmentContainer")]
    public partial class AddVehicleDepartmentView : UserControl
    {
        public AddVehicleDepartmentView()
        {
            InitializeComponent();
        }
    }
}
