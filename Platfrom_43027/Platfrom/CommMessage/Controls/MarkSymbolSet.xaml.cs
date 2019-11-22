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
    public partial class MarkSymbolSet : ChildWindow
    {
        public MarkSymbolSet()
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
                nSize = Convert.ToInt32(markSymbolSize.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidSize"));
                return;
            }
            if (nSize <= 0 || nSize > 30)
            {
                MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidSize"));
                return;
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        /// <summary>
        /// 大小
        /// </summary>
        private int nSize = 5;
        public int SymbolSize
        {
            get { return nSize; }
            set { nSize = value; }
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
    }
}

