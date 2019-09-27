/////Copyright (C) Gsafety 2013 .All Rights Reserved.
/////======================================================================
/////                   Guid: 045ee7b2-2c47-4559-a99c-3259b9c97abb      
/////             clrversion: 4.0.30319.17929
/////Registered organization: 
/////           Machine Name: PC-DZL
/////                 Author: TEST(dengzl)
/////======================================================================
/////           Project Name: Gsafety.Common.Converts
/////    Project Description:    
/////             Class Name: GPSLatChangeDisplayConvert
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
    public class GPSLatChangeDisplayConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return "";
                object toResult = LatStrToDouble(value);

                //if (toResult == null || string.IsNullOrEmpty(toResult.ToString()))
                //    return "";
                //string temp = toResult.ToString();
                //int indflag = temp.IndexOf("-");
                //temp = temp.Replace("-", "");
                //double dblat = double.Parse(temp.ToString());
                //int du = (int)(Math.Floor(dblat));

                //dblat = dblat - du;
                //dblat = dblat * 60;
                //int fen = (int)(Math.Floor(dblat));
                //if (indflag > -1)//
                //    return "-" + du.ToString("#0") + " " + fen.ToString("00") + (dblat - fen).ToString("#.000000");
                //else
                //    return du.ToString("#0") + " " + fen.ToString("00") + (dblat - fen).ToString("#.000000");
                if ((toResult == null) || (toResult.ToString() == "")) return "";
               // if (toResult.ToString().Equals("0")) return "0";
                if (toResult.ToString() == "-") return "-";
                return (double.Parse(toResult.ToString())).ToString("f6");         
            }
            catch (Exception ex)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return LatStrToDouble(value);
        }

        private object LatStrToDouble(object value)
        {

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
            //    du = int.Parse(temp.Substring(0, ind - 3 + 1));
            //    fen = double.Parse(temp.Substring(ind - 2));
            //}
            //else
            //{
            //    fen = double.Parse(temp);
            //}

            //if (indflag > -1) 
            //    return (-du - fen / 60);
            //else 
            //    return (du + fen / 60);

        }
    }
}