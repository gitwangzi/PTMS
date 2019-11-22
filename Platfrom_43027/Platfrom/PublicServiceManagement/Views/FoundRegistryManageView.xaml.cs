using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace PublicServiceManagement.Views
{
	[ExportAsView(PublicServiceName.FoundRegistryManageV)]
    [ExportViewToRegion(PublicServiceName.FoundRegistryManageV, "PublicServiceContainer")]
	public partial class FoundRegistryManageView : UserControl
	{
		public FoundRegistryManageView()
		{
			InitializeComponent();
			Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
		}
	}
}

