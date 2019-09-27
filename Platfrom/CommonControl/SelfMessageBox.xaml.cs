using System.Windows;
using System.Windows.Controls;

namespace Gsafety.Common.Controls
{
    public partial class SelfMessageBox : ChildWindow
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
                this.Tag = value;
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

        private readonly static DependencyProperty IsHavCancelButtonProperty = DependencyProperty.Register("IsHavCancelButton", typeof(bool), typeof(SelfMessageBox), new PropertyMetadata(false, OnIsHavCancelButtonChanged));

        private static void OnIsHavCancelButtonChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SelfMessageBox selfMessageBox = sender as SelfMessageBox;
            if ((bool)e.NewValue == true)
            {
                selfMessageBox.CancelButton.Visibility = Visibility.Visible;
            }
            else
            {
                selfMessageBox.CancelButton.Visibility = Visibility.Collapsed;
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

        public SelfMessageBox()
        {
            InitializeComponent();
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
    }
}

