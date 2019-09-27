using Gsafety.Common.CommMessage;
using Gsafety.PTMS.Share;
using Jounce.Core.Event;
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

namespace Gsafety.Common.Controls
{
    public partial class SymbolStyleSet : ChildWindow
    {
        public SymbolStyleSet()
        {
            InitializeComponent();
            Initialize_Colors();
            InitfontCmb();

           // MarkColor.DropDownOpened += PopupHandler.OnDropDown;
           // MarkColor.DropDownClosed += PopupHandler.OnDropDown;

        }
        private void LayoutRoot_MouseRightButtonUp_1(object sender,

System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void InitfontCmb()
        {
            //获得系统字体列表 
        }

        /// <summary>
        /// 控制tab页可见性(设置序号对应的item不可见)
        /// </summary>
        /// <param name="tabitemindex"></param>
        /// <param name="tabSelectIndex"></param>
        public void ControlTabItemVisbility(int tabitemindex, int tabSelectIndex)
        {
            TabItem item = SymbolsetTab.Items[tabitemindex] as TabItem;
            if (item != null)
            {
                item.Visibility = System.Windows.Visibility.Collapsed;
            }
            TabItem selectitem = SymbolsetTab.Items[tabSelectIndex] as TabItem;
            if (selectitem != null)
            {
                selectitem.IsSelected = true;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if ((FillColor.SelectedItem as PredefinedColor) == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_SelectColor"), MessageDialogButton.Ok);
                return;
            }
            TansparentParm = trasparentSet.Value;
            FillColorParm = (FillColor.SelectedItem as PredefinedColor).Value;

            if ((LineColor.SelectedItem as PredefinedColor) == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_SelectColor"), MessageDialogButton.Ok);
                return;
            }
            LineColorParm = (LineColor.SelectedItem as PredefinedColor).Value;
            try
            {
                _dLineWidth = Convert.ToDouble(linewidth.Text);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidWidth"), MessageDialogButton.Ok);
                return;
            }
            if (_dLineWidth <= 0 || _dLineWidth > 30)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidWidth"), MessageDialogButton.Ok);
                return;
            }

            if ((MarkColor.SelectedItem as PredefinedColor) == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_SelectColor"), MessageDialogButton.Ok);
                return;
            }
            MarkColorParm = (MarkColor.SelectedItem as PredefinedColor).Value;

            try
            {
                nSize = Convert.ToInt32(markSymbolSize.Text);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidSize"), MessageDialogButton.Ok);
                return;
            }
            if (nSize <= 0 || nSize > 100)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidSize"), MessageDialogButton.Ok);
                return;
            }

            //if (FontNameList.SelectedValue == null)
            //{
            //    MessageBox.Show(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidFontName"));
            //    return;
            //}
            //FontName = FontNameList.SelectedValue.ToString();
            FontSize = -1;
            try
            {
                FontSize = Convert.ToInt32(textSymbolSize.Value);
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidFontSize"), MessageDialogButton.Ok);
                return;
            }
            if (FontSize <= 0 || FontSize > 100)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_InvalidFontSize"), MessageDialogButton.Ok);
                return;
            }
            if ((FontColor.SelectedItem as PredefinedColor) == null)
            {
                MessageBoxHelper.ShowDialog(ApplicationContext.Instance.StringResourceReader.GetString("GIS_Notice"), ApplicationContext.Instance.StringResourceReader.GetString("GIS_Symbol_SelectColor"), MessageDialogButton.Ok);
                return;
            }
            TextColorParm = (FontColor.SelectedItem as PredefinedColor).Value;
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

            LineColor.DataContext = PreDefinedColors;
            //默认红色
            if (PredefinedColors.PredefinedColorCollection.Count > 1)
            {
                LineColor.SelectedValue = PredefinedColors.PredefinedColorCollection[1];
            }


            MarkColor.DataContext = PreDefinedColors;
            //默认红色
            if (PredefinedColors.PredefinedColorCollection.Count > 1)
            {
                MarkColor.SelectedValue = PredefinedColors.PredefinedColorCollection[1];
            }

            FontColor.DataContext = PreDefinedColors;
            //默认红色
            if (PredefinedColors.PredefinedColorCollection.Count > 1)
            {
                FontColor.SelectedValue = PredefinedColors.PredefinedColorCollection[1];
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
        /// 颜色
        /// </summary>
        private Color _fillcolor = Colors.Red;
        public Color FillColorParm
        {
            get { return _fillcolor; }
            set { _fillcolor = value; }
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
        private Color _linecolor = Colors.Red;
        public Color LineColorParm
        {
            get { return _linecolor; }
            set { _linecolor = value; }
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
        private Color _markcolor = Colors.Red;
        public Color MarkColorParm
        {
            get { return _markcolor; }
            set { _markcolor = value; }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        private Color _textColor = Color.FromArgb(255, 117, 20, 99);
        public Color TextColorParm
        {
            get { return _textColor; }
            set { _textColor = value; }
        }
        /// <summary>
        /// 字体名称
        /// </summary>
        private string _strFontName = "Microsoft YaHei";
        public string FontName
        {
            get { return _strFontName; }
            set { _strFontName = value; }
        }
        /// <summary>
        /// 字体大小
        /// </summary>
        private int _FontSize = 12;
        public int FontSize
        {
            get { return _FontSize; }
            set { _FontSize = value; }
        }
        private void ChildWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

    }
}

