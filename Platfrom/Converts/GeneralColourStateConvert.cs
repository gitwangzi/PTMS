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
    public class GeneralColourStateConvert : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.ToString() == "OnlineRateColour")
            {

                if (null != value && !string.IsNullOrEmpty(value.ToString()))
                {

                    double count = double.Parse(value.ToString());

                    if (count >= 80)
                    {
                        return new SolidColorBrush(Colors.Green);
                    }
                    else if (count > 40 && count < 80)
                    {
                        return new SolidColorBrush(Colors.Orange);
                    }
                    else
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                }

            }
            else if (parameter.ToString() == "VehicleLoadCompleteColour")
            {
                if (null != value && (bool)value)
                {
                    return new SolidColorBrush(Colors.White);
                }
                return new SolidColorBrush(Colors.Red);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
