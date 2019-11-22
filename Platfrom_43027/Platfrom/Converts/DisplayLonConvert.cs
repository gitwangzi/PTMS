/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 045ee7b2-2c47-4559-a99c-3259b9c97abb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-LILF
/////                 Author: TEST(lilf)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: DisplayLonConvert
/////          Class Version: v1.0.0.0
/////            Create Time: 2013/9/4 11:45:17
/////      Class Description:  
/////======================================================================
/////          Modified Time: 2013/9/4 11:45:17
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
    public class DisplayLonConvert : IValueConverter
    {
        /// <summary>
        /// 显示为小数点后6位
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            #region
            //input coordinate convert to “dufenfen.fenfenfenfen” geographical coordinates format
            //if (value == null) return "";
            //string temp = value.ToString();
            //int indflag = temp.IndexOf("-");
            //temp = temp.Replace("-", "");

            //double dblon = double.Parse(temp.ToString());
            //int du = (int)(Math.Floor(dblon));

            //dblon = dblon - du;
            //dblon = dblon * 60;
            //int fen = (int)(Math.Floor(dblon));
            //if (indflag > -1)//负数
            //    return "-" + du.ToString("##0") + " " + fen.ToString("00") + (dblon - fen).ToString("#.000000");
            //else
            //    return du.ToString("##0") + " " + fen.ToString("00") + (dblon - fen).ToString("#.000000");

            //input coordinates are converted to decimal degrees
            #endregion
            if ((value == null) || (value.ToString() == "")) return "";
            //if (value.ToString().Equals("0")) return "0";
            return (double.Parse(value.ToString())).ToString("f6");          
        }
        /// <summary>
        /// 将接收的GPS位置，进行本地化。本地位才可以进行运算、显示
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
            //string temp = value.ToString().Replace(".", System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
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
        /// <summary>
        /// 将本地化的数据格式转化为存储到服务器上的格式
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
            object temp=Convert(value,targetType,parameter,culture);
            if ((temp == null) || (temp.ToString() == "")) return "";
            if (temp.ToString().IndexOf('-') > -1)
            {
                return temp.ToString().Replace('-', 'W');
            }
            else
            {
                return "E"+temp.ToString();
            }
        }
    }
}
