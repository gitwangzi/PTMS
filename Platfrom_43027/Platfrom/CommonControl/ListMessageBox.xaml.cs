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

namespace Gsafety.Common.Controls
{
    public partial class ListMessageBox : ChildWindow
    {
          /// <summary>
        /// 提示信息
        /// </summary>
        public string MessageText
        {
            get
            {
                return this.Tag.ToString();
            }
            set
            {
                string msg = value;
                List<string> list = new List<string>();
                list = msg.Split(';').ToList();
                this.Tag = list;
            }
        }
        
        /// <summary>
        /// 是否有取消按钮
        /// </summary>
        public bool IsHavCancelButton
        {
            get
            {
                return (bool)GetValue(IsHavCancelButtonProperty);
            }
            set
            {
                SetValue(IsHavCancelButtonProperty, value);
            }
        }

        private readonly static DependencyProperty IsHavCancelButtonProperty = DependencyProperty.Register("IsHavCancelButton", typeof(bool), typeof(ListMessageBox), new PropertyMetadata(false, OnIsHavCancelButtonChanged));

        private static void OnIsHavCancelButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ListMessageBox listMessageBox = sender as ListMessageBox;
            if ((bool)e.NewValue == true)
            {
                listMessageBox.CancelButton.Visibility = Visibility.Visible;
            }
            else
            {
                listMessageBox.CancelButton.Visibility = Visibility.Collapsed;
            }
        }

        #region 方法重写
        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, true);
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            Application.Current.RootVisual.SetValue(Control.IsEnabledProperty, false);
        }
        #endregion

        public ListMessageBox()
        {
            InitializeComponent();
            this.listBox.SelectedIndex = -1;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            //base.OnMouseRightButtonDown(e);
            e.Handled = true;
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.listBox.SelectedIndex = -1;
        }
    }
}

