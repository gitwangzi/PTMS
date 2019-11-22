using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.SuperPowerManagement;
using Jounce.Core.View;
using Jounce.Regions.Core;
using SuperPowerManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace SuperPowerManagement.Views
{
    [ExportAsView(SuperPowerName.OrderClientManageV)]
    [ExportViewToRegion(SuperPowerName.OrderClientManageV, "SuperContainer")]
    public partial class OrderClientManageView : UserControl
    {
        public OrderClientManageView()
        {
            try
            {
                InitializeComponent();
                Gsafety.PTMS.Share.ApplicationContext.Instance.StringResourceReader.TranslateDataGrid(ListDataGrid);
                comboStatus.DropDownOpened += PopupHandler.OnDropDown;
                comboStatus.DropDownClosed += PopupHandler.OnDropDown;
            }
            catch (Exception ex)
            {
                ApplicationContext.Instance.Logger.LogException("OrderClientManageView()", ex);
            }
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
