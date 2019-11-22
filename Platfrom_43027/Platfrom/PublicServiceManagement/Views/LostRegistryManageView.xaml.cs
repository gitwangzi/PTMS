using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;

namespace PublicServiceManagement.Views
{
	[ExportAsView(PublicServiceName.LostRegistryManageV)]
    [ExportViewToRegion(PublicServiceName.LostRegistryManageV, "PublicServiceContainer")]
	public partial class LostRegistryManageView : UserControl
	{
		public LostRegistryManageView()
		{
			InitializeComponent();
			Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
		}
	}
}

