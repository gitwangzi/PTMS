/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: fb857d03-85c8-4331-a76d-3e3c7033fbd4      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: DisplayLatConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/4 11:43:48
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/4 11:43:48
/////            Modified by:
/////   Modified Description: 
/////======================================================================
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
    public class DisplayLatConvert : IValueConverter
    {
        /// <summary>
        /// display coordinate format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ////the unit for the input coordinate degrees decimal degrees
            //if (value == null) return "";
            //string temp = value.ToString();
            //int indflag = temp.IndexOf("-");
            //temp = temp.Replace("-", "");

            //double dblat = double.Parse(temp.ToString());
            //int du = (int)(Math.Floor(dblat));

            //dblat = dblat - du;
            //dblat = dblat * 60;
            //int fen = (int)(Math.Floor(dblat));
            //if (indflag > -1)
            //    return "-" + du.ToString("#0") + " " + fen.ToString("00") + (dblat - fen).ToString("#.000000");
            //else
            //    return du.ToString("#0") + " " + fen.ToString("00") + (dblat - fen).ToString("#.000000");

            //input coordinates are converted to decimal degrees
            if ((value == null)|| (value.ToString()=="")) return "";
           // if (value.ToString().Equals("0")) return "0";
            return (double.Parse(value.ToString())).ToString("f6");            
        }

        /// <summary>
        /// the coordinates to decimal degrees
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
            //要求value值必须是类似于"123.12"的格式,适用于经纬度,速度的转换.
            string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            return temp;
            //int indflag = temp.IndexOf("-");
            //temp = temp.Replace("-", "");

            //int du = 0;
            //double fen = 0;

            //int ind = temp.IndexOf(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            //if (ind == -1)
            //{
            //    ind = temp.Length;
            //}
            //if ((ind - 3 + 1) >= 1)
            //{
            //    if (temp.Substring(0, ind - 3 + 1) != "") du = int.Parse(temp.Substring(0, ind - 3 + 1));
            //    if ((temp.Substring(ind - 2)) != "") fen = double.Parse(temp.Substring(ind - 2));
            //}
            //else
            //{
            //    fen = double.Parse(temp);
            //}

            //if (indflag > -1) return (-du - fen / 60).ToString();
            //else return (du + fen / 60).ToString();
        }
        public object ConvertToSave(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((value == null) || (value.ToString() == "")) return "";
            string temp = (double.Parse(value.ToString())).ToString("f6").Replace(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator, ".");
            return temp;
        }
        /// <summary>
        /// 以WESN方式显示坐标
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertToWESN(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object temp = Convert(value, targetType, parameter, culture);
            if ((temp == null) || (temp.ToString() == "")) return "";
            if (temp.ToString().IndexOf('-') > -1)
            {
                return temp.ToString().Replace('-', 'S');
            }
            else
            {
                return "N" + temp.ToString();
            }
        }
    }
}
