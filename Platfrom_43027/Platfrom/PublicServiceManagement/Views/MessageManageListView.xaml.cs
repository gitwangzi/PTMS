using Gsafety.Common.Controls;
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

namespace Gsafety.PTMS.PublicServiceManagement.Views
{
    public partial class MessageManageListView : UserControl
    {
        public MessageManageListView()
        {
            InitializeComponent();
            comboStatus.DropDownOpened += PopupHandler.OnDropDown;
            comboStatus.DropDownClosed += PopupHandler.OnDropDown;
        }

        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

    }
}
