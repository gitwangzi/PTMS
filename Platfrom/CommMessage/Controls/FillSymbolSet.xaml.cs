using Gsafety.PTMS.Share;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.CommMessage.Controls
{
    public partial class FillSymbolSet : ChildWindow
    {
        public FillSymbolSet()
        {
            InitializeComponent();
            Initialize_Colors();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if ((FillColor.SelectedItem as PredefinedColor) == null)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_SelectColor"));
                return;
            }
            TansparentParm = trasparentSet.Value;
            ColorParm = (FillColor.SelectedItem as PredefinedColor).Value;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public ObservableCollection<PredefinedColor> PreDefinedColors = null;
        private void Initialize_Colors()
        {
            if (PreDefinedColors != null)
            {
                return;
            }
            PreDefinedColors = PredefinedColors.PredefinedColorCollection;

            FillColor.DataContext = PreDefinedColors;
            //default:red
            if (PredefinedColors.PredefinedColorCollection.Count > 1)
            {
                FillColor.SelectedValue = PredefinedColors.PredefinedColorCollection[1];
            }
        }
        /// <summary>
        /// 透明度
        /// </summary>
        private double _dTransparent = 0.3;
        public double TansparentParm
        {
            get { return _dTransparent; }
            set { _dTransparent = value; }
        }
        /// <summary>
        /// color
        /// </summary>
        private Color _color = Colors.Red;
        public Color ColorParm
        {
            get { return _color; }
            set { _color = value; }
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}

