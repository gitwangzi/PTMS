using Gsafety.PTMS.Bases.Enums;
using System;
using System.Collections.Generic;
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
    public class BusinessAlertTypeConverter : IValueConverter
    {
        Dictionary<int, string> types = null;

        public BusinessAlertTypeConverter()
        {
            var temp = new EnumAdapter<BusinessAlertType>().GetEnumInfos();
            types = new Dictionary<int, string>();
            foreach (var item in temp)
            {
                types.Add(item.Value, item.LocalizedString);
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                //try
                //{
                int key = int.Parse(value.ToString());
                if (types.ContainsKey(key))
                {
                    return types[key];
                }
                //}
                //catch (Exception)
                //{
                //    return string.Empty;
                //}

            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
