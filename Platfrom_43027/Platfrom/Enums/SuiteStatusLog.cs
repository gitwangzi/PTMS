using System;
using System.Net;
using System.Windows;
using System.Linq;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Gsafety.Common.Localization.Resource;
namespace Gsafety.PTMS.Bases.Enums
{
    public enum SuiteStatusLog
    {
        /// <summary>
        /// Initial
        /// </summary>
        [EnumAttribute(ResourceName = "Initial")]
        Initial = 10,
        /// <summary>
        ///  Testing
        /// </summary>
        [EnumAttribute(ResourceName = "Testing")]
        Testing = 22,
        /// <summary>
        /// Running
        /// </summary>
        [EnumAttribute(ResourceName = "Running")]
        Running = 23,
        /// <summary>
        /// Abnormal
        /// </summary>
        [EnumAttribute(ResourceName = "Abnormal")]
        Abnormal = 24,
        /// <summary>
        /// Maintenance 
        /// </summary>
        [EnumAttribute(ResourceName = "Maintenance")]
        Maintenance = 30,
        /// <summary>
        /// Scrap
        /// </summary>
        [EnumAttribute(ResourceName = "Scrap")]
        Scrap = 40,
    }
         public class SuiteStatusLogConverter : EnumAdapter<SuiteStatusLog>, IValueConverter
    {
        IList<EnumInfos> info = new List<EnumInfos>();
        public SuiteStatusLogConverter()
        {
            info = this.GetEnumInfos();
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return info.Where(f => f.Name.Equals(value.ToString())).First().LocalizedString;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    }

