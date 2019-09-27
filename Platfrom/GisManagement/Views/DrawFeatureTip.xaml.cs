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

namespace GisManagement.Views
{
    public partial class DrawFeatureTip : ChildWindow
    {
        private int _nResult = 0;
        public int SleResult
        {
            get { return _nResult; }
            set { _nResult = value; }
        }
        public DrawFeatureTip()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            SleResult = 0;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SleResult = 1;
            this.DialogResult = true;
        }

        private void RedrawButton_Click(object sender, RoutedEventArgs e)
        {
            SleResult = 2;
            this.DialogResult = true;
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    }
}

