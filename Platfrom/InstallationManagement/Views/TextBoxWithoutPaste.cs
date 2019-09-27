using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Installation.Views
{
    public class TextBoxWithoutPaste : TextBox
    {
        public bool IsControlKeyDown { get; set; }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Ctrl)
                IsControlKeyDown = true;
            if (IsControlKeyDown && e.Key == Key.V)
            {
                e.Handled = true;
            }
            else
                base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Ctrl)
                IsControlKeyDown = false;
            base.OnKeyUp(e);
        }
    }
}
