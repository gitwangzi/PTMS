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

namespace Gsafety.Common.Converts
{
    public class OrderClientEndDateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    DateTime date = (DateTime)value;
                    if (date==DateTime.Parse("01/01/0001 23:59:59"))
                        //return new SolidColorBrush(Colors.Red);
                        return "#ed1b45";
                    else if (date.Date.AddDays(-3) <= DateTime.Today.Date)
                    {
                        return "#ed1b45";
                    }
                    else
                        //return new SolidColorBrush(Colors.White);
                        return "#333333";
                }
            }
            finally
            {
            }
            return "#333333";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
