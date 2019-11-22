using Gsafety.PTMS.Share;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Controls
{
    public class SearchTextBox : TextBox
    {
        private string _defaultString = string.Empty;

        public SearchTextBox()
        {
            this.Loaded += SearchTextBox_Loaded;

            this.GotFocus += SearchTextBox_GotFocus;
            this.LostFocus += SearchTextBox_LostFocus;
        }

        void SearchTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var binding = this.GetBindingExpression(TextBox.TextProperty).ParentBinding;
            _defaultString = ApplicationContext.Instance.StringResourceReader.GetString(binding.ConverterParameter as string);

            this.Loaded -= SearchTextBox_Loaded;
        }

        void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                this.Text = _defaultString;
            }
        }

        void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (string.Equals(this.Text, _defaultString, StringComparison.CurrentCultureIgnoreCase))
            {
                this.Text = string.Empty;
            }
        }
    }
}
