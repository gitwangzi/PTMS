﻿using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using Jounce.Core;
using Gsafety.PTMS.Constants;
namespace Gsafety.PTMS.Installation.Views
{
    [ExportAsView(InstallationName.MaintainApplcationManagementScheduledV)]
    [ExportViewToRegion(InstallationName.MaintainApplcationManagementScheduledV, ViewContainer.InstallContainer)]
    public partial class MaintainApplicationScheduledManageView : UserControl
    {
        public MaintainApplicationScheduledManageView()
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

