using Gsafety.PTMS.BaseInformation;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System.Windows.Controls;

namespace Gsafety.Ant.BaseInformation.Views
{
    [ExportAsView(BaseInformationName.VehicleDetailV)]
    [ExportViewToRegion(BaseInformationName.VehicleDetailV, "VehicleOrganizationContainer")]
    public partial class VehicleDetailView : UserControl
    {
        public VehicleDetailView()
        {
            InitializeComponent();
        }
    }
}
