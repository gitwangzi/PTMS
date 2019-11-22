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

namespace Gsafety.PTMS.Traffic.Views
{
    public partial class FenceShapeSelectView : ChildWindow
    {
        public string selectShape = string.Empty;
        public FenceShapeSelectView()
        {
            InitializeComponent();
        }

        private void Poly_Click(object sender, RoutedEventArgs e)
        {
            selectShape = "polygon";
            this.DialogResult = true;
        }

        private void Rectangle_Click(object sender, RoutedEventArgs e)
        {
            selectShape = "rectangle";
            this.DialogResult = true;
        }

        private void Circle_Click(object sender, RoutedEventArgs e)
        {
            selectShape = "circle";
            this.DialogResult = true;
        }
    }
}

