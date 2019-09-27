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
using System.Collections.Generic;
using Gsafety.Common.Localization.Resource;
using System.Windows.Data;
using System.ComponentModel;
using System.Linq;


namespace Gsafety.PTMS.Bases.Enums
{
    public enum StatusChange
    {
        /// <summary>
        /// No_Operation
        /// </summary>
        [EnumAttribute(ResourceName = "No_Operation")]
        No_Operation = 5,
        /// <summary>
        /// Initial
        /// </summary>
        [EnumAttribute(ResourceName = "Initial")]
        Initial = 10,
        /// <summary>
        /// Testing 
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
    public class StatusToTextConverter : EnumAdapter<StatusChange>, IValueConverter
    {
        IList<EnumInfos> infoss = new List<EnumInfos>();
        public StatusToTextConverter()
        {
            infoss = base.GetEnumInfos();   
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToInt32(value) == 10 || System.Convert.ToInt32(value) == 22
                        || System.Convert.ToInt32(value) == 23 || System.Convert.ToInt32(value) == 24
                        || System.Convert.ToInt32(value) == 30 || System.Convert.ToInt32(value) == 40)
            {
                //10:Inital  20:working  22：testing  23: running 24:abnormal 25:waiting repair 30:repair 40:crap 99:history
                // abnormal to running or repair 
                // running to abnormal
                // repair to initial  or crap
                int type = System.Convert.ToInt32(value);           
                switch (type)
                {
                    case 10:
                        type = 5;
                        break;
                    case 23:
                        type = 24;
                        break;
                    case 22:
                        type = 5;
                        break;
                    case 24:
                        if (System.Convert.ToInt32(parameter) == 0)
                            type = 23;
                        else
                            type = 30;
                        break;
                    case 30:
                        if (System.Convert.ToInt32(parameter) == 0)
                            type = 10;
                        else
                            type = 40;
                        break;
                    case 40:
                        type = 5;
                        break;
                }                         
                return type;
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class StatusChangeConverter : EnumAdapter<StatusChange>, IValueConverter
    {

        IList<EnumInfos> infoss = new List<EnumInfos>();
        public StatusChangeConverter()
        {
            infoss = base.GetEnumInfos();   //全局缓存
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //value为枚举值
            
            if (System.Convert.ToInt32(value) == 10 || System.Convert.ToInt32(value) == 22
                        || System.Convert.ToInt32(value) == 23 || System.Convert.ToInt32(value) == 24
                        || System.Convert.ToInt32(value) == 30 || System.Convert.ToInt32(value) == 40)
            {
                int type = System.Convert.ToInt32(value);
                switch (type)
                {
                    case 10:
                        type = 5;
                        break;
                    case 23:
                        type = 24;
                        break;
                    case 22:
                        type = 5;
                        break;
                    case 24:
                        if (System.Convert.ToInt32(parameter) == 0)
                            type = 23;
                        else
                            type = 30;
                        break;
                    case 30:
                        if (System.Convert.ToInt32(parameter) == 0)
                            type = 10;
                        else
                            type = 40;
                        break;
                    case 40:
                        type = 5;
                        break;
                }
                string unDefined = StringResource.ResourceManager.GetString("UnDefined");  //数据库未定义
                return type == 0 ? (string.IsNullOrEmpty(unDefined) ? "UnDefined" : unDefined) : infoss.Where(f => f.Value.Equals(type)).First().LocalizedString;
            }
            return string.Empty;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }

    public class VisibleConverter : EnumAdapter<StatusChange>, IValueConverter
    {
        IList<EnumInfos> infoss = new List<EnumInfos>();
        public VisibleConverter()
        {
            infoss = base.GetEnumInfos();   //全局缓存
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility ifVisible= Visibility.Visible ;
            
            //value为枚举值
            if (value != null)
            {
                int type = System.Convert.ToInt32(value);
                switch (type)
                {
                    case 10:
                        type = 5;
                        ifVisible = Visibility.Collapsed;
                        break;
                    case 23:
                        type = 24;
                        if (System.Convert.ToInt32(parameter) == 1)
                            ifVisible = Visibility.Collapsed;
                        break;
                    case 22:
                        type = 5;
                        ifVisible = Visibility.Collapsed;
                        break;
                    case 24:
                        if (System.Convert.ToInt32(parameter) == 0)
                            type = 23;
                        else
                            type = 30;
                        break;
                    case 30:
                        if (System.Convert.ToInt32(parameter) == 0)
                            type = 10;
                        else
                            type = 40;
                        break;
                    case 40:
                            type = 5;
                            ifVisible = Visibility.Collapsed;
                        break;
                }
              
            }
            return ifVisible;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
    public class ImageConverter : EnumAdapter<StatusChange>, IValueConverter
    {
        IList<EnumInfos> infoss = new List<EnumInfos>();
        public ImageConverter()
        {
            infoss = base.GetEnumInfos();   //全局缓存
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s="";
            //value为枚举值
            if (value != null)
            {
                
                int type = System.Convert.ToInt32(value);
                switch (type)
                {
                    case 10:
                        type = 5;                       
                        break;
                    case 23:
                        type = 24;
                        s = "/ExternalResource;component/Images/DataGrid_abnormal.png";
                        break;
                    case 22:
                        type = 5;                        
                        break;
                    case 24:
                        if (System.Convert.ToInt32(parameter) == 0)
                        {
                            type = 23;
                            s = "/ExternalResource;component/Images/DataGrid_working.png";
                        }
                        else
                        {
                            type = 30;
                            s = "/ExternalResource;component/Images/DataGrid_repair.png";
                        }
                        break;
                    case 30:
                        if (System.Convert.ToInt32(parameter) == 0)
                        {
                            type = 10;
                            s = "/ExternalResource;component/Images/DataGrid_Not_installed.png";
                        }
                        else
                        {
                            type = 40;
                            s = "/ExternalResource;component/Images/DataGrid_Scrap.png";
                        }
                        break;
                    case 40:
                        type = 5;                       
                        break;
                }

            }
            return s;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
