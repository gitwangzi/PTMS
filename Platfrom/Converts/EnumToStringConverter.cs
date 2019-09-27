/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: e7b1bc92-d6b4-4de8-bdb6-268fac180d92      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LANQ
/////                 Author: TEST(lanq)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: EnumToStringConverter
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/4 17:41:16
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/4 17:41:16
/////            Modified by:
/////   Modified Description: 
/////======================================================================
using System;
using System.Net;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Gsafety.Common.Localization.Resource;
using System.ComponentModel;
using Gsafety.PTMS.Share;
using Gsafety.PTMS.Bases.Enums;

namespace Gsafety.Common.Converts
{
    public class EnumToStringConverter<EnumType> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            EnumType type = (EnumType)Enum.Parse(typeof(EnumType), value.ToString(), true);
            return ApplicationContext.Instance.StringResourceReader.GetString(getDescription(type));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("unexpected Converback");
        }

        private string getDescription(EnumType i)
        {
            FieldInfo info = i.GetType().GetField(i.ToString());
            EnumAttribute[] enumAttributes = (EnumAttribute[])info.GetCustomAttributes(typeof(EnumAttribute), false);
            if (enumAttributes.Length > 0)
                return enumAttributes[0].Description;
            return i.ToString();
        }
    }
}
