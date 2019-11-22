using Gsafety.Common.CommMessage;
using Gsafety.Common.Controls;
using Gsafety.PTMS.Share;
using Jounce.Core.View;
using Jounce.Regions.Core;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GisManagement.Views
{
    public partial class LayerSelectionWindow : ChildWindow
    {
        List<GisLayer> _layers = new List<GisLayer>();

        public List<GisLayer> Layers
        {
            get { return _layers; }
            set { _layers = value; }
        }

        public List<string> SelectedLayers = new List<string>();

        public LayerSelectionWindow()
        {
            InitializeComponent();
            this.MouseRightButtonDown += ChildWindow_MouseRightButtonDown;

            dgLayer.DataContext = Layers;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void ChildWindow_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Layers)
            {
                if (item.IsChecked)
                {
                    SelectedLayers.Add(item.Name);
                }
            }

            DialogResult = true;
        }
    }

    public class GisLayer
    {
        bool _ischecked;

        public bool IsChecked
        {
            get { return _ischecked; }
            set { _ischecked = value; }
        }

        string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}

