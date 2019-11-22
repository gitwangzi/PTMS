﻿using Gsafety.PTMS.Share;
using System;
using System.Globalization;
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
    public class AlarmLevelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                switch (value.ToString())
                {
                    case "1":
                        return ApplicationContext.Instance.StringResourceReader.GetString("IncidentCommon");
                    case "2":
                        return ApplicationContext.Instance.StringResourceReader.GetString("IncidentLarger");
                    case "3":
                        return ApplicationContext.Instance.StringResourceReader.GetString("IncidentMajor");
                    case "4":
                        return ApplicationContext.Instance.StringResourceReader.GetString("IncidentSpecialSignificant");
                    default:
                        return string.Empty;
                }
            }
            return string.Empty;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return parameter.ToString();
            }
            return null;
        }
    }
}
