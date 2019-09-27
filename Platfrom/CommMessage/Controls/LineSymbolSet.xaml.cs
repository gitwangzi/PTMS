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
    public partial class LineSymbolSet : ChildWindow
    {
        public LineSymbolSet()
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
            ColorParm = (FillColor.SelectedItem as PredefinedColor).Value;

            try
            {
                _dLineWidth = Convert.ToDouble(linewidth.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidWidth"));
                return;
            }
            if (_dLineWidth <= 0 || _dLineWidth > 30)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidWidth"));
                return;
            }
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
            //默认红色
            if (PredefinedColors.PredefinedColorCollection.Count > 1)
            {
                FillColor.SelectedValue = PredefinedColors.PredefinedColorCollection[1];
            }

        }

        /// <summary>
        /// 宽度
        /// </summary>
        private double _dLineWidth = 5;
        public double LineWidthParm
        {
            get { return _dLineWidth; }
            set { _dLineWidth = value; }
        }
        /// <summary>
        /// 颜色
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

