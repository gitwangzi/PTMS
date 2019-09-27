using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.VehicleOrganizationDetailV)]
    [ExportViewToRegion(BaseInformationName.VehicleOrganizationDetailV, "VehicleOrganizationContainer")]
    public partial class VehicleOrganizationDetailView : UserControl
    {
        public VehicleOrganizationDetailView()
        {
            InitializeComponent();
        }
    }
}
