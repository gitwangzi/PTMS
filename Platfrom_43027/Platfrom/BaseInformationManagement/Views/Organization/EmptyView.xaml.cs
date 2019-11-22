using Gsafety.PTMS.BaseInformation;
using Gsafety.PTMS.Manager;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Ant.BaseInformation.Views.Organization
{
    [ExportAsView(BaseInformationName.EmptyView, Category = BaseInformationName.CategoryName, MenuName = ManagerName.VehicleDepartmentListV)]
    [ExportViewToRegion(BaseInformationName.EmptyView, "VehicleDepartmentContainer")]
    public partial class EmptyView : UserControl
    {
        public EmptyView()
        {
            InitializeComponent();
        }
    }
}
