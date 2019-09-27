using System;
using System.Net;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.PTMS.Share
{
    public class ButtonServices
    {
        #region DefaultButton

        public static Button GetDefaultButton(DependencyObject obj)
        {
            return (Button)obj.GetValue(null);
        }

        public static void SetDefaultButton(DependencyObject obj, Button value)
        {
            obj.SetValue(DefaultButtonProperty, value);
        }

        public static readonly DependencyProperty DefaultButtonProperty =
            DependencyProperty.RegisterAttached("DefaultButton", typeof(Button), typeof(ButtonServices), new PropertyMetadata(OnDefaultButtonChanged));

        public static void OnDefaultButtonChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UIElement scopeElement = obj as UIElement;
            if (scopeElement != null)
            {
                scopeElement.KeyDown += new KeyEventHandler(Element_EnterKeyUp);
            }
        }

        static void Element_EnterKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UIElement scopeElement = sender as UIElement;
                if (scopeElement != null)
                {
                    Button defaultButton = scopeElement.GetValue(ButtonServices.DefaultButtonProperty) as Button;
                    defaultButton.Focus();
                    defaultButton.PerfomClick();
                }
                e.Handled = true;
            }
           
        }

        #endregion 

        #region CancelButton

        public static Button GetCancelButton(DependencyObject obj)
        {
            return (Button)obj.GetValue(null);
        }

        public static void SetCancelButton(DependencyObject obj, Button value)
        {
            obj.SetValue(CancelButtonProperty, value);
        }

        public static readonly DependencyProperty CancelButtonProperty =
            DependencyProperty.RegisterAttached("CancelButton", typeof(Button), typeof(ButtonServices), new PropertyMetadata(OnCancelButtonChanged));

        public static void OnCancelButtonChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UIElement scopeElement = obj as UIElement;
            if (scopeElement != null)
            {
                scopeElement.KeyDown += new KeyEventHandler(Element_EscKeyUp);
            }
        }

        static void Element_EscKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                UIElement scopeElement = sender as UIElement;
                if (scopeElement != null)
                {
                    Button defaultButton = scopeElement.GetValue(ButtonServices.CancelButtonProperty) as Button;
                    defaultButton.PerfomClick();
                }
            }
        }

        #endregion 
    }

    public static class ButtonExtension
    {
        public static void PerfomClick(this Button clickBtn)
        {
            if (clickBtn != null && clickBtn.IsEnabled)
            {
                var peer = new ButtonAutomationPeer(clickBtn);
                var invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                if (invokeProv != null)
                {
                    invokeProv.Invoke();
                    clickBtn.Focus();
                    
                }
            }
        }
    }

}
