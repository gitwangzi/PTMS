using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Gsafety.Common.Controls
{
    public class PopupHandler
    {
        public static void OnDropDown(object sender, EventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(box); i++)
            {
                var control = VisualTreeHelper.GetChild(box, i) as FrameworkElement;
                if (control is Grid)
                {
                    Grid grid = control as Grid;

                    foreach (var item in grid.Children)
                    {
                        if (item is Popup)
                        {
                            Popup pop = item as Popup;

                            pop.MouseRightButtonDown += popup_MouseRightButtonDown;
                            pop.Child.MouseRightButtonDown += popup_MouseRightButtonDown;
                        }

                    }
                }
            }
        }

        public static void OnTimerDropDown(object sender, EventArgs e)
        {
            TimePicker box = (TimePicker)sender;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(box); i++)
            {
                var control = VisualTreeHelper.GetChild(box, i) as FrameworkElement;
                if (control is Grid)
                {
                    Grid grid = control as Grid;

                    foreach (var item in grid.Children)
                    {
                        if (item is Popup)
                        {
                            Popup pop = item as Popup;

                            pop.MouseRightButtonDown += popup_MouseRightButtonDown;
                            pop.Child.MouseRightButtonDown += popup_MouseRightButtonDown;
                        }
                        else if (item is TimeUpDown)
                        {
                            TimeUpDown tup = item as TimeUpDown;
                            for (int j = 0; j < VisualTreeHelper.GetChildrenCount(tup); j++)
                            {
                                var childcontrol = VisualTreeHelper.GetChild(tup, j) as FrameworkElement;
                                if (childcontrol is Grid)
                                {
                                    Grid childgrid = childcontrol as Grid;

                                    foreach (var childitem in childgrid.Children)
                                    {
                                        if (childitem is Popup)
                                        {
                                            Popup childpop = childitem as Popup;

                                            childpop.MouseRightButtonDown += popup_MouseRightButtonDown;
                                            childpop.Child.MouseRightButtonDown += popup_MouseRightButtonDown;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void popup_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
