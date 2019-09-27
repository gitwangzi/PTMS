using Gsafety.PTMS.OrderClientManagement;
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

namespace OrderClientManagement.Views
{
    [ExportAsView(OrderClientName.OrderClientInfoV)]
    [ExportViewToRegion(OrderClientName.OrderClientInfoV, "OrderClientContainer")]
    public partial class OrderClientInfoView : UserControl
    {
        public OrderClientInfoView()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Block_Mobile.SetValue(Grid.RowProperty, 1);
            txtMobile.SetValue(Grid.RowProperty, 1);
            Block_Address.SetValue(Grid.RowProperty, 2);
            txtAddress.SetValue(Grid.RowProperty, 2);
            Block_Description.SetValue(Grid.RowProperty, 3);
            txtDescription.SetValue(Grid.RowProperty, 3);

            phoneBlock.SetValue(Grid.RowProperty, 1);
            txtPhone.SetValue(Grid.RowProperty, 1);
            mailBlock.SetValue(Grid.RowProperty, 2);
            txtEmail.SetValue(Grid.RowProperty, 2);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Block_Mobile.SetValue(Grid.RowProperty, 2);
            txtMobile.SetValue(Grid.RowProperty, 2);
            Block_Address.SetValue(Grid.RowProperty, 3);
            txtAddress.SetValue(Grid.RowProperty, 3);
            Block_Description.SetValue(Grid.RowProperty, 4);
            txtDescription.SetValue(Grid.RowProperty, 4);

            phoneBlock.SetValue(Grid.RowProperty, 2);
            txtPhone.SetValue(Grid.RowProperty, 2);
            mailBlock.SetValue(Grid.RowProperty, 3);
            txtEmail.SetValue(Grid.RowProperty, 3);
        }
    }
}
